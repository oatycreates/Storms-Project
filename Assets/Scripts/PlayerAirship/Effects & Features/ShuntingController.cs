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
    public class ShuntingController : MonoBehaviour
    {
        public float shuntingForce = 20.0f;
        [Tooltip("Cooldown until player is able to shunt again, in seconds")]
        public float cooldownTime = 0.5f;

        /// <summary>
        /// Reference to player's ship rigidbody.
        /// </summary>
        private Rigidbody m_rigidBody;
        /// <summary>
        /// Will be true if left bumper on player's controller is held.
        /// </summary>
        private bool m_leftTriggerDown = false;
        /// <summary>
        /// Will be true if right bumper on player's controller is held.
        /// </summary>
        private bool m_rightTriggerDown = false;
        /// <summary>
        /// Used to allow shunts on frame which a bumper is pressed, rather than held.
        /// </summary>
        private bool m_shuntApplied = false;
        /// <summary>
        /// Timer until shunt can be used again.
        /// </summary>
        private float m_currentCoolTime = 0.0f;

        public List<CannonFire> portCannons;
        public List<CannonFire> starboardCannons;

        /// <summary>
        /// Returns a quaternion which ignores the rotation on the Y axis
        /// </summary>
        private Quaternion rotationQuaternion
        {
            get
            {
                Vector3 rotationXZ = transform.rotation.eulerAngles;
                rotationXZ.y = 0.0f;
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

        }

        private void Update()
        {
            if (m_shuntApplied && !(m_leftTriggerDown || m_rightTriggerDown))
            {
                // Reset
                m_shuntApplied = false;
                m_currentCoolTime = 0.0f;
            }
            else
            {
                // Tick cool-down timer
                m_currentCoolTime += Time.deltaTime;
            }
        }

        public void FixedUpdate()
        {
            if (m_currentCoolTime <= cooldownTime)
            {
                return;
            }

            Quaternion rotationQuat = rotationQuaternion;

            Vector3 left = rotationQuat * -transform.right;
            Vector3 right = rotationQuat * transform.right;

            // Left shunt
            if (m_leftTriggerDown && !m_shuntApplied)
            {
                ApplyShunt(right);
                FireCannons(false);
            }

            // Right shunt
            if (m_rightTriggerDown && !m_shuntApplied)
            {
                ApplyShunt(left);
                FireCannons(true);
            }
        }

        private void FireCannons(bool a_fireStarboard)
        {
            List<CannonFire> cannons = a_fireStarboard ? starboardCannons : portCannons;

            for (int i = 0; i < cannons.Count; ++i)
            {
                cannons[i].Fire();
            }
        }

        private void ApplyShunt(Vector3 a_direction)
        {
            m_rigidBody.AddForce(a_direction * shuntingForce, ForceMode.Impulse);
            m_shuntApplied = true;
        }

        public void PlayerInputs(bool a_leftBumper, bool a_rightBumper)
        {
            // Zero input if not enabled
            if (!this.isActiveAndEnabled)
            {
                m_leftTriggerDown = false;
                m_rightTriggerDown = false;
            }
            else
            {
                m_leftTriggerDown = a_leftBumper;
                m_rightTriggerDown = a_rightBumper;
            }
        }
    } 
}
