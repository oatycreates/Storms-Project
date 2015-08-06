using UnityEngine;
using System.Collections;
using System.Collections.Generic;	//For lists

public class SpawnPassengers : MonoBehaviour 	// A script to pool and spawn pirate passengers
{

	public int pooledAmount = 2000; // To avoid memory spikes
	public float spawnRateInSeconds = 1.0f;
	private float startSpawnRate;

	List<GameObject> passengers;

	//public GameObject passengerPrefab;

	//detect player presence
	private Ray myRay;
	private RaycastHit hit;
	public float rayCastLength = 50.0f;


	void Start () 
	{
		passengers = new List<GameObject> ();

		for (int i = 0; i < pooledAmount; i++)
		{
			GameObject singlePassenger = GameObject.CreatePrimitive(PrimitiveType.Cube);

			singlePassenger.AddComponent<Rigidbody>();
			singlePassenger.GetComponent<Rigidbody>().useGravity = true;

			singlePassenger.tag = "Passengers";

			//add Passenger scripts here
			singlePassenger.AddComponent<PassengerDestroyScript>();

			singlePassenger.SetActive(false);
			//add to the passengers list
			passengers.Add(singlePassenger);

		}

		startSpawnRate = spawnRateInSeconds;	//Save an initial spawnRate.

	}


	void Update () 
	{
		spawnRateInSeconds -= Time.deltaTime; // count down

		Vector3 relativeSpace = gameObject.transform.TransformDirection (Vector3.down);	// from world space to local space

		myRay = new Ray (gameObject.transform.position, relativeSpace);
		Debug.DrawRay (myRay.origin, myRay.direction * rayCastLength, Color.green);


		if (spawnRateInSeconds < 0)
		{
			SpawnPassenger();	// call function

			spawnRateInSeconds = startSpawnRate; // reset spawn rate

		}

		/*
		if (Physics.Raycast(myRay, out hit, rayCastLength))	//fire a ray
		{
			if (hit.collider.gameObject.tag == "Player1_" || hit.collider.gameObject.tag == "Player2_"  || hit.collider.gameObject.tag == "Player3_" || hit.collider.gameObject.tag == "Player4_" )
			{
				if (spawnRateInSeconds < 0)
				{
					SpawnPassenger();
					spawnRateInSeconds = startSpawnRate; //Reset spawn rate
				}
			}
		}*/
	}


	void SpawnPassenger()
	{
		for (int i = 0; i < passengers.Count; i++)	//loop through
		{
			if (!passengers[i].activeInHierarchy) //search for inactive passengers
			{
				passengers[i].transform.position = gameObject.transform.position;
				passengers[i].transform.rotation = Quaternion.identity;

				passengers[i].SetActive(true);

				//Use relative space to spawn
				Vector3 relativeSpace = gameObject.transform.TransformDirection(Vector3.forward);

				//add initial passenger velocity here!
				passengers[i].GetComponent<Rigidbody>().AddForce(relativeSpace * 10, ForceMode.Impulse);	//Jump!

				break;	//Don't forget this!
			}
		}


	}




}
