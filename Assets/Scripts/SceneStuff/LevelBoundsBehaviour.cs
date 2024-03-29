﻿/**
 * File: LevelBoundsBehaviour.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 8/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Controls the behaviour of the level bounds
 *  game object, which serves as the center point of the level
 *  as well as, keeps players within the bounds of the level
 *  by pushing them back in, when outside of the bounds
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
	public class LevelBoundsBehaviour : MonoBehaviour 
	{
        public Rigidbody player1RigidBody;
        public Rigidbody player2RigidBody;
        public Rigidbody player3RigidBody;
        public Rigidbody player4RigidBody;

        // Player out of bounds, flags
        bool m_player1OutOfBounds = false;
        bool m_player2OutOfBounds = false;
        bool m_player3OutOfBounds = false;
        bool m_player4OutOfBounds = false;

        Transform m_transform;

        public void Awake()
        {
            m_transform = transform;
        }        
        
		void Start() 
		{
			
		}
		
        public void AssignPlayerRigidbodies(Rigidbody[] a_players)
        {
            player1RigidBody = a_players[0];
            player2RigidBody = a_players[1];
            player3RigidBody = a_players[2];
            player4RigidBody = a_players[3];
        }

		void Update() 
		{
			
		}

        public void FixedUpdate()
        {
            Vector3 levelOrigin = m_transform.position;

            // Keep player 1 within level bounds
            if (m_player1OutOfBounds && player1RigidBody != null)
            {
                Vector3 force = levelOrigin - player1RigidBody.transform.position;
                player1RigidBody.AddForce(force, ForceMode.Impulse);
            }

            // Keep player 2 within level bounds
            if (m_player2OutOfBounds && player2RigidBody != null)
            {
                Vector3 force = levelOrigin - player2RigidBody.transform.position;
                player2RigidBody.AddForce(force, ForceMode.Impulse);
            }

            // Keep player 3 within level bounds
            if (m_player3OutOfBounds && player3RigidBody != null)
            {
                Vector3 force = levelOrigin - player3RigidBody.transform.position;
                player3RigidBody.AddForce(force, ForceMode.Impulse);
            }

            // Keep player 4 within level bounds
            if (m_player4OutOfBounds && player4RigidBody != null)
            {
                Vector3 force = levelOrigin - player4RigidBody.transform.position;
                player4RigidBody.AddForce(force, ForceMode.Impulse);
            }
        }

        public void OnTriggerEnter(Collider a_other)
        {
            bool isPlayer =  a_other.GetComponent<AirshipControlBehaviour>() != null;

            if (isPlayer)
            {
                // Flag players which re-enter level bounds
                if (a_other.CompareTag("Player1_"))
                {
                    m_player1OutOfBounds = false;
                }
                else if (a_other.CompareTag("Player2_"))
                {
                    m_player2OutOfBounds = false;
                }
                else if (a_other.CompareTag("Player3_"))
                {
                    m_player3OutOfBounds = false;
                }
                else if (a_other.CompareTag("Player4_"))
                {
                    m_player4OutOfBounds = false;
                }
            }
        }

        public void OnTriggerExit(Collider a_other)
        {
            bool isPlayer =  a_other.GetComponent<AirshipControlBehaviour>() != null;

            if (isPlayer)
            {
                // Flag players which leave level bounds
                if (a_other.CompareTag("Player1_"))
                {
                    m_player1OutOfBounds = true;
                }
                else if (a_other.CompareTag("Player2_"))
                {
                    m_player2OutOfBounds = true;
                }
                else if (a_other.CompareTag("Player3_"))
                {
                    m_player3OutOfBounds = true;
                }
                else if (a_other.CompareTag("Player4_"))
                {
                    m_player4OutOfBounds = true;
                }
            }
        }
	}
}
