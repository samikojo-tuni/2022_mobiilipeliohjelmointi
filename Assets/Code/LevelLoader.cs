using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PeliprojektiExamples
{
	public class LevelLoader : MonoBehaviour
	{
		public enum LoadingState
		{
			None,
			Started,
			InProgress,
			Options
		}

		public const string LoaderName = "Loader";
		public const string OptionsName = "Options";

		public static LevelLoader Current
		{
			get;
			private set;
		}

		private LoadingState state = LoadingState.None;

		// Viittaus alkuperäiseen sceneen
		private Scene originalScene;
		// Seuraavan scenen nimi
		private string nextSceneName;
		// Viittaus loading-sceneen
		private Scene loadingScene;
		private Scene optionsScene;

		// Nk. Singleton, eli tästä oliosta voi olla vain yksi kopio olemassa kerralla
		private void Awake()
		{
			if (Current == null)
			{
				Current = this;
			}
			else
			{
				// LevelLoader on jo olemassa! Tuhotaan uusi instanssi
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);
		}

		private void OnEnable()
		{
			// Aletaan kuunnella eventtiä
			SceneManager.sceneLoaded += OnLevelLoaded;
		}

		private void OnDisable()
		{
			// Lopetetaan eventin kuuntelu
			SceneManager.sceneLoaded -= OnLevelLoaded;
		}

		public void LoadOptions()
		{
			// Pysäytä peli
			Time.timeScale = 0;
			state = LoadingState.Options;
			SceneManager.LoadSceneAsync(OptionsName, LoadSceneMode.Additive);
		}

		public void CloseOptions()
		{
			state = LoadingState.None;
			SceneManager.UnloadSceneAsync(optionsScene);
			Time.timeScale = 1; // Palauta pelin normaalinopeus
		}

		public void LoadLevel(string sceneName)
		{
			nextSceneName = sceneName;
			originalScene = SceneManager.GetActiveScene();
			state = LoadingState.Started;
			// Ladataan loading screen additiivisesti (nykyisen scenen rinnalle)
			SceneManager.LoadSceneAsync(LoaderName, LoadSceneMode.Additive);
		}

		// Suoritetaan, kun scene on ladattu
		private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
		{
			switch (state)
			{
				case LoadingState.Started:
					loadingScene = scene;
					// Aloitetaan Fade animaatio
					GameObject[] rootObjects = loadingScene.GetRootGameObjects(); // Palauttaa scenen kaikki root-GameObjectit
					foreach (GameObject item in rootObjects)
					{
						Fader fader = item.GetComponentInChildren<Fader>();
						if (fader != null)
						{
							float fadeTime = fader.FadeIn();
							StartCoroutine(ContinueLoad(fadeTime));

							break; // Poistuu loopista
						}
					}
					break;
				case LoadingState.InProgress:
					foreach (GameObject item in loadingScene.GetRootGameObjects())
					{
						Fader fader = item.GetComponentInChildren<Fader>();
						if (fader != null)
						{
							float fadeTime = fader.FadeOut();
							StartCoroutine(FinalizeLoad(fadeTime));

							state = LoadingState.None;

							break; // Poistuu loopista
						}
					}
					break;
				case LoadingState.Options:
					optionsScene = scene;
					break;
			}
		}

		private IEnumerator FinalizeLoad(float waitTime)
		{
			yield return new WaitForSeconds(waitTime);

			SceneManager.UnloadSceneAsync(loadingScene);
		}

		private IEnumerator ContinueLoad(float waitTime)
		{
			yield return new WaitForSeconds(waitTime); // Odottaa waitTime:n verran

			// Suoritus jatkuu waitTime:n kuluttua
			// Näyttö on musta, joten pelaaja ei enää näe alkuperäistä sceneä.
			// Unloadataan se
			SceneManager.UnloadSceneAsync(originalScene);
			// Ladataan seuraava scene
			SceneManager.LoadSceneAsync(nextSceneName, LoadSceneMode.Additive);
			state = LoadingState.InProgress;
		}
	}
}
