/**
 * File: MissileFlight.cs
 * Author: Rowan Donaldson
 * Maintainer: Pat Ferguson
 * Created: 28/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Controls the homing missile behaviour.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	[RequireComponent(typeof(Rigidbody))]
	public class MissileFlight : MonoBehaviour 
	{
		public GameObject target;

		private Rigidbody myRigid;

		private bool moving = false;

		//public float jumpForce = 30;
		public float movementVelocity = 1;
		public float turnSpeed = 5;
		//public float spiralSpeed = 10;

		private GameObject targetProxy;
		//public float retargetSpeed = 2;

		public float closeRangeThreshold = 20;
		private Color status = Color.white;
		public GameObject childRenderer;

		void Awake()
		{
			myRigid = gameObject.GetComponent<Rigidbody> ();

			targetProxy = new GameObject();
			targetProxy.name = "MissileTarget";
		}

		void Start () 
		{
			if (target != null)
			{
				//Fire();
				print("Target");
			}
			else
			if (target == null)
			{
				target = GameObject.FindGameObjectWithTag("Player1_");
				//Fire();
				print("Found Target");
			}
		}

		void Update () 
		{
			//if (Application.isEditor) //Quick Reset
			//{
				if (Input.GetKeyDown(KeyCode.Space))
				{
					Fire();
				}
			//}


			//Raycast
			Vector3 rayDirection = (targetProxy.transform.position - gameObject.transform.position).normalized;
			float rayDistance = Vector3.Distance (targetProxy.transform.position, gameObject.transform.position);

			if (moving)
			{
				Debug.DrawRay (gameObject.transform.position, rayDirection * rayDistance, Color.red);
			}
			else
			if (!moving)
			{
				Debug.DrawRay(gameObject.transform.position, rayDirection * rayDistance, Color.green);
			}

			//Set colours
			if (childRenderer != null)
			{
				childRenderer.GetComponent<MeshRenderer>().material.color = status;
			}
			else
			{
				print("No child object attached.");
			}

		}

		void FixedUpdate()
		{
			if (moving) {
				myRigid.velocity = transform.forward * movementVelocity;

				/*
				if (target != null)
				{
					Quaternion targetDirection = Quaternion.LookRotation(target.transform.position - myRigid.transform.position);

					myRigid.MoveRotation(Quaternion.RotateTowards(myRigid.transform.rotation, targetDirection, turnSpeed));
				}
				else
				if (target == null)
				{
					Quaternion zeroDirection = Quaternion.LookRotation(Vector3.zero - myRigid.transform.position);
					
					myRigid.MoveRotation(Quaternion.RotateTowards(myRigid.transform.rotation, zeroDirection, turnSpeed));
				}*/

				//Try it with targetProxy
				Quaternion targetDirection = Quaternion.LookRotation (targetProxy.transform.position - myRigid.transform.position);
					
				myRigid.MoveRotation (Quaternion.RotateTowards (myRigid.transform.rotation, targetDirection, turnSpeed));


			} 
			else
			if (!moving) 
			{
				//Move forward in a straight line
				myRigid.velocity = transform.forward * movementVelocity;
			}

			float distanceToProxyPoint = Vector3.Distance(targetProxy.transform.position, myRigid.transform.position);

			if (distanceToProxyPoint > closeRangeThreshold)
			{
				//only update the target pos if target is more than 10 meters away from missile
				targetProxy.transform.position = target.transform.position;
			}
			else
			if (distanceToProxyPoint < closeRangeThreshold)
			{
				//Turn off Movement
				moving = false;
				print("Free");
			}

			//Set the colour
			if (distanceToProxyPoint > closeRangeThreshold + 10)
			{
				//Colour
				status = Color.white;
			}
			else
			if (distanceToProxyPoint < closeRangeThreshold + 10)
			{
				//Colour
				status = Color.red;
			}
		
			//targetProxy.transform.position = Vector3.Lerp(targetProxy.transform.position, target.transform.position, retargetSpeed* Time.deltaTime);

			
			//Roll around in local space
			//Spiral();
		}

		void Fire()
		{
			//Reset angular velocity?
			myRigid.angularVelocity = Vector3.zero;

			print ("Fire");

			if  (!moving)
			{
				moving = true;
			}

			//Give the missile a target
			targetProxy.transform.position = target.transform.position;
		}


		/*
		void Spiral()
		{

			Vector3 pos = gameObject.transform.position;

			Quaternion rot = Quaternion.AngleAxis (spiralSpeed * Time.time, Vector3.forward);
			Vector3 dir = pos - new Vector3 (gameObject.transform.position.x+1, gameObject.transform.position.y, gameObject.transform.position.z);
			dir = rot * dir;

			myRigid.MovePosition (pos + dir);
		}*/

	}
}
