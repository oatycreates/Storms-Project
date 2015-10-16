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
        public float expireTime = 300.0f;

        /// <summary>
        /// How long to disable the collider for.
        /// </summary>
        public float colDisableTime = 1.0f;

        /// <summary>
        /// Percentage to slow the prisoner's horizontal movement per second.
        /// </summary>
        public float horizSlowRate = 0.5f;

        /// <summary>
        /// When the prisoner was spawned.
        /// </summary>
        private float m_spawnTime = 0.0f;

        /// <summary>
        /// Cached expiry time.
        /// </summary>
        private float m_startExpireTime = 0.0f;

        // Cached variables
        private Transform m_trans;
        private Rigidbody m_rigid;
        private Collider m_col;
        private FallingScream m_scream;

        void Awake()
        {
            m_trans = transform;
            m_rigid = GetComponent<Rigidbody>();
            m_col = GetComponent<Collider>();
            m_scream = GetComponent<FallingScream>();

            m_startExpireTime = expireTime;
        }

        void Start()
        {
            m_trans.tag = "Passengers";
        }

        void OnEnable()
        {
            m_spawnTime = Time.time;
            m_col.enabled = false;
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
                else
                {
                    // Enable the collider after a short wait
                    if (!m_col.enabled && Time.time - m_spawnTime >= colDisableTime)
                    {
                        m_col.enabled = true;
                    }

                    // Slow horizontal movement to make vertical dropping easier
                    Vector3 currVel = m_rigid.velocity;
                    currVel.x *= 1 - (horizSlowRate * Time.deltaTime);
                    m_rigid.velocity = currVel;
                }
            }
        }

        public void ResetExpireTimer()
        {
            expireTime = m_startExpireTime;
        }
    }

}