using UnityEngine;
using System.Collections;
//This script controls the creaks and groans of the Wood on the Airship as it moves.
[RequireComponent(typeof(AudioSource))]
public class AirshipAudio : MonoBehaviour 
{
	private AudioSource mySource;
	public AudioClip[] woodenSounds;
	//private float timer;
	private bool movement = false;

	void Start () 
	{
		mySource = gameObject.GetComponent<AudioSource>();
		mySource.volume = 0.20f;
		RandomMe();
	}
	
	void Update () 
	{		
		// If movement is True, play a sound.
		// If that sound finishes, and movement is Still True, play a new sound.
		if (movement)
		{
			// If source is Not Playing
			if (!mySource.isPlaying)
			{
				RandomMe();
				mySource.Play();
			}
			
		}
		else
		if (!movement)
		{
			mySource.Stop();
			RandomMe();
		}
		
	}
	
	public void AudioInputs(float pitch, float yaw, float roll)//, float speed)
	{
		if ((pitch != 0) || (yaw != 0) || (roll != 0))
		{
			movement = true;
		}
		else
		{
			movement = false;
		}
	}
	
	void RandomMe()
	{
		//Random clip from array
		mySource.clip = woodenSounds[Random.Range(0, woodenSounds.Length)];
		//Give it a random pitch.
		mySource.pitch = Random.Range(0.75f, 1.5f);
	}
}
