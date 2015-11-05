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
        /// Which cannonball prefab to spawn.
        /// </summary>
        public GameObject cannonBallPrefab;

        /// <summary>
        /// How much force to launch the cannon balls with.
        /// </summary>
        public float cannonBallForce = 50.0f;

        /// <summary>
        /// Minimum time between shots.
        /// </summary>
        public float shotCooldown = 2.51f;

        /// <summary>
        /// Handle to the player firing reticle.
        /// </summary>
        public Transform lookAtTarget = null;

        /// <summary>
        /// How many cannonballs to pool per cannon. Equal to shotLifetime/fireCooldown.
        /// </summary>
        private int m_pooledAmount = 2;

        /// <summary>
        /// Object pooled cannonballs.
        /// </summary>
        private List<GameObject> m_cannonBalls;

        /// <summary>
        /// Direction the cannon is facing in, equal to m_trans.forward.
        /// </summary>
        private Vector3 m_relativeForward;

        /// <summary>
        /// Time before the cannon can fire again.
        /// </summary>
        private float m_currShotCooldown = 0.0f;

        /// <summary>
        /// Cannonball holder object.
        /// </summary>
        static private GameObject ms_ballHolder = null;

        // Cached variables
        private Transform m_trans = null;

        void Awake()
        {
            m_trans = transform;

            // Find the cannonball holder object
            if (ms_ballHolder == null)
            {
                ms_ballHolder = GameObject.FindGameObjectWithTag("BallHolder");
                if (ms_ballHolder == null)
                {
                    ms_ballHolder = new GameObject();
                    ms_ballHolder.name = "BallHolder";
                    ms_ballHolder.tag = "BallHolder";
                }
            }

            m_cannonBalls = new List<GameObject>();

            // Create the first cannonball so that we may read its lifetime
            Transform holderTrans = ms_ballHolder.transform;
            GameObject firstBall = CreateCannonball(holderTrans);
            
            float ballLife = 0;
            CannonBallBehaviour ballScript = firstBall.GetComponent<CannonBallBehaviour>();
            if (ballScript != null)
            {
                ballLife = ballScript.cannonBallLifetime;
            }
            CannonBallRaytracer rayBallScript = firstBall.GetComponent<CannonBallRaytracer>();
            if (rayBallScript)
            {
                ballLife = rayBallScript.totalLifeTime;
            }
            m_pooledAmount = Mathf.CeilToInt(ballLife / shotCooldown);

            // Start with the shot on cooldown
            m_currShotCooldown = shotCooldown;

            // Spawn the other cannonballs
            for (int i = 1; i < m_pooledAmount; i++)
            {
                CreateCannonball(holderTrans);
            }
        }

        
        private GameObject CreateCannonball(Transform a_holderTrans)
        {
            // Pooled object details
            GameObject singleBall = Instantiate(cannonBallPrefab, m_trans.position, Quaternion.identity) as GameObject;

            // Tag the cannonball
            singleBall.tag = parentAirship.tag;

            // Store it under the holder object
            singleBall.transform.parent = a_holderTrans;

            singleBall.SetActive(false);

            // Add the singleBall to the list
            m_cannonBalls.Add(singleBall);

            return singleBall;
        }

        void Start()
        {

        }
        

        void Update()
        {
            // Count down the shot cool-down
            m_currShotCooldown -= Time.deltaTime;

            // Look at the target
            if (lookAtTarget != null)
            {
                m_trans.LookAt(lookAtTarget.position);
            }

            m_relativeForward = m_trans.forward;

            Ray ray = new Ray(m_trans.position, m_relativeForward);
            Debug.DrawRay(ray.origin, ray.direction * 5, Color.red);
        }

        public void Fire()
        {
            Rigidbody rigidBall = null;
            Transform transBall = null;
            GameObject goBall = null;
            TrailRenderer trailBall = null;

            if (m_currShotCooldown <= 0)
            {
                // Put the cannon on cool-down
                m_currShotCooldown = shotCooldown;

                if (this.isActiveAndEnabled)
                {
                    for (int i = 0; i < m_cannonBalls.Count; i++)
                    {
                        goBall = m_cannonBalls[i];
                        // Find only inactive cannonballs
                        if (!goBall.activeInHierarchy)
                        {
                            transBall = goBall.transform;
                            transBall.position = m_trans.position;
                            transBall.rotation = m_trans.rotation;

                            Debug.Log("Fired cannon along " + transBall.rotation.eulerAngles);

                            transBall.tag = this.tag;

                            goBall.SetActive(true);

                            rigidBall = goBall.GetComponent<Rigidbody>();

                            if (rigidBall != null)
                            {
                                // Inherit the parent's velocity
                                //rigidBall.velocity = m_shipRB.velocity;

                                // Toggle the trail renderer to prevent it from snapping to the new position
                                trailBall = goBall.GetComponent<TrailRenderer>();
                                if (trailBall != null)
                                {
                                    trailBall.time = -1000.0f;
                                    trailBall.enabled = false;
                                }

                                // Fire off the cannonball
                                rigidBall.AddForce(transBall.forward * cannonBallForce, ForceMode.Impulse);
                            }

                            // Don't forget! Every once in a while, you deserve a...
                            break;
                        }
                    }
                }
            }
        }
    } 
}
