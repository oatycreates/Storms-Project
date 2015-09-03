/**
 * File: PassengerDestroyScript.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script deactivates the pirate passengers as soon as they go below a certain height.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This script deactivates the pirate passengers as soon as they go below a certain height.
    /// </summary>
    public class PassengerDestroyScript : MonoBehaviour
    {
        /// <summary>
        /// Kill-Y.
        /// </summary>
        public float heightTillDeath = -2000.0f;

        /// <summary>
        /// Time till each passenger expires.
        /// </summary>
        public float expireTime = 60.0f;

        /// <summary>
        /// When the prisoner was spawned.
        /// </summary>
        private float m_spawnTime = 0.0f;

        // Cached variables
        private Transform m_trans;
        private FallingScream m_scream;

        void Awake()
        {
            m_trans = transform;
            m_scream = GetComponent<FallingScream>();
        }

        void Start()
        {
            m_trans.tag = "Passengers";
        }

        void OnEnable()
        {
            m_spawnTime = Time.time;
        }

        void Update()
        {
            if (gameObject.activeInHierarchy)
            {
                // If below kill Y or has expired
                if (m_trans.position.y < heightTillDeath || Time.time - m_spawnTime >= expireTime)
                {
                    // Check timeout
                    gameObject.SetActive(false);

                    // Reset scream
                    if (m_scream != null)
                    {
                        m_scream.readyToScream = true;
                    }
                }
            }
        }
    }

}