/**
 * File: EngineAudio.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script controls the engine volume of the Airship.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This script controls the engine volume of the airship.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class EngineAudio : MonoBehaviour 
{
	private AudioSource m_mySource;
	public AudioClip engineClip;
	
	private float m_volumeModifier;
	private float m_pitchModifier;
	
	[HideInInspector]
	public bool suicideMode = false;
	private float m_lerpSoundSpeed = 0.1f;
	
	public StateManager lookToTheStateManager;

	void Awake () 
	{
		m_mySource = gameObject.GetComponent<AudioSource>();
		m_mySource.clip = engineClip;
		
		
		m_mySource.loop = true;
	}
	
	void Start()
	{
		m_mySource.volume = 0.5f;
		m_mySource.pitch = 1.0f;
	}
	
	void OnEnable()
	{
		if (engineClip != null)
		{
			m_mySource.Play();
		}
	}
	
	void OnDisable()
	{
		m_mySource.Stop();
	}
	
	void Update () 
	{
		m_volumeModifier = Mathf.Clamp(m_volumeModifier, 0.2f, 0.4f);
		m_pitchModifier = Mathf.Clamp(m_pitchModifier, 0.5f, 1.5f);
		
		
		// Is the airship in suicide mode or not?
		if (lookToTheStateManager.GetPlayerState() == EPlayerState.Suicide)
		{
			suicideMode = true;
		}
		else
		{
			suicideMode = false;
		}
		
		if (!suicideMode)
		{
			m_mySource.volume = m_volumeModifier;
			m_mySource.pitch = m_pitchModifier;
		}
		else
		if (suicideMode)
		{
			m_mySource.volume = Mathf.Lerp(m_mySource.volume, 1.0f, m_lerpSoundSpeed);
			m_mySource.pitch = Mathf.Lerp(m_mySource.pitch, 1.8f, m_lerpSoundSpeed);
		}
	}
	
	public void AudioInput(float a_throttle)
	{
		if (a_throttle > 0)
		{
			//volumeModifier += 0.1f;
			//pitchModifier += 0.1f;
			m_volumeModifier = Mathf.Lerp(m_volumeModifier, 0.4f, m_lerpSoundSpeed);
			m_pitchModifier = Mathf.Lerp(m_pitchModifier, 1.5f, m_lerpSoundSpeed);
		}
		else
		if (a_throttle < 0)
		{
			//volumeModifier += 0.1f;
			//pitchModifier -= 0.1f;
			m_volumeModifier = Mathf.Lerp(m_volumeModifier, 0.4f, m_lerpSoundSpeed);
			m_pitchModifier = Mathf.Lerp(m_pitchModifier, 0.5f, m_lerpSoundSpeed);
		}
		else
		if (a_throttle == 0)
		{
			//volumeModifier -= 0.1f;
			m_volumeModifier = Mathf.Lerp(m_volumeModifier, 0.2f, m_lerpSoundSpeed);
			m_pitchModifier = Mathf.Lerp(m_pitchModifier, 1.0f, m_lerpSoundSpeed);
		}
	}
}
