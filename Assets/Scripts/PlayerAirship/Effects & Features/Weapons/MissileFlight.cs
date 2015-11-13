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
using System.Collections.Generic;

namespace ProjectStorms
{
	[RequireComponent(typeof(AudioSource))]
	[RequireComponent(typeof(Rigidbody))]
	public class MissileFlight : MonoBehaviour 
	{
        [Tooltip("How much force to apply on hit.")]
        public float ramForce = 150.0f;

        [Tooltip("How much force to apply to passengers on hit.")]
        public float passengerRamForce = 15.0f;

		private Transform m_target = null;

		private Rigidbody m_myRigid = null;
        private Transform m_trans = null;

		private bool attacking = false;

		public float movementVelocity = 1;
		private float hiddenMovementVelocity;
		public float turnSpeed = 5;

		private Transform m_targetProxy;

		//This is the key value
		public float closeRangeThreshold = 20;
		private float hiddenRangeThreshold;

		//Trail renderer
		private TrailRenderer childTrail;

		public float missileLifetime = 6;
		
		//AUdio stuff
		private AudioSource m_Audio;
		public AudioClip beepSFX;
		public AudioClip launchSFX;
        private float customPitch = 0.1f;

        private static GameObject ms_powerupHolder = null;
        
        //Who fired the missile?
        private GameObject myAirship;
        //Have I been caught in the wind
       	private bool caughtInTheWind = false;

        private float m_flightDist = 5.0f;
        private Vector3 m_startPos = Vector3.zero;

		void Awake()
		{
			m_myRigid = gameObject.GetComponent<Rigidbody> ();
            m_trans = transform;

            m_startPos = m_trans.position;

            // Find the powerup holder object
            if (ms_powerupHolder == null)
            {
                ms_powerupHolder = GameObject.FindGameObjectWithTag("PowerupHolder");
                if (ms_powerupHolder == null)
                {
                    ms_powerupHolder = new GameObject();
                    ms_powerupHolder.name = "PowerupHolder";
                    ms_powerupHolder.tag = "PowerupHolder";
                }
            }

            GameObject tarProxyObj = new GameObject();
            tarProxyObj.name = "MissileTarget";
            m_targetProxy = tarProxyObj.transform;
            m_targetProxy.parent = ms_powerupHolder.transform;
            // Just leverage the cannonball holder for now

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
			customPitch = Mathf.Clamp(customPitch, 0.1f, 1.0f);
			m_Audio.pitch = customPitch;
		
		
			//Kill the missile if it's been caught in a pinwheel spiral
			
			if (caughtInTheWind)
			{
				if (m_target == null)
				{
					StopAttacking();
					Invoke("GoToSleep", 1);
				}
			}		
		}


		void FixedUpdate()
        {
            Vector3 offset = m_trans.position - m_startPos;
            if (offset.magnitude > m_flightDist)
            {
                GoToSleep();
            }

            //Prevent angular velocity
            m_myRigid.angularVelocity = Vector3.zero;

            // Don't chase an invalid target
            if (m_target == null)
            {
                attacking = false;
            }

			if (attacking) 
			{
                m_myRigid.velocity = m_trans.forward * hiddenMovementVelocity;

				//Try it with targetProxy
				Quaternion targetDirection = Quaternion.LookRotation (m_targetProxy.position - m_trans.position);
					
				m_myRigid.MoveRotation (Quaternion.RotateTowards (m_trans.rotation, targetDirection, turnSpeed));
			} 
			else
			if (!attacking) 
			{
				//Move forward in a straight line
                m_myRigid.velocity = m_trans.forward * hiddenMovementVelocity;
			}

			float distanceToProxyPoint = Vector3.Distance(m_targetProxy.position, m_trans.position);


            if (distanceToProxyPoint > hiddenRangeThreshold && m_target != null)
			{
				//only update the target pos if target is more than 10 meters away from missile
				m_targetProxy.position = m_target.position;
			}
			else
			if (distanceToProxyPoint < hiddenRangeThreshold)
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
			//Only airships have the AirshipControlBehaviour script so look for them
			m_target = GameObject.FindObjectOfType<AirshipControlBehaviour>().transform;
			//print (target.gameObject.transform.root.gameObject.name);

			//Give the missile a target
            m_targetProxy.position = m_target.position;
			
			if  (!attacking)
			{
				attacking = true;
			}
		}


		void OnEnable()
		{
			//Reset movement velocity
			hiddenMovementVelocity = movementVelocity;
			
			//Reset range threshold
			hiddenRangeThreshold = closeRangeThreshold;
			
			//Caught in the wind
			caughtInTheWind = false;
		
			//Invoke ("GoToSleep", secondsTillTimeout);
			///FindTarget ();
			/// //Don't try and find target straight away, because it'll just find the player that shot the missile.
			//Invoke ("FindTarget", 1);
			//Fire ();
			//Fix the trail time
			childTrail.time = 0.2f;
			
			m_Audio.clip = launchSFX;
			//m_Audio.pitch = 0.75f;
			//use custom pitch instead
			customPitch = 0.75f;
            m_Audio.Play();

            // Disable trail renderer on spawn, scheduled re-enable
            ResetTrail(childTrail, this);

            Invoke("GoToSleep", missileLifetime);
		}

        /// <summary>
        /// Reset the trail renderer to prevent the trail from joining
        /// </summary>
        public static void ResetTrail(TrailRenderer a_trail, MonoBehaviour a_inst)
        {
            a_inst.StartCoroutine(ResetTrailCoroutine(a_trail));
        }

        /// <summary>
        /// Coroutine to reset trail renderer trail.
        /// </summary>
        /// <param name="a_trail">Trail renderer to reset.</param>
        /// <returns>Enumerator for yield statement.</returns>
        static IEnumerator ResetTrailCoroutine(TrailRenderer a_trail)
        {
            var trailTime = a_trail.time;
            a_trail.time = 0;
            yield return new WaitForEndOfFrame();
            a_trail.time = trailTime;
        }

        void OnCollisionEnter(Collision a_other)
        {
            // Apply force to other
            Rigidbody rb = a_other.rigidbody;
            if (rb != null)
            {
               // rb.AddForce(m_trans.forward * ramForce, ForceMode.VelocityChange);
                //rb.AddExplosionForce(ramForce, m_target.position, 9, 9);
                rb.AddExplosionForce(ramForce, a_other.contacts[0].point, 9, 9);

                PassengerTray tray = rb.GetComponentInChildren<PassengerTray>();
                if (tray != null)
                {
                    Vector3 missileVel = m_myRigid.velocity;
                    tray.PowerDownTray();

                    // Launch passengers with missile velocity
                    Rigidbody rbTemp = null;
                    List<GameObject> contents = tray.trayContents;
                    foreach (GameObject passenger in contents)
                    {
                        rbTemp = passenger.GetComponent<Rigidbody>();
                        if (rbTemp != null)
                        {
                            rbTemp.AddForce(missileVel.normalized * passengerRamForce, ForceMode.VelocityChange);
                        }
                    }
                }
            }

            // Disable on hit
           //gameObject.SetActive(false);
           Invoke("GoToSleep", 0.1f);
        }

        /*
        void OnTriggerEnter(Collider a_other)
        {
            //passenger tray stuff
            if ( a_other.gameObject.GetComponent<PassengerTray>() != null)
            {
                print("Yess!");
                a_other.gameObject.GetComponent<PassengerTray>().PowerDownTray();
                a_other.gameObject.GetComponent<PassengerTray>().ExplodeTray();
            }
        }
        */

        public void SetTarget(Transform a_trans, float a_homeDist)
        {
            if (a_trans != null)
            {
                m_target = a_trans;

                attacking = true;

                //Give the missile a target
                m_targetProxy.position = m_target.position;
            }

            // Store flight expiry distance
            m_startPos = m_trans.position;
            m_flightDist = a_homeDist;
        }

		void GoToSleep()
		{
            m_target = null;

			if (!attacking)
			{
				//Fix the trail time
				childTrail.time = -1;
				gameObject.SetActive (false);
			}
		}
		
		
		public void WhoShotMe(GameObject airship)
		{
			myAirship = airship;
		}
		
		
		void StopAttacking()
		{
			attacking = false;
		}
				
		void ReturnToSender()
		{
			if (myAirship != null)
			{
				//m_targetProxy.position = myAirship.transform.position;
				m_target = myAirship.transform;
				
				CancelInvoke("GoToSleep");
				attacking = true;
				
				// give the missile some time to 'retrack';
				Invoke("StopAttacking", missileLifetime - 0.2f);
				Invoke ("GoToSleep", missileLifetime);
				
				Debug.Log("Returning to Sender " + myAirship.name);
			}
			else
			{
				Debug.Log("No Sender to Reutrn To");
			}
		}
		
		
		void EyeOfTheStorm(GameObject pinwheel)
		{
			m_target = pinwheel.transform;
			//Directly modify the hidden movement velocity
			//hiddenMovementVelocity = 30.0f;
			hiddenRangeThreshold = 0.0f;
		
			//CancelInvoke("GoToSleep");
			attacking = true;
			//It's very important to set caughtInTheWind to True;
			caughtInTheWind = true;

            // Re-store start position to fix missile life distance
            m_startPos = m_trans.position;
		
			Debug.Log("Missile is sucked into the Eye of the Storm: " + myAirship.name);
		}
		
		
		void OnTriggerEnter(Collider other)
		{
			// If a missile enters Delay Chaff, send it Return to sender.
			if (other.gameObject.GetComponent<DelayChaff>() != null)
			{
				//print ("Yeah!");
				ReturnToSender();
			}
			
			//If a missie enters a pinwheel, head towards the local offset target on the pinwheel
			if (other.gameObject.GetComponent<PinwheelBlade>() != null)
			{
				EyeOfTheStorm(other.gameObject);
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
