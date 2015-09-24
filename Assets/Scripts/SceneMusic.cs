/**
 * File: AmbientSound.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 23/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script plays and loops the music in the scene
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	/// <summary>
	/// This script controls the scene's music
	/// </summary>
	[RequireComponent(typeof(AudioSource))]
	public class SceneMusic : MonoBehaviour 
	{
		public bool fadeMusicIn = false;
		public float maxMusicVolume = 0.5f;

		private AudioSource m_mySource;
		public AudioClip music;
		
		void Start()
		{
			m_mySource = gameObject.GetComponent<AudioSource>();
			m_mySource.volume = 0.0f;
			PlayClip ();
		}
		
		void Update()
		{
			if (fadeMusicIn)
			{
				if (m_mySource.volume < maxMusicVolume)
				{
					float tempVol = m_mySource.volume;
					tempVol = Mathf.Lerp(tempVol, maxMusicVolume, Time.deltaTime);
					m_mySource.volume = tempVol;
				}
			}

			if (!m_mySource.isPlaying)
			{
				PlayClip();
			}
		}

		public void PlayClip()
		{
			if (!m_mySource.isPlaying)
			{
				if (music != null)
				{
					m_mySource.clip = music;
					m_mySource.Play();
				}
				else
				{
					Debug.LogError("No music attached");
				}
			}
		}
	
	} 
}
