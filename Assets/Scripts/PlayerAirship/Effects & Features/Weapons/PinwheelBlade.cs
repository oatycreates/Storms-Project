﻿/**
 * File: Pinwheel Blade.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 01/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Triggers a physical response if any rigidbody enters its collider.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	/// <summary>
	/// Pinwheel blade trigger. Triggers a physical response if any rigidbody enters its collider (after a brief interval)
	/// </summary>
	public class PinwheelBlade : MonoBehaviour 
	{
		public float pinwheelForce = 100;
		private bool delayFinished = false;

        //Custom timer for the pinwheel - this has to be here, because this way we can caluclate how the passengers are ejected from the vortex

        private float internalTimer = 1;
        public float secondsBeforeTimeout = 10.0f;

		private bool velocityCancel = false;
		
        void Awake()
        {
            //timer
            internalTimer = secondsBeforeTimeout;
        }
        void OnEnable()
        {
            //Make sure the colliders don't have an impact for the first few updates after pinwheel is spawned
            delayFinished = false;
            Invoke("DelayOver", 0.5f);


            //timer
            internalTimer = secondsBeforeTimeout;
            
            //allow passenger velocity to be manipulated
            velocityCancel = false;
        }

        void Update()
        {
            //timer stuff
            internalTimer -= Time.deltaTime;
            
            //turn off velocity just before we deactivate the pinwheel
            if (internalTimer < 0.1f)
            {
            	velocityCancel = true;
            }
			// Now deactivate the gameobject
            if (internalTimer <= 0)
            {
                //This is the pinwheel child, kill parent object instead
                gameObject.transform.parent.gameObject.SetActive(false);

            }
        }

		void OnDisable()
		{
			delayFinished = false;
			//print ("reset_____________");
		}

		void DelayOver()
		{
			delayFinished = true;
			//print ("READY");
		}

         
        void OnTriggerStay(Collider other)
        {
            Rigidbody passengerRigidbody = other.gameObject.GetComponent<Rigidbody>();

			//Local offset - just makes the passengers gather a little bit higher than the actual center of the gameObject.
			Vector3 localOffset = new Vector3 (0, 15, 0);
			Vector3 worldOffest = gameObject.transform.rotation * localOffset;
			Vector3 lookPos = gameObject.transform.position + worldOffest; 
		
            if (passengerRigidbody != null)
            {
            	//DOn't forget this bit!
            	if (delayFinished)
            	{
	                if (passengerRigidbody.gameObject.tag == "Passengers")
	                {
	                	//Do different things depending on the remaining lifetime of the pinwheel
	                	
	                	if (!velocityCancel) //suck passengers in
	                	{
		                    passengerRigidbody.AddForce(Vector3.up * 0.7f, ForceMode.Force);
		
		                    passengerRigidbody.transform.LookAt(lookPos);
		
		                    passengerRigidbody.AddRelativeForce(Vector3.forward * 10, ForceMode.Force);
	                    }
	                    else
	                    if (velocityCancel) // cancel out velocity
	                    {
	                    	passengerRigidbody.velocity = Vector3.zero;
	                    	passengerRigidbody.angularVelocity = Vector3.zero;
	                    }
	                }
                }
            }


            //passenger tray stuff
            if (other.gameObject.GetComponent<PassengerTray>() != null)
            {
                other.gameObject.GetComponent<PassengerTray>().PowerDownTray();
            }
            
            /*
            //if there is both a passenger tray, and a passenger in it,
            // make the passenger in it "jump" in the tray's local space
            if (other.gameObject.GetComponent<PassengerTray>() != null && passengerRigidbody != null)
            {
            	if (passengerRigidbody.gameObject.tag == "Passengers")
            	{
					//Vector3 direction = passengerRigidbody.transform.position - gameObject.transform.position;
					Vector3 trayUpDirection = other.gameObject.transform.up;
					passengerRigidbody.AddForce(trayUpDirection * 1.5f, ForceMode.Force);	
					Debug.Log("HERE!");
            	}
            }*/
            
        }

        /*
        void OnTriggerExit(Collider other)
        {
            Rigidbody passengerRigidbody = other.gameObject.GetComponent<Rigidbody>();

            if (passengerRigidbody != null)
            {

                if (passengerRigidbody.gameObject.tag == "Passengers")
                {
                    passengerRigidbody.velocity = new Vector3(0, 0, 0);
                    passengerRigidbody.angularVelocity = new Vector3(0, 0, 0);
                }
            }
        }
         */

	}
}
