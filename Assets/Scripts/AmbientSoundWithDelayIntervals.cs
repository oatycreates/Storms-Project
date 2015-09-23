/**
 * File: AmbientSound.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 23/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Similar to the Ambient Sound Script, this script takes an array of audio clips, and plays them with an interval in-between.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
		/// <summary>
		/// This script controls the ambient wind audio, fire sfx etc - It chooses ONE clip to loop throughout the level, but changes the pitch often.
		/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class AmbientSoundWithDelayIntervals : MonoBehaviour 
	{
		private AudioSource m_mySource;
		public int maxDelayTime = 20;
		public AudioClip[] sounds;
		private float delayTime = 0;
		
		void Start()
		{
			m_mySource = gameObject.GetComponent<AudioSource>();
			//m_mySource.volume = 0.3f;
			RandomClip();
		}
		
		void Update()
		{
			if (!m_mySource.isPlaying)
			{
				delayTime -= Time.deltaTime;
			}

			if (delayTime < 0)
			{
				RandomClip();
			}

			Debug.Log (delayTime);
		}
		
		void RandomClip()
		{
			// Random clip
			m_mySource.clip = sounds[Random.Range(0, sounds.Length)];
			
			RandomPitch();
			RandomDelayTime ();
		}
		
		void RandomPitch()
		{
			// Random pitch
			m_mySource.pitch = Random.Range(0.85f, 1.15f);
			// Play clip
			m_mySource.Play();
		}

		void RandomDelayTime()
		{
			delayTime = Random.Range (1, maxDelayTime);
		}
	}
}
