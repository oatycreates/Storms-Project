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
	private bool readyToScream = false;
	private bool playing = false;
	

	void Start () 
	{
		mySource = gameObject.GetComponent<AudioSource>();
		
		RandomSound();
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
				if (playing = false)
				{
					RandomSound();
					playing = true;
				}
			}
		}
	}
	
	void OnCollisionStay(Collision other)
	{
		//Check for player collision
		if (other.gameObject.tag == "Player1_" || other.gameObject.tag == "Player2_" || other.gameObject.tag == "Player3_" || other.gameObject.tag == "Player4_")
		{
			mySource.Stop();
			readyToScream = false;
			//Use max time to scream here
			fallTimer = timeFallingBeforeScream;
			playing = false;
		}
	}
	
	void OnCollisionExit(Collision other)
	{
		if (other.gameObject.tag == "Player1_" || other.gameObject.tag == "Player2_" || other.gameObject.tag == "Player3_" || other.gameObject.tag == "Player4_")
		{
			readyToScream = true;	
		}
	}
	
	
	void RandomSound()
	{	
		//Get a random sound from the list.
		mySource.clip = sounds[Random.Range(0, sounds.Length)];
		//Give it a random pitch.
		mySource.pitch = Random.Range(0.7f, 1.5f);
		//Play the clip
		mySource.Play ();
	}
}
