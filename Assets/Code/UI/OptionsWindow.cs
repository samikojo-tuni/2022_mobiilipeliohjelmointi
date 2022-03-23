using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace PeliprojektiExamples.UI
{
	public class OptionsWindow : MonoBehaviour
	{
		[SerializeField] private AudioMixer mixer;
		[SerializeField] private AudioControl musicControl;
		[SerializeField] private AudioControl sfxControl;
		[SerializeField] private string musicVolName;
		[SerializeField] private string sfxVolName;

		private void Start()
		{
			musicControl.Setup(mixer, musicVolName);
			sfxControl.Setup(mixer, sfxVolName);
		}

		public void Close()
		{
			musicControl.Save();
			sfxControl.Save();
			LevelLoader.Current.CloseOptions();
		}
	}
}
