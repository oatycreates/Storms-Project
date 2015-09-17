/**
 * File: PassengerTriggerZone.cs
 * Author: Patrick Ferguson
 * Maintainers: 
 * Created: 11/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Transfers prisoners from one zone to another when the player ship enters.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
	public class PassengerTriggerZone : MonoBehaviour 
	{
        /// <summary>
        /// Cooldown between each wave.
        /// </summary>
        public float waveCooldown = 5.0f;

        /// <summary>
        /// Number of passengers to be released per wave.
        /// </summary>
        public int waveSize = 10;

        /// <summary>
        /// Offset to spawn passengers on from the chosen root transform.
        /// </summary>
        public Vector3 spawnOffset = new Vector3(0, 10, 0);

        /// <summary>
        /// Prefab to use for the passenger.
        /// </summary>
        public GameObject passengerPrefab;

        /// <summary>
        /// How many to pool.
        /// </summary>
        public int pooledAmount = 100;

        /// <summary>
        /// How heavy to make each passenger. Mass in kg.
        /// </summary>
        private float m_passengerMass = 0.01f;

        private List<GameObject> m_passengers;

        /// <summary>
        /// Current wait between each spawn.
        /// </summary>
        private float m_currSpawnWait = 0.0f;

        /// <summary>
        /// Number of minions spawned in the current block.
        /// </summary>
        private int m_currWaveSpawned = 0;

        /// <summary>
        /// Stored ship name.
        /// </summary>
        private string m_shipName = "";

        /// <summary>
        /// Whether to spawn a wave of enemies.
        /// </summary>
        private bool m_spawnAWave = false;

        /// <summary>
        /// For putting the prisoners under a common holder transform to keep the scene tidy.
        /// </summary>
        private static GameObject ms_prisonerHolder = null;

        // Cached variables
        private Transform m_trans = null;

        void Awake()
        {
            // Cache variables
            m_trans = transform;
            m_currSpawnWait = waveCooldown;
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

            m_passengers = new List<GameObject>();

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
                m_passengers.Add(singlePassenger);

            }
		}
		
		void Update()
        {
            m_currSpawnWait -= Time.deltaTime;
            if (m_currSpawnWait < 0)
            {
                // Set to spawn a wave
                m_spawnAWave = true;
            }

            if (m_currWaveSpawned >= waveSize)
            {
                // Reset the wave
                m_spawnAWave = false;
                m_currWaveSpawned = 0;
                m_currSpawnWait = waveCooldown;
            }
		}

        void FixedUpdate()
        {

        }

        /// <summary>
        /// Called when an object enters this trigger.
        /// </summary>
        /// <param name="a_other"></param>
        void OnTriggerStay(Collider a_other)
        {
            if (m_spawnAWave && m_currWaveSpawned < waveSize && IsPlayer(a_other))
            {
                SpawnPassengerFor(a_other.transform);
            }
        }

        /// <summary>
        /// Spawns a passenger slightly above the input transform.
        /// </summary>
        /// <param name="a_trans">Root transform for the object.</param>
        private void SpawnPassengerFor(Transform a_trans)
        {
            // Variables for loop
            Vector3 relativeSpace;
            Rigidbody passengerRb;
            Transform passengerTrans;

            ++m_currWaveSpawned;

            // Loop through, find first non-active player
            for (int i = 0; i < m_passengers.Count; i++)
            {
                // Search for inactive passengers
                if (!m_passengers[i].activeInHierarchy)
                {
                    Quaternion sprayQuat = Quaternion.identity;

                    passengerTrans = m_passengers[i].transform;
                    //passengerTrans.position = m_trans.position;
                    passengerTrans.position = a_trans.position + spawnOffset;
                    passengerTrans.rotation = Quaternion.identity;

                    m_passengers[i].SetActive(true);

                    // Use relative space to spawn
                    relativeSpace = m_trans.forward;

                    // Set up player rigidbody
                    passengerRb = m_passengers[i].GetComponent<Rigidbody>();
                    passengerRb.mass = m_passengerMass;

                    // TODO Ignore collision with the spawner colliders and the prison fortress

                    // Reset velocity before adding to it
                    passengerRb.velocity = Vector3.zero;
                    passengerRb.angularVelocity = Vector3.zero;

                    // Don't forget this!
                    break;
                }
            }
        }

        /// <summary>
        /// Returns whether the input collider is a player.
        /// </summary>
        /// <param name="a_col">Collider.</param>
        /// <returns>True if player, false if not.</returns>
        private bool IsPlayer(Collider a_col)
        {
            return a_col.CompareTag("Player1_") ||
                a_col.CompareTag("Player2_") || 
                a_col.CompareTag("Player3_") || 
                a_col.CompareTag("Player4_");
        }
	}
}
