using UnityEngine;
using System.Collections;

public enum FortressStates{state_Dormant, state_Warning, state_Spawning}

//This script makes a Klaxon sound as a warning, before the Fortress jettisions Prisioners.
[RequireComponent(typeof(AudioSource))]
public class PrisonFortressKlaxonWarning : MonoBehaviour 
{
	public FortressStates fortressState;
	
	public float timeBetweenPrisonerDrops = 160.0f;
	private float timer;
	
	public int numberOfTimesKlaxonShouldSound = 3;
	private int rememberStartValue;
	
	public float timePassengerSpawnFor = 15.0f;
	private float rememberPassengerTimerValue;
	

	private AudioSource mySource;

	public Light sternLight;
	public Light hullLight;
	//Should the lights be increasing or decreasing in intensity.
	private bool lightUp = false;
	
	public AudioClip klaxonSound;
	private bool warningCanPlay = true;
	private bool spawningCanPlay = true;
	
	public SpawnPassengers sternPassengerSpawn;
	public SpawnPassengers hullPassengerSpawn;
	
	
	
	void Start () 
	{
		mySource = gameObject.GetComponent<AudioSource>();
		mySource.volume = 0.3f;	
		sternLight.intensity = 0.0f;
		hullLight.intensity = 0.0f;
		
		timer = timeBetweenPrisonerDrops;
		rememberStartValue = numberOfTimesKlaxonShouldSound;
		rememberPassengerTimerValue = timePassengerSpawnFor;
	}
	
	void Update () 
	{	
	 	ClampLights();
	 	
	 	timer -= Time.deltaTime;
	 	
	 	if (timer < 0)
	 	{
	 		if (fortressState == FortressStates.state_Dormant)
	 		{
	 			fortressState = FortressStates.state_Warning;
	 		}
	 	}
	 
	 	
	 	//State Management
	 	if (fortressState == FortressStates.state_Dormant)
	 	{
	 		lightUp = false;
	 		warningCanPlay = true;
	 		spawningCanPlay = true;
			sternPassengerSpawn.currentlySpawning = false;
			hullPassengerSpawn.currentlySpawning = false;
			timePassengerSpawnFor = rememberPassengerTimerValue;
	 	}
	 	
	 	
	 	if (fortressState == FortressStates.state_Warning)
	 	{
	 		//Make sure the klaxon doesn't go on forever
			if (numberOfTimesKlaxonShouldSound > 0)
			{
	 	
				 	if (!mySource.isPlaying)
				 	{				 	
						//This is just to make sure that Warning IS NOT invoked more than once in the update.
						if (warningCanPlay)
						{
				 			Invoke("Warning", 1.0f);
				 			lightUp = false;
				 			warningCanPlay = false;
						
							numberOfTimesKlaxonShouldSound -= 1;
				 		}
				 	}
		 	}
		 	else
		 	if (numberOfTimesKlaxonShouldSound <= 0)
		 	{	
		 		//Change state
		 		fortressState = FortressStates.state_Spawning;
		 	}
	 	}
	 	
	 	
		if (fortressState == FortressStates.state_Spawning)
		{
			warningCanPlay = true;
			
			if (spawningCanPlay)
			{
				Invoke("Spawning", 4.5f);
				spawningCanPlay = false;
			}
		
			timePassengerSpawnFor -= Time.deltaTime;
			
			if (timePassengerSpawnFor < 0)
			{
				fortressState = FortressStates.state_Dormant;
				timer = timeBetweenPrisonerDrops;
			}
		
			//Reset the number of times the klaxon should sound
			numberOfTimesKlaxonShouldSound = rememberStartValue;
		}
	}
	
	
	void ClampLights()
	{
		if (sternLight.intensity < 0.0f)
		{
			sternLight.intensity = 0.0f;
		}
		
		if (sternLight.intensity > 8.0f)
		{
			sternLight.intensity = 8.0f;
		}
		
		if (hullLight.intensity < 0.0f)
		{
			hullLight.intensity = 0.0f;
		}
		
		if (hullLight.intensity > 8.0f)
		{
			hullLight.intensity = 8.0f;
		}
		
		//Increase or Decrease lights
		if (lightUp)
		{
			sternLight.intensity += 0.05f;
			hullLight.intensity += 0.05f;
		}
		else
		if (!lightUp)
		{
			sternLight.intensity -= 0.5f;
			hullLight.intensity -= 0.5f;
		}
	}
	
	
	//Warning is called before Spawning
	void Warning()
	{
		mySource.clip = klaxonSound;
			
		sternLight.color = Color.red;
		hullLight.color = Color.red;
		
		lightUp = true;
		
		mySource.Play();
		
		warningCanPlay = true;
		sternPassengerSpawn.currentlySpawning = false;
		hullPassengerSpawn.currentlySpawning = false;
	}
	
	
	//Spawning is called repeatedly for a bit, before returning to Warning
	void Spawning()
	{	
		sternLight.color = Color.green;
		hullLight.color = Color.green;
		lightUp = true;
		
		sternPassengerSpawn.currentlySpawning = true;
		hullPassengerSpawn.currentlySpawning = true;
	}
}
