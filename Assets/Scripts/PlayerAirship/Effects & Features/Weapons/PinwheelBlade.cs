/**
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
        }

        void Update()
        {
            //timer stuff
            internalTimer -= Time.deltaTime;

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


        //For Airship Effects
        /*
		void OnTriggerEnter(Collider other)
		{
			Rigidbody anotherRigidbody = other.gameObject.transform.root.gameObject.GetComponent<Rigidbody> ();

			//Has the delay finished?
			if (delayFinished)
			{
				if (anotherRigidbody != null)
                {
                    //Check rigidbody
                   print("Airship wind");

                    anotherRigidbody.AddRelativeTorque(new Vector3(0, -pinwheelForce, 0), ForceMode.Acceleration);
                    anotherRigidbody.AddForce(Vector3.up * pinwheelForce * 2, ForceMode.Acceleration);
                }
			}
		}
        */

        /*
        void OnTriggerEnter(Collider other)
        {
            Rigidbody passengerRigidbody = other.gameObject.GetComponent<Rigidbody>();

            if (passengerRigidbody != null)
            {

                if (passengerRigidbody.gameObject.tag == "Passengers")
                {
                    if (delayFinished)
                    {
                        // Make passengers jump in the air
                        //passengerRigidbody.velocity = new Vector3(0, 0, 0);

                        passengerRigidbody.AddForce(Vector3.up * 2.0f, ForceMode.Force);
                    }
                }

            }

            
            //Do passenger tray stuff
            if (other.gameObject.GetComponent<PassengerTray>() != null)
            {
                other.gameObject.GetComponent<PassengerTray>().PowerDownTray();
            }

        }
         */
         
        void OnTriggerStay(Collider other)
        {
            Rigidbody passengerRigidbody = other.gameObject.GetComponent<Rigidbody>();

            if (passengerRigidbody != null)
            {

                if (passengerRigidbody.gameObject.tag == "Passengers")
                {
                   // print("Passengers only wind");

                    Vector3 direction = passengerRigidbody.transform.position - gameObject.transform.position;

                    //Local offset
                   // Vector3 localOffset = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 10, gameObject.transform.position.z);

                    //Debug.DrawRay(localOffset, Vector3.down * 10, Color.green);
                   
                   // passengerRigidbody.transform.LookAt(worldOffest);
                    passengerRigidbody.AddForce(Vector3.up * 0.5f, ForceMode.Force);

                    passengerRigidbody.transform.LookAt(gameObject.transform.position);

                    passengerRigidbody.AddRelativeForce(Vector3.forward * 10, ForceMode.Force);
                }
            }


            //passenger tray stuff
            if (other.gameObject.GetComponent<PassengerTray>() != null)
            {
                other.gameObject.GetComponent<PassengerTray>().PowerDownTray();
            }
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
