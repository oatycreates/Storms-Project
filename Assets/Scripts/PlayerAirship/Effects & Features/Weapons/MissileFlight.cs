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
        [Tooltip("How much force to apply on hit.")]
        public float ramForce = 150.0f;

		private Transform m_target = null;

		private Rigidbody m_myRigid = null;
        private Transform m_trans = null;

		private bool attacking = false;

		public float movementVelocity = 1;
		public float turnSpeed = 5;

		private Transform m_targetProxy;

		//This is the key value
		public float closeRangeThreshold = 20;

		//Trail renderer
		private TrailRenderer childTrail;

		public float missileLifetime = 6;
		
		//AUdio stuff
		private AudioSource m_Audio;
		public AudioClip beepSFX;
		public AudioClip launchSFX;
        private float customPitch = 0.1f;

        private static GameObject ms_powerupHolder = null;

		void Awake()
		{
			m_myRigid = gameObject.GetComponent<Rigidbody> ();
            m_trans = transform;

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
		
		
			/*//Raycast
			Vector3 rayDirection = (m_targetProxy.position - m_trans.position).normalized;
            float rayDistance = Vector3.Distance(m_targetProxy.position, m_trans.position);

			if (attacking)
			{
                Debug.DrawRay(m_trans.position, rayDirection * rayDistance, Color.red);
			}
			else
			if (!attacking)
			{
                Debug.DrawRay(m_trans.position, rayDirection * rayDistance, Color.green);
			}*/
			
		}


		void FixedUpdate()
        {
            //Prevent angular velocity
            m_myRigid.angularVelocity = Vector3.zero;

            // Don't chase an invalid target
            if (m_target == null)
            {
                attacking = false;
            }

			if (attacking) 
			{
                m_myRigid.velocity = m_trans.forward * movementVelocity;

				//Try it with targetProxy
				Quaternion targetDirection = Quaternion.LookRotation (m_targetProxy.position - m_trans.position);
					
				m_myRigid.MoveRotation (Quaternion.RotateTowards (m_trans.rotation, targetDirection, turnSpeed));
			} 
			else
			if (!attacking) 
			{
				//Move forward in a straight line
                m_myRigid.velocity = m_trans.forward * movementVelocity;
			}

			float distanceToProxyPoint = Vector3.Distance(m_targetProxy.position, m_trans.position);


            if (distanceToProxyPoint > closeRangeThreshold && m_target != null)
			{
				//only update the target pos if target is more than 10 meters away from missile
				m_targetProxy.position = m_target.position;
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
			//Invoke ("GoToSleep", secondsTillTimeout);
			///FindTarget ();
			/// //Don't try and find target straight away, because it'll just find the player that shot the missile.
			//Invoke ("FindTarget", 1);
			//Fire ();
			//Fix the trail time
			childTrail.time = 1;
			
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

        public void SetTarget(Transform a_trans)
        {
            if (a_trans != null)
            {
                m_target = a_trans;

                attacking = true;

                //Give the missile a target
                m_targetProxy.position = m_target.position;
            }
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
