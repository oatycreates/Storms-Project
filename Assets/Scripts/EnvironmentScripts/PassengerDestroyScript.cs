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
        private FallingScream m_scream;

        // Cached variables
        private Transform m_trans;

        void Awake()
        {
            m_trans = transform;
            m_scream = GetComponent<FallingScream>();
        }

        void Start()
        {
            m_trans.tag = "Passengers";
        }

        void Update()
        {
            if (gameObject.activeInHierarchy)
            {
                if (m_trans.position.y < heightTillDeath)
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