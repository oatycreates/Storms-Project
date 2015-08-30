/**
 * File: AirshipStallingBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the stalling of the player script.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// A simple script that sets the behaviour for the falling airship state.
    /// The player's ship will free-fall and the camera will stay in place and look at the ship.
    /// </summary>
    public class AirshipStallingBehaviour : MonoBehaviour
    {
        /// <summary>
        /// How fast the player ship will fall, defaults to the Earth gravitational constant.
        /// </summary>
        public float fallAcceleration = 100.0f;

        /// <summary>
        /// How long should the player watch their ship falling until it resets and takes them to the Roulette screen - experiment with this.
        /// </summary>
        [HideInInspector]
        public float timerUntilBoost = 0.0f;

        /// <summary>
        /// Multiplier to revert the player's control at. E.g. If 0.9 only revert to the control state if the player is below 90% of the stallY and moving down.
        /// </summary>
        public float stallYRevertMult = 0.9f;

        /// <summary>
        /// Handle to the airship camera script.
        /// </summary>
        public AirshipCamBehaviour airshipMainCam;

        // Animation trigger hashes
        private int m_animPropellerMult = Animator.StringToHash("PropellerMult");

        /// <summary>
        /// For storing the stall Y value when the player ship goes above it.
        /// </summary>
        private float m_cachedStallY = 1000.0f;

        /// <summary>
        /// Reset every time this script is entered, true if the player went above the stall Y. Will reset when they fall below the limit again. 
        /// </summary>
        private bool m_aboveStallY = false;

        // Cached variables
        private Rigidbody m_myRigid = null;
        private Transform m_trans = null;
        private Animator m_anim = null;
        private StateManager m_shipStates = null;
        private PassengerTray m_passTray = null;

        void Awake()
        {
            m_myRigid = GetComponent<Rigidbody>();
            m_trans = transform;
            m_anim = GetComponent<Animator>();
            m_shipStates = GetComponent<StateManager>();
            m_passTray = GetComponentInChildren<PassengerTray>();
        }

        void Start()
        {
            timerUntilBoost = 0.0f;
        }

        void OnEnable()
        {
            // Explode the ship tray
            m_passTray.ExplodeTray();

            // Stop the propeller from moving
            m_anim.SetFloat(m_animPropellerMult, 0.0f);

            //Reset the timer
            timerUntilBoost = 0.0f;

            //m_myRigid.useGravity = true;

            // This will be set later in the SetSetAboveStallY function if needed
            m_aboveStallY = false;
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

            if (m_aboveStallY)
            {
                // Only reset when the player falls back below the stall Y
                if (m_trans.position.y <= m_cachedStallY * stallYRevertMult && m_myRigid.velocity.y <= 0)
                {
                    // Revert back to the control state
                    m_shipStates.SetPlayerState(EPlayerState.Control);
                }
            }
            else
            {
                // Time until the player state resets
                if (timerUntilBoost < 10.0f)
                {
                    timerUntilBoost += Time.deltaTime;
                }

                /*
                if (timerUntilBoost <= 0.0f)
                {
                    // Reset the camera and change the play state
                    airshipMainCam.camFollowPlayer = true;
                    //gameObject.GetComponent<StateManager>().currentPlayerState = EPlayerState.Roulette;
                    //Skip Roulette for now - go to suicide or control
                    gameObject.GetComponent<StateManager>().currentPlayerState = EPlayerState.Suicide;
                }
                */
            }
        }

        /// <summary>
        /// Notifies this script that the player entered the state because they were above the stall Y. Will reset when they fall below the limit again.
        /// </summary>
        public void SetAboveStallY(float a_stallY)
        {
            m_cachedStallY = a_stallY;
            m_aboveStallY = true;
        }

        /*
        public void ResetTimer()
        {
            timerUntilBoost = hiddenValueReset;
        }*/
    } 
}
