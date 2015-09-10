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

	public enum E_WhaleMode 
	{
		Attack,
		Dormant
	}

	[RequireComponent(typeof(Rigidbody))]
	public class SkyWhaleFlight : MonoBehaviour 
	{
		
		private Rigidbody myRigid;
		public E_WhaleMode whaleMode;
		
		public Transform followObject;
		
		public float turnSpeed = 1;
		public float moveSpeed = 12;
		
		public bool speedByDistance = false;
		
		private Quaternion lookRotation;
		private Vector3 direction;
		
		private float distanceToTarget;
		
		private GameObject closest = null;
		private Vector3 spiral;
		
		//Stuff for the editor
		private Vector3 tempDirection;
		private float tempDistance;

		//Random timer
		private float random;

		private Renderer rend;
		
		void Awake()
		{
			myRigid = gameObject.GetComponent<Rigidbody>();
			rend = GetComponentInChildren<Renderer>();
		}
		
		void Start()
		{
			//Start by spawning a target;
			/*SpawnATarget();*/
			//Or Start by Spiraling
			random = Random.Range (1, 9);
			//start all whales going downwards
			spiral = new Vector3 (myRigid.transform.position.x, myRigid.transform.position.y - random*10, myRigid.transform.position.z);

			//Spawn a Target Every Second
			/*InvokeRepeating("SpawnATarget", 0, 1);*/
		}
		
		void FixedUpdate () 
		{
			//Choose a new spiral target every few seconds
			random -= Time.deltaTime;

			if (random < 0)
			{
				Spiral();
				random = Random.Range(0, 8);
			}

			if (whaleMode == E_WhaleMode.Attack)
			{
				if (followObject != null)
				{
					speedByDistance = true;

					Rotating(followObject.position);
					Moving(followObject.position);
				}
				else
				{
					speedByDistance = false;
				}
			}
			else
			if (whaleMode == E_WhaleMode.Dormant)
			{
				speedByDistance = false;

				Rotating (spiral);
				Moving (spiral);
			}

			
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


		void Rotating(Vector3 rotateTarget)
		{		
			//Direction between me and the other object
			direction = (rotateTarget - myRigid.transform.position).normalized;
			
			//Create the rotation to the target
			lookRotation = Quaternion.LookRotation(direction);
			
			//Rotate over time
			//gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
			Quaternion tempTurn = Quaternion.Slerp(myRigid.transform.rotation, lookRotation, turnSpeed * Time.deltaTime);
			
			myRigid.MoveRotation(tempTurn);
		}
		
		
		void Moving(Vector3 moveTarget)
		{
			//Distance to target object
			distanceToTarget = Vector3.Distance(moveTarget, myRigid.transform.position);
			
			float tempDistanceValue = distanceToTarget;
			//Clamp the distance value, so we can then use it in the Movement vector
			tempDistanceValue = Mathf.Clamp(tempDistanceValue, 0, 100);
			
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
		
		
		void Spiral()
		{
			spiral = (myRigid.transform.position + Random.insideUnitSphere*150);

			Debug.Log (spiral);
		}
		
		
		void SpawnATarget()
		{
			GameObject tempTarget = new GameObject("WhaleNode");
			tempTarget.transform.position = followObject.transform.position;
			tempTarget.transform.rotation = followObject.transform.rotation;
			tempTarget.tag = "AINode";
			Destroy(tempTarget, 1.5f);
			
		}

		void OnCollisionEnter(Collision other)
		{
			if (followObject == null)
			{
				//IF a player bumps into me - chase them!
				if (other.gameObject.tag == "Player1_" || other.gameObject.tag == "Player2_" || other.gameObject.tag == "Player3_" || other.gameObject.tag == "Player4_")
				{
					followObject = other.transform;
					whaleMode = E_WhaleMode.Attack;

					rend.material.color = Color.red;

					// I think this function needs to be called regularly
					//closest = FindClosestNode();
				}
			}

		}
		
	}
}