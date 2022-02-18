using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PeliprojektiExamples
{
	public class Spawner : MonoBehaviour
	{
		public enum State
		{
			WaitingForSpawn,
			WaitingForDestroy
		}

		[SerializeField, Tooltip("The time after which an object is spawned (in seconds)")]
		private float spawnTime;

		[SerializeField, Tooltip("A random offset for the spawn timer (in seconds)")]
		private float timerOffset;

		[SerializeField, Tooltip("A reference to the prefab we want to create copies from")]
		private GameObject prefab;

		private float timer;

		private State state = State.WaitingForSpawn;

		private GameObject spawnedObject;

		void Start()
		{
			timer = spawnTime + Random.Range(-timerOffset, timerOffset);
		}

		void Update()
		{
			// Early exit
			if (timer <= 0) return;

			timer -= Time.deltaTime;

			if (timer <= 0)
			{
				switch (state)
				{
					case State.WaitingForSpawn:
						// Ajastimen aika kului loppuun
						Spawn();
						break;
					case State.WaitingForDestroy:
						DoDestroy();
						break;
				}
				ChangeState();
			}
		}

		private void ChangeState()
		{
			// Vastaa if-else -rakennetta ja muuttujaan sijoittamista
			state = state == State.WaitingForDestroy
				? State.WaitingForSpawn
				: State.WaitingForDestroy;

			// Sama kuin yllä oleva
			// if (state == State.WaitingForDestroy)
			// {
			// 	state = State.WaitingForSpawn;
			// }
			// else
			// {
			// 	state = State.WaitingForDestroy;
			// }

			// Alustetaan timer uudelleen
			timer = spawnTime + Random.Range(-timerOffset, timerOffset);
		}

		private void DoDestroy()
		{
			Destroy(spawnedObject);
			spawnedObject = null;
		}

		// OnValidate varmistaa, että Unityssä muokattavien fieldien arvot ovat lailliset
		// Suoritetaan, kun fieldin arvoa muutetaan Unityssa
		private void OnValidate()
		{
			if (timerOffset > spawnTime)
			{
				timerOffset = spawnTime;
			}
			else if (timerOffset < 0)
			{
				timerOffset = 0;
			}

			if (spawnTime < 0)
			{
				spawnTime = 0;
			}
		}

		private void Spawn()
		{
			spawnedObject = Instantiate(prefab, transform.position, transform.rotation);
		}
	}
}
