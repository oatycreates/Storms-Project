using UnityEngine;
using System.Collections;
//This script controls the Ambient Wind audio - It chooses ONE clip to loop throughout the level, but changes the pitch often. 
[RequireComponent(typeof(AudioSource))]
public class AmbientSound : MonoBehaviour 
{
	private AudioSource mySource;
	public AudioClip[] windSounds;

	void Start () 
	{
		mySource = gameObject.GetComponent<AudioSource>();
		mySource.volume = 0.3f;
		RandomClip();
	}
	
	void Update () 
	{	
		if (!mySource.isPlaying)
		{
			RandomPitch();
		}
	}
	
	void RandomClip()
	{
		//Random clip
		mySource.clip = windSounds[Random.Range(0, windSounds.Length)];
		
		RandomPitch();
	}
	
	void RandomPitch()
	{
		//Random pitch
		mySource.pitch = Random.Range(0.85f, 1.15f);
		//Play clip
		mySource.Play();
	}
}
