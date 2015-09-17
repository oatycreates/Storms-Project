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
    public class SlipStream : MonoBehaviour
    {
        public float force = 100.0f;

        List<Rigidbody> m_playerRigidBodies = new List<Rigidbody>(4);

        void GetPlayerRigidBodies()
        {
            Rigidbody body = null;

            // Airships are the only things that will have AirshipControlBehaviours, this will only change if we refactor the input system
            AirshipControlBehaviour[] objs = GameObject.FindObjectsOfType<AirshipControlBehaviour>();
            foreach (AirshipControlBehaviour script in objs)
            {
                // Use the script to find the rigidbody.
                body = script.GetComponent<Rigidbody>();
                
                if (body != null)
                {
                    if (body.CompareTag("Player1_"))
                    {
                        m_playerRigidBodies[0] = body;
                    }
                    else if (body.CompareTag("Player2_"))
                    {
                        m_playerRigidBodies[1] = body;
                    }
                    else if (body.CompareTag("Player3_"))
                    {
                        m_playerRigidBodies[2] = body;
                    }
                    else if (body.CompareTag("Player4_"))
                    {
                        m_playerRigidBodies[3] = body;
                    }
                }
            }
        }

        public void Awake()
        {
            // Get player rigidbodies
            GetPlayerRigidBodies();
        }

        public void OnTriggerStay(Collider a_other)
        {
            Rigidbody playerBody = null;

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
                playerBody.AddForce(transform.forward, ForceMode.VelocityChange);
            }
        }
    }
}
