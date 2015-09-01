/**
 * File: SpawnPassengers.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the spawning and pooling of passengers.
 **/

using UnityEngine;
using System.Collections;
// For lists
using System.Collections.Generic;

namespace ProjectStorms
{
    /// <summary>
    /// A script to pool and spawn pirate passengers.
    /// UPDATE: From now on- use the passenger prefab object instead of primitive cubes.
    /// </summary>
    public class SpawnPassengers : MonoBehaviour
    {
        public float initialPassengerForce = 10.0f;

        /// <summary>
        /// To avoid memory spikes.
        /// </summary>
        public bool currentlySpawning = false;

        public int pooledAmount = 2000;
        public float spawnRateInSeconds = 1.0f;
        private float m_startSpawnRate;

        /// <summary>
        /// How heavy to make each passenger. Mass in kg.
        /// </summary>
        private float m_passengerMass = 0.01f;

        List<GameObject> passengers;

        public GameObject passengerPrefab;

        public LineRenderer spawnHelperLaser;
        public float spawnLaserAlpha = 1.0f;

        // Detect player presence
        public float rayCastLength = 50.0f;
        private Ray m_myRay;
        private RaycastHit m_hit;

        /// <summary>
        /// For putting the prisoners under a common holder transform to keep the scene tidy.
        /// </summary>
        private static GameObject ms_prisonerHolder = null;

        // Cached variables
        private Transform m_trans = null;
        

        void Awake()
        {
            m_trans = transform; 
        }

        void Start()
        {

            // Find the prisoner holder object
            if (ms_prisonerHolder == null)
            {
                ms_prisonerHolder = GameObject.FindGameObjectWithTag("PrisonerHolder");
                if (ms_prisonerHolder == null)
                {
                    ms_prisonerHolder = new GameObject();
                    ms_prisonerHolder.name = "PrisonerHolder";
                    ms_prisonerHolder.tag = "PrisonerHolder";
                }
            }

            passengers = new List<GameObject>();

            Transform holderTrans = ms_prisonerHolder.transform;
            for (int i = 0; i < pooledAmount; i++)
            {
                //GameObject singlePassenger = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //Use the prefab from now on.
                GameObject singlePassenger = Instantiate(passengerPrefab, m_trans.position, Quaternion.identity) as GameObject;

                singlePassenger.tag = "Passengers";
                /*
                            singlePassenger.AddComponent<Rigidbody>();			
                            singlePassenger.GetComponent<Rigidbody>().useGravity = true;


                            // Add Passenger scripts here
                            singlePassenger.AddComponent<PassengerDestroyScript>();
                            //Add an audiosource before the falling scream script.
                            singlePassenger.AddComponent<AudioSource>();	
                            singlePassenger.AddComponent<FallingScream>();
                */
                // Hide under a holder prefab to keep the scene tidy
                singlePassenger.transform.parent = holderTrans;

                singlePassenger.GetComponent<Rigidbody>().useGravity = true;
                singlePassenger.SetActive(false);

                // Add to the passengers list
                passengers.Add(singlePassenger);

            }

            // Save an initial spawnRate
            m_startSpawnRate = spawnRateInSeconds;
        }

        void Update()
        {
            // Count down
            spawnRateInSeconds -= Time.deltaTime;

            // From world space to local space
            Vector3 relativeSpace = m_trans.TransformDirection(Vector3.down);

            m_myRay = new Ray(m_trans.position, relativeSpace);
            Debug.DrawRay(m_myRay.origin, m_myRay.direction * rayCastLength, Color.green);

            if (currentlySpawning)
            {
                if (spawnRateInSeconds < 0)
                {
                    SpawnPassenger();

                    spawnRateInSeconds = m_startSpawnRate; // Reset spawn rate
                }
            }

            /*
            // Fire a ray
            if (Physics.Raycast(m_myRay, out m_hit, rayCastLength))
            {
                if (m_hit.collider.gameObject.tag == "Player1_" || m_hit.collider.gameObject.tag == "Player2_"  || m_hit.collider.gameObject.tag == "Player3_" || m_hit.collider.gameObject.tag == "Player4_" )
                {
                    if (spawnRateInSeconds < 0)
                    {
                        SpawnPassenger();
                        //Reset spawn rate
                        spawnRateInSeconds = m_startSpawnRate;
                    }
                }
            }*/
        }

        void SpawnPassenger()
        {
            // Variables for loop
            Vector3 relativeSpace;
            Rigidbody passengerRb;

            // Loop through, find first non-active player
            for (int i = 0; i < passengers.Count; i++)
            {
                // Search for inactive passengers
                if (!passengers[i].activeInHierarchy)
                {
                    passengers[i].transform.position = m_trans.position;
                    passengers[i].transform.rotation = Quaternion.identity;

                    passengers[i].SetActive(true);

                    // Use relative space to spawn
                    relativeSpace = m_trans.TransformDirection(Vector3.forward);

                    // Set up player rigidbody
                    passengerRb = passengers[i].GetComponent<Rigidbody>();
                    passengerRb.mass = m_passengerMass;

                    // TODO Ignore collision with the spawner colliders and the prison fortress

                    // Reset velocity before adding to it
                    passengerRb.velocity = Vector3.zero;
                    passengerRb.angularVelocity = Vector3.zero;

                    // Add initial passenger velocity here!	Jump!
                    passengerRb.AddForce(relativeSpace * initialPassengerForce * passengerRb.mass, ForceMode.Impulse);

                    // Don't forget this!
                    break;
                }
            }
        }

        public float GetSpawnLaserAlpha()
        {
            return spawnLaserAlpha;
        }
    } 
}
