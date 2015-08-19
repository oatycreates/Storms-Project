/**
 * File: FallingScream.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script takes in any number of audio clips, and plays one randomly while a passenger falls.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This script takes in any number of audio clips, and plays one randomly while a passenger falls.
/// </summary>
[RequireComponent (typeof(AudioSource))]
public class FallingScream : MonoBehaviour 
{
	private AudioSource m_mySource;
	public AudioClip[] sounds;
	public float timeFallingBeforeScream = 0.80f;
	private float m_fallTimer = 0.80f;
	[HideInInspector]
	public bool readyToScream = false;
	//private bool m_playing = false;
	
	void Start () 
	{
		m_mySource = gameObject.GetComponent<AudioSource>();
		
		//SERIOUS!!! THIS CAN'T BE TOO LOUD, OTHERWISE IT'LL LEAVE PEOPLE DEAF
		m_mySource.volume = 0.05f;
		//RandomSound();
	}
	
	void Update () 
	{
		if (readyToScream)
		{
			m_fallTimer -= Time.deltaTime;
		}
		
		// Trigger the scream, once the object has begun to fall
		if (m_fallTimer < 0.0f)
		{
			// Check if audio source is already playing
			if (!m_mySource.isPlaying)
			{
			/*
				if (playing = false)
				{
					RandomSound();
					playing = true;
				}
				*/
				RandomSound();
			}
		}
	}
	
	void OnCollisionStay(Collision a_other)
	{
		if (a_other.gameObject.tag != "Passengers")
		{
			m_mySource.Stop();
			readyToScream = false;
			// Use max time to scream here
			m_fallTimer = timeFallingBeforeScream;
			//playing = false;
		}
	}
	
	void OnCollisionExit(Collision a_other)
	{
		if (a_other.gameObject.tag != "Passengers")
		{
			readyToScream = true;	
		}
	}
	
	public void RandomSound()
	{	
		// Get a random sound from the list
		m_mySource.clip = sounds[Random.Range(0, sounds.Length)];
		// Give it a random pitch
		m_mySource.pitch = Random.Range(0.7f, 1.5f);
		// Play the clip
		m_mySource.Play();
	}
}
