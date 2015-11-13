/**
 * File: AutoOpenTray.cs
 * Author: Patrick Ferguson
 * Maintainer: Patrick Ferguson
 * Created: 13/11/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Automatically opens the player's passengery tray in this zone.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// Pinwheel blade trigger. Triggers a physical response if any rigidbody enters its collider (after a brief interval)
    /// </summary>
    public class AutoOpenTray : MonoBehaviour
    {
        private AirshipControlBehaviour m_myPlayer = null;
        private PassengerTray m_myTray = null;

        /// <summary>
        /// Last time the tray was open.
        /// </summary>
        private float m_lastForceOpenTime = -1.0f;

        void Start()
        {

        }

        void Update()
        {
            if (Time.time - m_lastForceOpenTime >= 2.5f && m_myPlayer != null)
            {
                m_myPlayer.forceOpenHatch = false;
            }
        }

        void OnTriggerEnter(Collider a_other)
        {
            // Find the passenger tray
            AirshipControlBehaviour shipScript = a_other.GetComponentInParent<AirshipControlBehaviour>();
            if (shipScript != null)
            {
                if (m_myPlayer == null)
                {
                    m_myPlayer = shipScript;
                    m_myTray = shipScript.GetComponentInChildren<PassengerTray>();
                }

                if (m_myPlayer == shipScript)
                {
                    m_myPlayer.forceOpenHatch = (Time.time - m_myTray.lastTrayTime) <= 0.1f;

                    m_lastForceOpenTime = Time.time;
                }
            }
        }

        void OnTriggerLeave(Collider a_other)
        {
            // Find the passenger tray
            AirshipControlBehaviour shipScript = a_other.GetComponentInParent<AirshipControlBehaviour>();
            if (shipScript != null)
            {
                if (m_myPlayer == null)
                {
                    m_myPlayer = shipScript;
                    m_myTray = shipScript.GetComponentInChildren<PassengerTray>();
                }

                if (m_myPlayer == shipScript)
                {
                    shipScript.forceOpenHatch = false;
                }
            }
        }
    }
}