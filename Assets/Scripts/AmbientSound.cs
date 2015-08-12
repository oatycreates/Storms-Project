﻿/**
 * File: AmbientSound.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script controls the ambient wind audio - It chooses ONE clip to loop throughout the level, but changes the pitch often.
 **/

using UnityEngine;
using System.Collections;
/// <summary>
/// This script controls the ambient wind audio - It chooses ONE clip to loop throughout the level, but changes the pitch often.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class AmbientSound : MonoBehaviour 
{
	private AudioSource m_mySource;
	public AudioClip[] windSounds;

	void Start () 
	{
		m_mySource = gameObject.GetComponent<AudioSource>();
		m_mySource.volume = 0.3f;
		RandomClip();
	}
	
	void Update () 
	{	
		if (!m_mySource.isPlaying)
		{
			RandomPitch();
		}
	}
	
	void RandomClip()
	{
		// Random clip
		m_mySource.clip = windSounds[Random.Range(0, windSounds.Length)];
		
		RandomPitch();
	}
	
	void RandomPitch()
	{
		// Random pitch
		m_mySource.pitch = Random.Range(0.85f, 1.15f);
		// Play clip
		m_mySource.Play();
	}
}
