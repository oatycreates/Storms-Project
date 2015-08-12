/**
 * File: AirshipAudio.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script controls the creaks and groans of the wood on the airship as it moves.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This script controls the creaks and groans of the wood on the airship as it moves.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AirshipAudio : MonoBehaviour 
{
	private AudioSource m_mySource;
	public AudioClip[] woodenSounds;
	//private float timer;
	private bool m_movement = false;

	void Start () 
	{
		m_mySource = gameObject.GetComponent<AudioSource>();
		m_mySource.volume = 0.20f;
		RandomMe();
	}
	
	void Update () 
	{		
		// If movement is True, play a sound.
		// If that sound finishes, and movement is Still True, play a new sound.
		if (m_movement)
		{
			// If source is Not Playing
			if (!m_mySource.isPlaying)
			{
				RandomMe();
				m_mySource.Play();
			}
			
		}
		else
		if (!m_movement)
		{
			m_mySource.Stop();
			RandomMe();
		}
		
	}
	
	public void AudioInputs(float a_pitch, float a_yaw, float a_roll)//, float a_speed)
	{
		if ((a_pitch != 0) || (a_yaw != 0) || (a_roll != 0))
		{
			m_movement = true;
		}
		else
		{
			m_movement = false;
		}
	}
	
	void RandomMe()
	{
		// Random clip from array
		m_mySource.clip = woodenSounds[Random.Range(0, woodenSounds.Length)];
		// Give it a random pitch
		m_mySource.pitch = Random.Range(0.75f, 1.5f);
	}
}
