using UnityEngine;
using System.Collections;
//This script controls the engine volume of the Airship.
[RequireComponent(typeof(AudioSource))]
public class EngineAudio : MonoBehaviour 
{
	private AudioSource mySource;
	public AudioClip engineClip;
	
	private float volumeModifier;
	private float pitchModifier;
	
	[HideInInspector]
	public bool suicideMode = false;
	private float lerpSoundSpeed = 0.1f;
	
	public StateManager lookToTheStateManager;

	void Awake () 
	{
		mySource = gameObject.GetComponent<AudioSource>();
		mySource.clip = engineClip;
		
		
		mySource.loop = true;
	}
	
	void Start()
	{
		mySource.volume = 0.5f;
		mySource.pitch = 1.0f;
	}
	
	void OnEnable()
	{
		if (engineClip != null)
		{
			mySource.Play();
		}
	}
	
	void OnDisable()
	{
		mySource.Stop();
	}
	
	void Update () 
	{
		volumeModifier = Mathf.Clamp(volumeModifier, 0.2f, 0.4f);
		pitchModifier = Mathf.Clamp(pitchModifier, 0.5f, 1.5f);
		
		
		//Is the airship in suicide mode or not?
		if (lookToTheStateManager.currentPlayerState == EPlayerState.Suicide)
		{
			suicideMode = true;
		}
		else
		{
			suicideMode = false;
		}
		
		if (!suicideMode)
		{
			mySource.volume = volumeModifier;
			mySource.pitch = pitchModifier;
		}
		else
		if (suicideMode)
		{
			mySource.volume = Mathf.Lerp(mySource.volume, 1.0f, lerpSoundSpeed);
			mySource.pitch = Mathf.Lerp(mySource.pitch, 1.8f, lerpSoundSpeed);
		}
	}
	
	public void AudioInput(float throttle)
	{
		if (throttle > 0)
		{
			//volumeModifier += 0.1f;
			//pitchModifier += 0.1f;
			volumeModifier = Mathf.Lerp(volumeModifier, 0.4f, lerpSoundSpeed);
			pitchModifier = Mathf.Lerp(pitchModifier, 1.5f, lerpSoundSpeed);
		}
		else
		if (throttle < 0)
		{
			//volumeModifier += 0.1f;
			//pitchModifier -= 0.1f;
			volumeModifier = Mathf.Lerp(volumeModifier, 0.4f, lerpSoundSpeed);
			pitchModifier = Mathf.Lerp(pitchModifier, 0.5f, lerpSoundSpeed);
		}
		else
		if (throttle == 0)
		{
			//volumeModifier -= 0.1f;
			volumeModifier = Mathf.Lerp(volumeModifier, 0.2f, lerpSoundSpeed);
			pitchModifier = Mathf.Lerp(pitchModifier, 1.0f, lerpSoundSpeed);
		}
	}
}
