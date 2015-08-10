using UnityEngine;
using System.Collections;
//This script takes in any number of audio clips, and plays one randomly while a passenger falls.
[RequireComponent (typeof(AudioSource))]
public class FallingScream : MonoBehaviour 
{
	private AudioSource mySource;
	public AudioClip[] sounds;
	public float timeFallingBeforeScream = 0.80f;
	private float fallTimer = 0.80f;
	[HideInInspector]
	public bool readyToScream = false;
	//private bool playing = false;
	

	void Start () 
	{
		mySource = gameObject.GetComponent<AudioSource>();
		
		//SERIOUS!!! THIS CAN'T BE TOO LOUD, OTHERWISE IT'LL LEAVE PEOPLE DEAF
		mySource.volume = 0.05f;
		//RandomSound();
	}
	
	void Update () 
	{
		if (readyToScream)
		{
			fallTimer -= Time.deltaTime;
		}
		
		// Trigger the scream, once the object has begun to fall.
		if (fallTimer < 0.0f)
		{
			//Check if audiosource is already playing
			if (!mySource.isPlaying)
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
	
	void OnCollisionStay(Collision other)
	{
		if (other.gameObject.tag != "Passengers")
		{
			mySource.Stop();
			readyToScream = false;
			//Use max time to scream here
			fallTimer = timeFallingBeforeScream;
			//playing = false;
		}
	}
	
	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag != "Passengers")
		{
			readyToScream = true;	
		}
	}
	
	
	public void RandomSound()
	{	
		//Get a random sound from the list.
		mySource.clip = sounds[Random.Range(0, sounds.Length)];
		//Give it a random pitch.
		mySource.pitch = Random.Range(0.7f, 1.5f);
		//Play the clip
		mySource.Play ();
	}
	
}
