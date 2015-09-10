/**
 * File: ShuntingController.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour, Patrick Ferguson
 * Created: 28/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Controls the shunting ability for players ship movement
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
    public class BroadsideWeaponsController : MonoBehaviour
    {
        public float shuntingForce = 20.0f;
        [Tooltip("Cooldown until player is able to shunt again, in seconds")]
        public float cooldownTime = 0.5f;

        public List<CannonFire> portCannons;
        public List<CannonFire> starboardCannons;

        /// <summary>
        /// Reference to player's ship rigidbody.
        /// </summary>
        private Rigidbody m_rigidBody;

        /// <summary>
        /// Will be true if left bumper on player's controller is held.
        /// </summary>
        private bool m_leftBumperDown = false;
        /// <summary>
        /// Will be true if right bumper on player's controller is held.
        /// </summary>
        private bool m_rightBumperDown = false;
        /// <summary>
        /// Last frame's left bumper state
        /// </summary>
        private bool m_lastLeftBumperDown  = false;
        /// <summary>
        /// Last frame's right bumper state
        /// </summary>
        private bool m_lastRightBumperDown = false;

        /// <summary>
        /// Current timer for starboard cannon firing
        /// </summary>
        private float m_starboardTimer    = 0.0f;
        /// <summary>
        /// Current timer for port cannon firing
        /// </summary>
        private float m_portTimer         = 0.0f;

        /// <summary>
        /// Returns a quaternion which ignores the rotation on the Y axis
        /// </summary>
        private Quaternion rotationQuaternion
        {
            get
            {
                Vector3 rotationXZ  = transform.rotation.eulerAngles;
                rotationXZ.y        = 0.0f;

                Quaternion rotationQuat = Quaternion.Euler(rotationXZ);

                return rotationQuat;
            }
        }

        public void Awake()
        {
            m_rigidBody = GetComponent<Rigidbody>();
        }

        private void Start()
        {
            // Ensure player is able to shoot at round start
            m_portTimer       = cooldownTime;
            m_starboardTimer  = cooldownTime;
        }

        private void Update()
        {
            // Tick port firing timer
            if (m_portTimer < cooldownTime)
            {
                m_portTimer += Time.deltaTime;
            }

            // Tick starboard firing timer
            if (m_starboardTimer < cooldownTime)
            {
                m_starboardTimer += Time.deltaTime;
            }
        }

        public void FixedUpdate()
        {
            Quaternion rotationQuat = rotationQuaternion;

            Vector3 leftShunt_dir  = rotationQuat * -transform.right;
            Vector3 rightShunt_dir = rotationQuat * transform.right;

            // Left bumper pressed
            if (m_leftBumperDown && !m_lastLeftBumperDown)
            {
                if (m_portTimer >= cooldownTime)
                {
                    FireCannons(false);
                    ApplyShunt(rightShunt_dir);

                    m_portTimer = 0.0f;
                }
            }

            // Right bumper pressed
            if (m_rightBumperDown && !m_lastRightBumperDown)
            {
                if (m_starboardTimer >= cooldownTime)
                {
                    FireCannons(true);
                    ApplyShunt(leftShunt_dir);

                    m_starboardTimer = 0.0f;
                }
            }

            // Store trigger state for next frame comparison
            // (ensures events trigger on first frame triggers are held)
            m_lastLeftBumperDown   = m_leftBumperDown;
            m_lastRightBumperDown  = m_rightBumperDown;
        }

        /// <summary>
        /// Fire a set of boardside cannons
        /// </summary>
        /// <param name="a_fireStarboard">If true, will fire starboard side cannons, 
        /// else will fire portside cannons</param>
        private void FireCannons(bool a_fireStarboard)
        {
            // Fire cannons
            List<CannonFire> cannons = a_fireStarboard ? starboardCannons : portCannons;

            for (int i = 0; i < cannons.Count; ++i)
            {
                cannons[i].Fire();
            }
        }

        /// <summary>
        /// Applies a shunt to the player's ship in a given direction
        /// </summary>
        /// <param name="a_direction">Direction of shunt being applied</param>
        private void ApplyShunt(Vector3 a_direction)
        {
            m_rigidBody.AddForce(a_direction * shuntingForce, ForceMode.Impulse);
        }

        /// <summary>
        /// Helper function, to pass current state of player's input into this component
        /// </summary>
        /// <param name="a_leftBumper">Current state of left bumper for this player</param>
        /// <param name="a_rightBumper">Current state of right bumper for this player</param>
        public void PlayerInputs(bool a_leftBumper, bool a_rightBumper)
        {
            // Zero input if not enabled
            if (!this.isActiveAndEnabled)
            {
                m_leftBumperDown = false;
                m_rightBumperDown = false;
            }
            else
            {
                m_leftBumperDown = a_leftBumper;
                m_rightBumperDown = a_rightBumper;
            }
        }
    } 
}
