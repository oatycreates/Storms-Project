/**
 * File: ScoreManager.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 03/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Determines how the Skywhale Flys through the game world, using ridigbody physics
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{

	[RequireComponent(typeof(Rigidbody))]
	public class SkyWhaleFlight : MonoBehaviour 
	{
		
		private Rigidbody myRigid;
		
		public GameObject followObject;
		
		public float turnSpeed = 1;
		public float moveSpeed = 12;
		
		public bool speedByDistance = false;
		
		private Quaternion lookRotation;
		private Vector3 direction;
		
		private float distanceToTarget;
		
		private GameObject closest = null;
		
		//Stuff for the editor
		private Vector3 tempDirection;
		private float tempDistance;
		
		void Awake()
		{
			myRigid = gameObject.GetComponent<Rigidbody>();
		}
		
		void Start()
		{
			//Start by spawning a target;
			SpawnATarget();
			
			//Spawn a Target Every Second
			InvokeRepeating("SpawnATarget", 0, 1);
		}
		
		void FixedUpdate () 
		{
			// I think this function needs to be called regularly
			FindClosestNode();
			
			Rotating();
			Moving();
			
			Debug.DrawRay(myRigid.transform.position, direction * distanceToTarget, Color.green);
		}
		
		//Hmm this is the example from the Unity Scripting API page
		GameObject FindClosestNode()
		{
			GameObject[] gos;		
			gos = GameObject.FindGameObjectsWithTag("AINode");
			
			//GameObject closest = null;	
			float distance = Mathf.Infinity;	
			Vector3 position = myRigid.transform.position;
			
			foreach (GameObject go in gos)
			{
				Vector3 diff = go.transform.position - position;
				float curDistance = diff.sqrMagnitude;
				
				if (curDistance < distance)
				{
					closest = go;
					distance = curDistance;
				}
			}		
			return closest;			
		}
		
		
		
		void Rotating()
		{		
			//Direciton between me and the other object
			direction = (closest.transform.position - myRigid.transform.position).normalized;
			
			//Create the rotation to the target
			lookRotation = Quaternion.LookRotation(direction);
			
			//Rotate over time
			//gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
			Quaternion tempTurn = Quaternion.Slerp(myRigid.transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
			
			myRigid.MoveRotation(tempTurn);
		}
		
		
		void Moving()
		{
			//Distance to target object
			distanceToTarget = Vector3.Distance(closest.transform.position, myRigid.transform.position);
			
			float tempDistanceValue = distanceToTarget;
			//Clamp the distance value, so we can then use it in the Movement vector
			Mathf.Clamp(tempDistanceValue, 0, 1);
			
			Vector3 tempPos;
			
			if (speedByDistance)
			{
				//Move over time
				tempPos = (myRigid.transform.position + myRigid.transform.forward * Time.deltaTime * tempDistanceValue);
			}
			else
			{
				//Move with standard speed
				tempPos = (myRigid.transform.position + myRigid.transform.forward * Time.deltaTime * moveSpeed);
			}
			
			myRigid.MovePosition(tempPos);
		}
		
		
		void SpawnATarget()
		{
			GameObject tempTarget = new GameObject("WhaleNode");
			tempTarget.transform.position = followObject.transform.position;
			tempTarget.transform.rotation = followObject.transform.rotation;
			tempTarget.tag = "AINode";
			Destroy(tempTarget, 1.5f);
			
		}
		
	}
}