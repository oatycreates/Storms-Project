using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ECannonPos
{
	Forward,
	Port,
	Starboard
}

//This script pools the cannonball prefab objects, and fires them when triggerd.
public class CannonFire : MonoBehaviour 
{
	public GameObject ParentAirship;

	public ECannonPos cannon;
	
	public int pooledAmount = 3;
	
	List<GameObject> cannonBalls;
	
	public GameObject cannonBallPrefab;
	
	public float cannonBallForce = 50.0f;


	void Start () 
	{
		cannonBalls = new List<GameObject>();
		
		for (int i = 0; i < pooledAmount; i++)
		{
			//Pooled object details
			GameObject singleBall = Instantiate(cannonBallPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
			
			singleBall.tag = ParentAirship.tag;
			singleBall.GetComponent<SphereCollider>().isTrigger = true;
			
			singleBall.SetActive(false);
			
			// Add the singleBall to the list
			cannonBalls.Add(singleBall);
		}
	}
	
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Fire();
		}
	}
	
	public void Fire()
	{
		Vector3 relativeSpace;
		Rigidbody rigidBall;
		SphereCollider collider;
	
		for (int i = 0; i < cannonBalls.Count; i++)
		{
			// Find only inactive cannonballs
			if (!cannonBalls[i].activeInHierarchy)
			{
				cannonBalls[i].transform.position = gameObject.transform.position;
				cannonBalls[i].transform.rotation = Quaternion.identity;
				
				cannonBalls[i].SetActive(true);
				
				relativeSpace = gameObject.transform.TransformDirection(Vector3.forward);
				
				rigidBall = cannonBalls[i].GetComponent<Rigidbody>();
				
				rigidBall.AddRelativeForce(relativeSpace * cannonBallForce, ForceMode.Impulse);
				
				collider = cannonBalls[i].GetComponent<SphereCollider>();
				collider.isTrigger = true;
				
				//Don't forget! Every once in a while, you deserve a...
				break;
			}
		}
	}
	
}
