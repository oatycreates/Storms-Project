/**
 * File: SlipStream.cs
 * Author: Andrew Barbour
 * Maintainer: Andrew Barbour
 * Created: 2/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Behaviour for slip streams. Will apply a velocity change
 *      in the slip stream's forward vector
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
    //[RequireComponent(typeof(AudioSource))]
    public class SlipStream : MonoBehaviour
    {
        public float force = 100.0f;

        public float rumbleStr = 1.0f;
        public float rumbleDurr = 0.1f;
        private float rumbleCooldown = 0.0f;

        Rigidbody[] m_playerRigidBodies = new Rigidbody[4];

        //Audio
        private AudioSource m_AudioSource;

        void GetPlayerRigidBodies()
        {
            // Airships are the only things that will have AirshipControlBehaviours, this will only change if we refactor the input system
            AirshipControlBehaviour[] objs = GameObject.FindObjectsOfType<AirshipControlBehaviour>();
            // Use the script to find the rigidbody
            StoreBodyInSlot(objs[0].GetComponent<Rigidbody>());
            StoreBodyInSlot(objs[1].GetComponent<Rigidbody>());
            StoreBodyInSlot(objs[2].GetComponent<Rigidbody>());
            StoreBodyInSlot(objs[3].GetComponent<Rigidbody>());
        }

        private void StoreBodyInSlot(Rigidbody a_body)
        {
            // Store in the slot
            if (a_body != null)
            {
                if (a_body.CompareTag("Player1_"))
                {
                    m_playerRigidBodies[0] = a_body;
                }
                else if (a_body.CompareTag("Player2_"))
                {
                    m_playerRigidBodies[1] = a_body;
                }
                else if (a_body.CompareTag("Player3_"))
                {
                    m_playerRigidBodies[2] = a_body;
                }
                else if (a_body.CompareTag("Player4_"))
                {
                    m_playerRigidBodies[3] = a_body;
                }
            }
        }

        public void Awake()
        {
            // Get player rigidbodies
            GetPlayerRigidBodies();

            m_AudioSource = gameObject.GetComponent<AudioSource>();
        }

        public void Update()
        {
            rumbleCooldown -= Time.deltaTime;
        }

        public void OnTriggerStay(Collider a_other)
        {
            Rigidbody playerBody = null;

            if (m_playerRigidBodies.Length > 0)
            {
                bool isPlayer = false;
                switch (a_other.tag)
                {
                    case "Player1_":
                        playerBody = m_playerRigidBodies[0];
                        isPlayer = true;
                        break;
                    case "Player2_":
                        playerBody = m_playerRigidBodies[1];
                        isPlayer = true;
                        break;
                    case "Player3_":
                        playerBody = m_playerRigidBodies[2];
                        isPlayer = true;
                        break;
                    case "Player4_":
                        playerBody = m_playerRigidBodies[3];
                        isPlayer = true;
                        break;
                    case "Passengers":
                        playerBody = a_other.GetComponentInParent<Rigidbody>();
                        break;
                    default:
                        // Not player
                        playerBody = a_other.GetComponent<Rigidbody>();
                        return;
                }

                // Ensure that player has a rigidbody
                if (playerBody != null)
                {
                    Vector3 forceDir = transform.forward;
                    float forceMag = force + Mathf.Sqrt(playerBody.velocity.magnitude * 0.001f);
                    playerBody.AddForce(forceDir * forceMag, ForceMode.VelocityChange);
                }

                if (isPlayer && rumbleCooldown <= 0)
                {
                    // Rumble and screenshake the player
                    InputManager.SetControllerVibrate(a_other.tag, rumbleStr, rumbleStr, rumbleDurr, true);

                    // Only rumble as often as it would be triggered
                    rumbleCooldown = rumbleDurr;

                    //Trigger a sound
                    if (m_AudioSource != null)
                    {
	                    if (!m_AudioSource.isPlaying)
	                    { 
	                        m_AudioSource.Play(); 
	                    }
                    }
                }
            }
        }

        /*
        void OnTriggerExit(Collider a_other)
        {
            Rigidbody playerBody = null;

            if (m_playerRigidBodies.Length > 0)
            {
                //bool isPlayer = false;
                switch (a_other.tag)
                {
                    case "Player1_":
                        playerBody = m_playerRigidBodies[0];
                      
                        break;
                    case "Player2_":
                        playerBody = m_playerRigidBodies[1];
                     
                        break;
                    case "Player3_":
                        playerBody = m_playerRigidBodies[2];
                       
                        break;
                    case "Player4_":
                        playerBody = m_playerRigidBodies[3];
                       
                        break;
                    default:
                        // Not player
                        return;
                }

                if (m_AudioSource.isPlaying)
                {
                    m_AudioSource.Stop();
                }
            }

        }
        */
    }
}
