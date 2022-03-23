using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace PeliprojektiExamples.UI
{
	public class AudioControl : MonoBehaviour
	{
		private AudioMixer mixer;
		private Slider slider;
		private string volumeName;

		private void Awake()
		{
			slider = GetComponentInChildren<Slider>();
		}

		public void Setup(AudioMixer mixer, string volumeName)
		{
			this.mixer = mixer;
			this.volumeName = volumeName;

			if (mixer.GetFloat(volumeName, out float volume))
			{
				slider.value = ToLinear(volume);
			}
		}

		public void Save()
		{
			mixer.SetFloat(volumeName, ToDB(slider.value));
		}

		private float ToDB(float linear)
		{
			return linear <= 0 ? -144.0f : 20f * Mathf.Log10(linear);
		}

		private float ToLinear(float dB)
		{
			return Mathf.Clamp01(Mathf.Pow(10.0f, dB / 20.0f));
		}
	}
}
