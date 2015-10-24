/**
 * File: AirshipDyingBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the death of the player script.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// A simple script that sets the behaviour for the falling airship state.
    /// The player's ship will free-fall and the camera will stay in place and look at the ship.
    /// </summary>
    public class AirshipDyingBehaviour : MonoBehaviour
    {
        /// <summary>
        /// How fast the player ship will fall, defaults to the Earth gravitational constant.
        /// </summary>
        public float fallAcceleration = 100.0f;

        /// <summary>
        /// How long should the player watch their ship falling until it resets and takes them to the Roulette screen - experiment with this.
        /// </summary>
       	public float timerUntilReset = 4.0f;

        /// <summary>
        /// Handle to the airship camera script.
        /// </summary>
        public AirshipCamBehaviour airshipMainCam;

        /// <summary>
        /// Actual time until the ship reset occurs.
        /// </summary>
        private float m_resetTimer = 0.0f;

        // Animation trigger hashes
        private int m_animPropellerMult = Animator.StringToHash("PropellerMult");

        // Cached variables
        private Rigidbody m_myRigid = null;
        private Animator m_anim = null;
        private StateManager m_shipStates = null;
        private PassengerTray m_passTray = null;

        void Awake()
        {
            m_myRigid = GetComponent<Rigidbody>();
            m_anim = GetComponent<Animator>();
            m_shipStates = GetComponent<StateManager>();
            m_passTray = GetComponentInChildren<PassengerTray>();
        }

        void Start()
        {
           m_resetTimer = timerUntilReset;
        }

        void OnEnable()
        {
            // Explode the ship tray
            m_passTray.ExplodeTray();

            // Stop the propeller from moving
            m_anim.SetFloat(m_animPropellerMult, 0.0f);

            //Reset the timer
            m_resetTimer = timerUntilReset;

            //m_myRigid.useGravity = true;
        }

        void FixedUpdate()
        {
            // Add downwards acceleration
            m_myRigid.AddForce(Vector3.down * fallAcceleration, ForceMode.Impulse);
        }

        void Update()
        {
            // Change the camera behaviour;
            airshipMainCam.camFollowPlayer = false;

            // Time unil the player state resets
            if (m_resetTimer > 0.0f)
            {
                m_resetTimer -= Time.deltaTime;
            }

            if (m_resetTimer <= 0.0f)
            {
                // Reset the camera and change the play state
                airshipMainCam.camFollowPlayer = true;

                //Skip Roulette for now - go to suicide or control
                m_shipStates.SetPlayerState(EPlayerState.Pregame);
            }
        }

        /*
        public void ResetTimer()
        {
            timerUntilBoost = hiddenValueReset;
        }*/
    } 
}
