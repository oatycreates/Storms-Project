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

        Rigidbody GetPlayerRigidBody(int a_num)
        {
            string playerName;
            GameObject playerObject = null;

            switch (a_num)
            {
                case 1:
                    playerName = "Player1Stuff";
                    break;

                case 2:
                    playerName = "Player2Stuff";
                    break;

                case 3:
                    playerName = "Player3Stuff";
                    break;

                case 4:
                    playerName = "Player4Stuff";
                    break;

                default:
                    // Invalid player number
                    return null;
            }

            GameObject playerStuffObject = GameObject.Find(playerName);
            if (playerStuffObject == null)
            {
                // Player not present
                return null;
            }

            Transform playerObjectTransform = playerStuffObject.transform.FindChild("Airship_Prefab");
            if (playerObjectTransform == null)
            {
                // Can't find Airship prefab
                Debug.LogError("Unable to find airship prefab for player: " + a_num);
                Debug.Break();

                return null;
            }

            playerObject = playerObjectTransform.gameObject;

            return playerObject.GetComponent<Rigidbody>();
        }

        public void Awake()
        {
            // Get player rigidbodies
            m_playerRigidBodies.Add(GetPlayerRigidBody(1));
            m_playerRigidBodies.Add(GetPlayerRigidBody(2));
            m_playerRigidBodies.Add(GetPlayerRigidBody(3));
            m_playerRigidBodies.Add(GetPlayerRigidBody(4));
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
