/**
 * File: CannonFire.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 20/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script pools the cannonball prefab objects, and fires them when triggered.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
    public enum ECannonPos
    {
        Forward,
        Port,
        Starboard
    }

    /// <summary>
    /// This script pools the cannonball prefab objects, and fires them when triggered.
    /// </summary>
    public class CannonFire : MonoBehaviour
    {
        /// <summary>
        /// Handle to the parent airship.
        /// </summary>
        public GameObject parentAirship;

        /// <summary>
        /// Where the cannon is relative to the ship, see ECannonPos.
        /// </summary>
        public ECannonPos cannon;

        /// <summary>
        /// Minimum time between shots.
        /// </summary>
        public float shotCooldown = 2.51f;

        /// <summary>
        /// How many cannonballs to pool per cannon.
        /// </summary>
        public int pooledAmount = 2;

        /// <summary>
        /// Which cannonball prefab to spawn.
        /// </summary>
        public GameObject cannonBallPrefab;

        /// <summary>
        /// How much force to launch the cannon balls with.
        /// </summary>
        public float cannonBallForce = 50.0f;

        /// <summary>
        /// Handle to the player firing reticle.
        /// </summary>
        public GameObject lookAtTarget;

        /// <summary>
        /// Time before the cannon can fire again.
        /// </summary>
        private float m_currShotCooldown = 0.0f;

        /// <summary>
        /// Object pooled cannonballs.
        /// </summary>
        private List<GameObject> m_cannonBalls;

        /// <summary>
        /// Direction the cannon is facing in, equal to m_trans.forward.
        /// </summary>
        private Vector3 m_relativeForward;

        // Cached variables
        private Rigidbody m_shipRB;
        private Transform m_trans = null;
        private Transform m_tarTrans = null;

        void Awake()
        {
            m_trans = transform;
            m_tarTrans = lookAtTarget.transform;
            m_shipRB = parentAirship.GetComponent<Rigidbody>();

            m_cannonBalls = new List<GameObject>();

            for (int i = 0; i < pooledAmount; i++)
            {
                // Pooled object details
                GameObject singleBall = Instantiate(cannonBallPrefab, m_trans.position, Quaternion.identity) as GameObject;

                // Tag the cannonball
                singleBall.tag = parentAirship.tag;

                singleBall.SetActive(false);

                // Add the singleBall to the list
                m_cannonBalls.Add(singleBall);
            }
        }

        void Start()
        {

        }

        void Update()
        {
            // Count down the shot cool-down
            m_currShotCooldown -= Time.deltaTime;

            // Look at the target
            m_trans.LookAt(m_tarTrans.position);
            m_relativeForward = m_trans.forward;

            Ray ray = new Ray(m_trans.position, m_relativeForward);
            Debug.DrawRay(ray.origin, ray.direction * 5, Color.red);
        }

        public void Fire()
        {
            Vector3 relativeSpace;
            Rigidbody rigidBall = null;
            Transform transBall = null;
            GameObject goBall = null;
            TrailRenderer trailBall = null;

            if (m_currShotCooldown <= 0)
            {
                // Put the cannon on cool-down
                m_currShotCooldown = shotCooldown;

                for (int i = 0; i < m_cannonBalls.Count; i++)
                {
                    goBall = m_cannonBalls[i];
                    // Find only inactive cannonballs
                    if (!goBall.activeInHierarchy)
                    {
                        transBall = goBall.transform;
                        transBall.position = m_trans.position;
                        transBall.rotation = Quaternion.identity;

                        goBall.SetActive(true);

                        relativeSpace = m_trans.TransformDirection(Vector3.forward);

                        rigidBall = goBall.GetComponent<Rigidbody>();

                        // Inherit the parent's velocity
                        rigidBall.velocity = m_shipRB.velocity;

                        // Toggle the trail renderer to prevent it from snapping to the new position
                        trailBall = goBall.GetComponent<TrailRenderer>();
                        trailBall.time = -1000.0f;
                        trailBall.enabled = false;

                        // Fire off the cannonball
                        rigidBall.AddRelativeForce(relativeSpace * cannonBallForce, ForceMode.Impulse);

                        // Don't forget! Every once in a while, you deserve a...
                        break;
                    }
                }
            }
        }
    } 
}
