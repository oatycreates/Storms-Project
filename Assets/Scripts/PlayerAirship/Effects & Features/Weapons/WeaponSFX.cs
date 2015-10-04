/**
 * File: WeaponSFX.cs
 * Author: Rowan Donaldson
 * Maintainer: Pat Ferguson
 * Created: 04/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Triggers attached audio.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	[RequireComponent(typeof(AudioSource))]
	/// <summary>
	/// Weapon SFX - trigger the attached audio.
	/// </summary>
	public class WeaponSFX : MonoBehaviour 
	{
		private AudioSource m_Audio;
		public AudioClip sfx;
		
		public bool makeNoiseOnSpawn = false;
		public bool loopOnSpawn = false;
		public bool randomPitchOnSpawn = false;
		
		private float startingPitch;
		private float startingVolume;
	
		void Awake() 
		{
			if (gameObject.GetComponent<AudioSource>() == null)
			{
				gameObject.AddComponent<AudioSource>();
			}	
			
			m_Audio = gameObject.GetComponent<AudioSource>();
			
			//Take reference of pitch.
			startingPitch = m_Audio.pitch;
			//Take reference of volume.
			startingVolume = m_Audio.volume;
			
		}
		
		void OnEnable()
		{
			if (makeNoiseOnSpawn)
			{
				PlaySound(loopOnSpawn, randomPitchOnSpawn);
			}
		}
		
		public void SetSound(AudioClip a_overrideClip, bool a_looping, bool a_randomPitch)
		{
			m_Audio.clip = a_overrideClip;
			m_Audio.loop = a_looping;
			
			if (a_randomPitch)
			{
				m_Audio.pitch = Random.Range(0.75f, 1.25f);
			}
			
			m_Audio.Play();	
		}
		
		public void PlaySound(bool a_looping, bool a_randomPitch)
		{
			m_Audio.volume = startingVolume;
			//Always play my attached clip first
			m_Audio.clip = sfx;
			m_Audio.loop = a_looping;
			
			if (a_randomPitch)
			{
				m_Audio.pitch = Random.Range(0.75f, 1.25f);
			}
			else
			{
				m_Audio.pitch = startingPitch;
			}
			
			m_Audio.Play();	
		}
		
		//Reset
		void OnDisable()
		{
			if (m_Audio.isPlaying)
			{
				m_Audio.Stop();
			}
			m_Audio.volume = startingVolume;
			m_Audio.pitch = startingPitch;
		}
	}
}