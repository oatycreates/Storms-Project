/**
 * File: AirshipSuicideBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the controlled kamakazi of the player ship.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This ship determines how the ship moves once the player has decided to "commit suicide" and play as a kamikaze 'fire ship'.
    /// Compared to regular movement, this time the airship controls like a missile - the player has less control.
    /// </summary>
    public class AirshipSuicideBehaviour : MonoBehaviour
    {
        public GameObject fireShipParticles;

        /// <summary>
        /// How long until the player defaults back to the roulette selection screen?
        /// </summary>
        [HideInInspector]
        public float timerUntilReset = 0;
        private float hiddenResetValue;

        //Less inputs than the standard airship controller
        public float pitchForce = 1000.0f;
        public float yawForce = 1000.0f;
        //Currently, there is no input for 'Roll' - maybe this needs to be added?

        [HideInInspector]
        public float pitch;
        [HideInInspector]
        public float yaw;

        /// <summary>
        /// Percentage of the base propeller animation speed to apply when moving. 
        /// </summary>
        public float animThrottleMult = 10.0f;

        /// <summary>
        /// How strong to rumble.
        /// </summary>
        public float boostRumbleStr = 3.0f;
        /// <summary>
        /// How long to rumble for.
        /// </summary>
        public float boostRumbleDurr = 0.1f;

        /// <summary>
        /// Handle to the airship camera script.
        /// </summary>
        public AirshipCamBehaviour airshipMainCam;

        // Animation trigger hashes
        private int m_animPropellerMult = Animator.StringToHash("PropellerMult");

        // Cached variables
        private Rigidbody m_myRigid;
        private Animator m_anim;
        private StateManager m_stateManager;

        // Input variables
        private bool m_selectHeld     = false;    // Current state of the select button
        private bool m_lastSelectHeld = false;    // Last frame's state of the back button, for comparison

        void Awake()
        {
            m_myRigid = gameObject.GetComponent<Rigidbody>();
            m_anim = GetComponent<Animator>();
            m_stateManager = GetComponent<StateManager>();
        }

        void Start()
        {
            hiddenResetValue = timerUntilReset;
        }

        void Update()
        {
            // Set cam stuff
            airshipMainCam.camFollowPlayer = true;
            airshipMainCam.SuicideCam();

            // Turn on particles
            fireShipParticles.SetActive(true);

            m_myRigid.useGravity = false;

            // Time until the player state resets
            timerUntilReset -= Time.deltaTime;

            if (timerUntilReset < 0.0f || SelectButtonReleased())
            {
                // Try sending the airship back into control mode
                //m_stateManager.SetPlayerState(EPlayerState.Roulette);
                m_stateManager.SetPlayerState(EPlayerState.Control);
            }

			//Make controller vibrate - update every second
            InputManager.SetControllerVibrate(gameObject.tag, boostRumbleStr, boostRumbleStr, boostRumbleDurr, true);
        }

        void FixedUpdate()
        {
            Rocket();
            CalculateTorque();
        }

        public void PlayerFireshipInputs(
            float a_Vertical,
            float a_Horizontal, bool a_leftClick)
        {
            // Zero input if not enabled
            if (!this.isActiveAndEnabled)
            {
                pitch = 0;
                yaw = 0;

                m_lastSelectHeld = false;
                m_selectHeld = false;
            }
            else
            {
                pitch = a_Vertical;
                yaw = a_Horizontal;

                // Keep the inputs in reasonable ranges, see the standard asset examples for more
                ClampInputs();

                // Set back button state
                m_lastSelectHeld    = m_selectHeld;
                m_selectHeld        = a_leftClick;
            }
        }

        bool SelectButtonReleased()
        {
            return m_lastSelectHeld && !m_selectHeld;
        }

        /// <summary>
        /// Clamps the input values into the [-1, 1] range.
        /// </summary>
        void ClampInputs()
        {
            pitch = Mathf.Clamp(pitch, -1, 1);
            yaw = Mathf.Clamp(yaw, -1, 1);

        }

        /// <summary>
        /// Moves the player's ship forward with the rocket speed.
        /// </summary>
        private void Rocket()
        {

            m_myRigid.AddRelativeForce(Vector3.forward * 250, ForceMode.Impulse);

            // This finds the 'up' vector.
            var liftDirection = Vector3.Cross(m_myRigid.velocity, m_myRigid.transform.right).normalized;

            m_myRigid.AddForce(liftDirection);

            // Spin the propeller fast
            m_anim.SetFloat(m_animPropellerMult, animThrottleMult);
        }

        /// <summary>
        /// Calculates the rotation forces on the ship, see the standard assets example for more.
        /// </summary>
        private void CalculateTorque()
        {
            var torque = Vector3.zero;

            torque += -pitch * m_myRigid.transform.right * pitchForce;

            torque += yaw * m_myRigid.transform.up * yawForce;

            // No roll here

            // Add all the torque forces together
            m_myRigid.AddTorque(torque);
        }

        public void ResetTimer()
        {
            timerUntilReset = hiddenResetValue;
            if (fireShipParticles.activeSelf)
            {
                fireShipParticles.SetActive(false);
            }
        }
    } 
}
