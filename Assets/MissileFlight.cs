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
		public float spiralSpeed = 10;

		private Vector3 startPoint;

		void Awake()
		{
			myRigid = gameObject.GetComponent<Rigidbody> ();
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
				print("World Centre");
			}

			startPoint = gameObject.transform.position;
		}

		void Update () 
		{
			if (Application.isEditor) //Quick Reset
			{
				if (Input.GetKeyDown(KeyCode.Space))
				{
					//gameObject.transform.position = startPoint;
					Fire();
				}
			}
		}

		void FixedUpdate()
		{
			if (moving)
			{
				myRigid.velocity = transform.forward * movementVelocity;

				if (target != null)
				{
					Quaternion targetDirection = Quaternion.LookRotation(target.transform.position - myRigid.transform.position);

					//Quaternion rotationDirection = Quaternion.AngleAxis(spiralSpeed * Time.time, Vector3.forward);

					//targetDirection *= rotationDirection;

					myRigid.MoveRotation(Quaternion.RotateTowards(myRigid.transform.rotation, targetDirection, turnSpeed));
				}
				else
				if (target == null)
				{
					Quaternion zeroDirection = Quaternion.LookRotation(Vector3.zero - myRigid.transform.position);
					
					myRigid.MoveRotation(Quaternion.RotateTowards(myRigid.transform.rotation, zeroDirection, turnSpeed));
				}

				//Roll around in local space
				Spiral();
			}
		}

		void Fire()
		{
			if  (!moving)
			{
				//moving = true;
				moving = !moving;
			}
		}

		void Spiral()
		{
			Vector3 pos = gameObject.transform.position;
			Quaternion rot = Quaternion.AngleAxis (spiralSpeed * Time.time, Vector3.forward);
			Vector3 dir = pos - new Vector3 (gameObject.transform.localPosition.x + 1, gameObject.transform.localPosition.y, gameObject.transform.localPosition.z);
			dir = rot * dir;
			
			myRigid.MovePosition (pos + dir);
		}
	}
}
