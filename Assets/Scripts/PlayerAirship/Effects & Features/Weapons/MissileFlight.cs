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
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(Rigidbody))]
	public class MissileFlight : MonoBehaviour 
	{
		public GameObject target;

		private Rigidbody myRigid;

		private bool attacking = false;
		private bool startWait = false;

		public float movementVelocity = 1;
		public float turnSpeed = 5;

		private GameObject targetProxy;

		//This is the key value
		public float closeRangeThreshold = 20;

		//Trail renderer
		private TrailRenderer childTrail;

		public float missileLifetime = 6;
		
		//AUdio stuff
		private AudioSource m_Audio;
		public AudioClip beepSFX;
		public AudioClip launchSFX;
		private float customPitch = 0.75f;


		void Awake()
		{
			myRigid = gameObject.GetComponent<Rigidbody> ();

			targetProxy = new GameObject();
			targetProxy.name = "MissileTarget";

			childTrail = gameObject.GetComponentInChildren<TrailRenderer> ();
			
			if (gameObject.GetComponent<AudioSource>() == null)
			{
				gameObject.AddComponent<AudioSource>();
			}
			
			m_Audio = gameObject.GetComponent<AudioSource>();
		}

		void Update () 
		{
			//Clamp pitch 
			customPitch = Mathf.Clamp(customPitch, 0.75f, 3.5f);
			m_Audio.pitch = customPitch;
		
		
			//Raycast
			Vector3 rayDirection = (targetProxy.transform.position - gameObject.transform.position).normalized;
			float rayDistance = Vector3.Distance (targetProxy.transform.position, gameObject.transform.position);

			if (attacking)
			{
				Debug.DrawRay (gameObject.transform.position, rayDirection * rayDistance, Color.red);
			}
			else
			if (!attacking)
			{
				Debug.DrawRay(gameObject.transform.position, rayDirection * rayDistance, Color.green);
			}


			//Condition to disable the object
			if (!startWait)
			{
				//GoToSleep();
				Invoke("GoToSleep", missileLifetime);
			}
			
		}


		void FixedUpdate()
		{
			if (attacking) 
			{
				myRigid.velocity = transform.forward * movementVelocity;

				//Try it with targetProxy
				Quaternion targetDirection = Quaternion.LookRotation (targetProxy.transform.position - myRigid.transform.position);
					
				myRigid.MoveRotation (Quaternion.RotateTowards (myRigid.transform.rotation, targetDirection, turnSpeed));
			} 
			else
			if (!attacking) 
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
				attacking = false;
			}
			
			//Play other audio
			if (!m_Audio.isPlaying)
			{
				m_Audio.clip = beepSFX;
				
				if (attacking)
				{
					//m_Audio.pitch = Mathf.Lerp(m_Audio.pitch, 3, 1);
					//m_Audio.pitch += 0.25f;
					//Use custom pitch instead
					customPitch += 0.25f;
					m_Audio.Play();
				}
			}
		}

		void FindTarget()
		{
			//Reset angular velocity?
			myRigid.angularVelocity = Vector3.zero;

			//Only airships have the AirshipControlBehaviour scipt so look for them
			target = GameObject.FindObjectOfType<AirshipControlBehaviour> ().gameObject;
			//print (target.gameObject.transform.root.gameObject.name);

			//Give the missile a target
			targetProxy.transform.position = target.transform.position;
			
			if  (!attacking)
			{
				attacking = true;
			}

			startWait = false;
		}


		void OnEnable()
		{
			//Invoke ("GoToSleep", secondsTillTimeout);
			///FindTarget ();
			/// //Don't try and find target straight away, because it'll just find the player that shot the missile.
			Invoke ("FindTarget", 1);
			//Fire ();
			//Fix the trail time
			childTrail.time = 1;
			startWait = true;
			
			m_Audio.clip = launchSFX;
			//m_Audio.pitch = 0.75f;
			//use custom pitch instead
			customPitch = 0.75f;
			m_Audio.Play();
		}

		void GoToSleep()
		{
			if (!attacking)
			{
				//Fix the trail time
				childTrail.time = -1;
				gameObject.SetActive (false);
			}
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

		/*
		void Fire()
		{
			//Reset angular velocity?
			//myRigid.angularVelocity = Vector3.zero;

			if  (!attacking)
			{
				attacking = true;
			}

			//Give the missile a target
			//targetProxy.transform.position = target.transform.position;
		}*/
	}
}
