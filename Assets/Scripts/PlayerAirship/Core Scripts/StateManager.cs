/**
 * File: RoulletteBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick FergusonAirshi
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script organises all the different 'states' the player can be in. If we need to add more states, make sure to do them here.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// The state that the local player is currently in.
    /// </summary>
    public enum EPlayerState
    {
        Pregame,
        Roulette,
        Control,
        Dying,
        Stalling,
        Suicide
    };

    /// <summary>
    /// This script organises all the different 'States' the player can be in. If we need to add more States, make sure to do them here.
    /// The State Manager will automatically add these 6 scripts - they are vital to how the airship works.
    /// </summary>
    [RequireComponent(typeof(RouletteBehaviour))]
    [RequireComponent(typeof(AirshipControlBehaviour))]
    [RequireComponent(typeof(AirshipDyingBehaviour))]
    [RequireComponent(typeof(AirshipStallingBehaviour))]
    [RequireComponent(typeof(AirshipSuicideBehaviour))]
    [RequireComponent(typeof(InputManager))]
    [RequireComponent(typeof(TagChildren))]
    public class StateManager : MonoBehaviour
    {
        private EPlayerState m_currentPlayerState;

        // References to all the different state scripts
        private RouletteBehaviour m_rouletteScript;
        private AirshipControlBehaviour m_airshipScript;
        private AirshipDyingBehaviour m_dyingScript;
        private AirshipStallingBehaviour m_stallingScript;
        private AirshipSuicideBehaviour m_suicideScript;

        /// <summary>
        /// Good to make sure the airship HAS an input manager.
        /// </summary>
        //private InputManager m_inputManager;

        // References to the different components on the airship
        public GameObject colliders = null;
        public GameObject meshes = null;
        public GameObject hinges = null;
        public GameObject rouletteHierachy = null;
        public GameObject particlesEffectsHierachy = null;
        public GameObject weaponsHierachy = null;
        public GameObject playerReticle = null;

        /// <summary>
        /// Remember the start position and rotation in world space, so we can return here when the player has died.
        /// </summary>
        private Vector3 m_worldStartPos;
        private Quaternion m_worldStartRotation;

        private float stallCommit = 1.0f;
        private bool escapeStall = false;

        [HideInInspector]
        public float timeBetweenStall = 5.0f;

        /// <summary>
        /// State of the player last tick, used to detect state changes.
        /// </summary>
        private EPlayerState m_lastState;

        // Cached variables
        private Transform m_trans = null;
        private Rigidbody m_rb = null;
        private ShipPartDestroy m_shipParts = null;
        private ShuntingController m_shuntScript = null;

        void Awake()
        {
            m_trans = transform;
            m_rb = GetComponent<Rigidbody>();
            m_shipParts = GetComponent<ShipPartDestroy>();
            m_shuntScript = GetComponent<ShuntingController>();

            m_rouletteScript = GetComponent<RouletteBehaviour>();
            m_airshipScript = GetComponent<AirshipControlBehaviour>();
            m_dyingScript = GetComponent<AirshipDyingBehaviour>();
            m_stallingScript = GetComponent<AirshipStallingBehaviour>();
            m_suicideScript = GetComponent<AirshipSuicideBehaviour>();

            //m_inputManager = GetComponent<InputManager>();

            // World position & rotation
            m_worldStartPos = m_trans.position;
            m_worldStartRotation = m_trans.rotation;
        }

        void Start()
        {
            // Set the starting state
            SetPlayerState(EPlayerState.Pregame);
        }

        void Update()
        {
            // Hehehe
            //if (Application.isEditor == true)
            //{
            DevHacks();
            //}

            // Make the player commit to the STALL
            if (m_currentPlayerState == EPlayerState.Stalling)
            {
                stallCommit -= Time.deltaTime;
            }

            if (stallCommit < 0)
            {
                escapeStall = true;
            }
            else
            {
                escapeStall = false;
            }

            // Anti-spam function for stall
            timeBetweenStall -= Time.deltaTime;

            // Before the round begins, have a 'press any button to join' prompt
            if (m_currentPlayerState == EPlayerState.Pregame)
            {
                PregameUpdate();
            }

            // The player airship is not being used while the roulette wheel is spinning, the airship is deactivated
            if (m_currentPlayerState == EPlayerState.Roulette)
            {
                RouletteUpdate();
            }

            // Standard player airship control
            if (m_currentPlayerState == EPlayerState.Control)
            {
                ControlUpdate();
            }

            // Player has no-control over airship, but it's still affected by forces. Gravity is making the airship fall
            if (m_currentPlayerState == EPlayerState.Dying)
            {
                DyingUpdate();
            }

            // Player has no-control over airship, but it's still affected by forces. Gravity is making the airship fall
            if (m_currentPlayerState == EPlayerState.Stalling)
            {
                StallingUpdate();
            }

            // Recent addition- this is for the fireship/suicide function - the player has limited control here, needs further experimentation
            if (m_currentPlayerState == EPlayerState.Suicide)
            {
                SuicideUpdate();
            }
        }

        public EPlayerState GetPlayerState()
        {
            return m_currentPlayerState;
        }

        public void SetPlayerState(EPlayerState a_state)
        {
            m_currentPlayerState = a_state;
            switch (a_state)
            {
                case EPlayerState.Pregame:
                    {
                        ChangeToPregame();
                        break;
                    }
                case EPlayerState.Roulette:
                    {
                        ChangeToRoulette();
                        break;
                    }
                case EPlayerState.Control:
                    {
                        ChangeToControl();
                        break;
                    }
                case EPlayerState.Dying:
                    {
                        ChangeToDying();
                        break;
                    }
                case EPlayerState.Stalling:
                    {
                        ChangeToStalling();
                        break;
                    }
                case EPlayerState.Suicide:
                    {
                        ChangeToSuicide();
                        break;
                    }
                default:
                    {
                        Debug.LogWarning("Changing to unknown state!");
                        break;
                    }
            }
        }

        private bool GetAnyButtonDownMyPlayer()
        {
            return InputManager.GetAnyButtonDown(gameObject.tag);
        }

        private void PregameUpdate()
        {
            if (GetAnyButtonDownMyPlayer())
            {
                // Switch to the next state
                SetPlayerState(EPlayerState.Control);
            }
        }

        private void SuicideUpdate()
        {

        }

        private void DyingUpdate()
        {

        }

        private void StallingUpdate()
        {

        }

        private void ControlUpdate()
        {
            m_suicideScript.ResetTimer();
        }

        private void RouletteUpdate()
        {

        }

        private void ResetPlayerShip()
        {
            // Reset position
            m_rouletteScript.ResetPosition(m_worldStartPos, m_worldStartRotation);

            // Reset velocity
            m_rb.velocity = Vector3.zero;
            m_rb.angularVelocity = Vector3.zero;

            // Repair the player ship
            m_shipParts.RepairAllParts();
        }

        private void ChangeToPregame()
        {
            // Disable all scripts
            m_rouletteScript.enabled = false;
            m_airshipScript.enabled = false;
            m_dyingScript.enabled = false;
            m_stallingScript.enabled = false;
            m_suicideScript.enabled = false;
            m_shuntScript.enabled = false;

            // Revert the ship back to the start
            ResetPlayerShip();

            ToggleShipParts(true, true, true, false, false, false);
        }

        private void ChangeToSuicide()
        {
            // Airship behaves like a rocket
            m_rouletteScript.enabled = false;
            m_airshipScript.enabled = false;
            m_dyingScript.enabled = false;
            m_stallingScript.enabled = false;
            m_suicideScript.enabled = true;
            m_shuntScript.enabled = false;

            ToggleShipParts(true, true, true, false, true, false);
        }

        private void ChangeToDying()
        {
            // No Control, gravity makes airship fall
            m_rouletteScript.enabled = false;
            m_airshipScript.enabled = false;
            m_dyingScript.enabled = true;
            m_stallingScript.enabled = false;
            m_suicideScript.enabled = false;
            m_shuntScript.enabled = false;

            ToggleShipParts(true, true, true, false, true, false);
        }

        private void ChangeToStalling()
        {
            // No Control, gravity makes airship fall
            m_rouletteScript.enabled = false;
            m_airshipScript.enabled = false;
            m_dyingScript.enabled = false;
            m_stallingScript.enabled = true;
            m_suicideScript.enabled = false;
            m_shuntScript.enabled = false;

            ToggleShipParts(true, true, true, false, true, false);
        }

        private void ChangeToControl()
        {
            // Standard Physics Control
            m_rouletteScript.enabled = false;
            m_airshipScript.enabled = true;
            m_dyingScript.enabled = false;
            m_stallingScript.enabled = false;
            m_suicideScript.enabled = false;
            m_shuntScript.enabled = true;

            ToggleShipParts(true, true, true, false, true, true);
        }

        private void ChangeToRoulette()
        {
            // Roulette control
            m_rouletteScript.enabled = true;
            m_airshipScript.enabled = false;
            m_dyingScript.enabled = false;
            m_stallingScript.enabled = false;
            m_suicideScript.enabled = false;
            m_shuntScript.enabled = false;

            // Revert the ship back to the start
            ResetPlayerShip();

            // We don't need to see the airship during the roulette wheel
            ToggleShipParts(false, false, false, true, false, false);
        }

        /// <summary>
        /// Toggles the ship's component parts and sub-children for the current state.
        /// </summary>
        /// <param name="a_colEnable">Whether to enable the collider hierarchy.</param>
        /// <param name="a_meshEnable">Whether to enable the mesh hierarchy.</param>
        /// <param name="a_hingesEnable">Whether to enable the hinges hierarchy.</param>
        /// <param name="a_rouletteEnable">Whether to enable the roulette hierarchy.</param>
        /// <param name="a_particlesEnable">Whether to enable the particles hierarchy.</param>
        /// <param name="a_weaponsEnable">Whether to enable the weapons hierarchy.</param>
        private void ToggleShipParts(bool a_colEnable, bool a_meshEnable, bool a_hingesEnable, bool a_rouletteEnable, bool a_particlesEnable, bool a_weaponsEnable)
        {
            if (colliders != null)
            {
                colliders.SetActive(a_colEnable);
            }

            if (meshes != null)
            {
                meshes.SetActive(a_meshEnable);
            }

            if (hinges != null)
            {
                hinges.SetActive(a_hingesEnable);
            }

            if (rouletteHierachy != null)
            {
                rouletteHierachy.SetActive(a_rouletteEnable);
            }

            if (particlesEffectsHierachy != null)
            {
                particlesEffectsHierachy.SetActive(a_particlesEnable);
            }

            if (weaponsHierachy != null)
            {
                weaponsHierachy.SetActive(a_weaponsEnable);
                playerReticle.SetActive(a_weaponsEnable);
            }
        }

        /// <summary>
        /// Skip to next EPlayerState.
        /// If we add more states, make sure we add functionality here.
        /// We've intentionally left out Roulette for the Time being...........
        /// </summary>
        void DevHacks()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                //SetPlayerState(EPlayerState.Roulette);
                SetPlayerState(EPlayerState.Pregame);
            }

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SetPlayerState(EPlayerState.Control);
            }

            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SetPlayerState(EPlayerState.Dying);
            }

            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SetPlayerState(EPlayerState.Stalling);
            }

            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                SetPlayerState(EPlayerState.Suicide);
            }

            //Old code- consider removing
            /*
            //inputs
            if (Input.GetButtonDown(gameObject.tag + "Select"))
            {
                if (currentPlayerState == EPlayerState.Roulette)
                {
                    currentPlayerState = EPlayerState.Control;
                }
                else
                if (currentPlayerState == EPlayerState.Control)
                {
                    currentPlayerState = EPlayerState.Dying;
                }
                else
                if (currentPlayerState == EPlayerState.Dying)
                {
                    //currentPlayerState = EPlayerState.Suicide;
                }
                else
                if (currentPlayerState == EPlayerState.Suicide)
                {
                    //currentPlayerState = EPlayerState.Roulette;
                    currentPlayerState = EPlayerState.Control;
                }
            }
            */
            if (m_currentPlayerState == EPlayerState.Control)
            {
                if (Input.GetButtonDown(gameObject.tag + "Select"))
                {
                    if (timeBetweenStall < 0)
                    {
                        SetPlayerState(EPlayerState.Stalling);
                    }
                }
            }

            // Stop spamming stall
            if (m_currentPlayerState == EPlayerState.Stalling || m_currentPlayerState == EPlayerState.Suicide || m_currentPlayerState == EPlayerState.Roulette)
            {
                timeBetweenStall = 5.0f;
            }

            // Hacky!! Make auto stall an option
            if (m_currentPlayerState == EPlayerState.Stalling)
            {

                // If the button is not down, but the player is allowed to escape the stall anyway.
                if (!Input.GetButton(gameObject.tag + "Select"))
                {
                    if (escapeStall)
                    {
                        //Take a Stall value and pass it into the suicide script.
                        float timer = m_stallingScript.timerUntilBoost;

                        SetPlayerState(EPlayerState.Suicide);

                        m_suicideScript.timerUntilReset = timer;
                    }
                }
            }

            if (m_currentPlayerState == EPlayerState.Control)
            {
                //Then player is not stalling
                stallCommit = 1.0f;
            }


            if (Input.GetButtonDown(gameObject.tag + "Start"))
            {
                Application.LoadLevel(Application.loadedLevelName);
            }
        }
    } 
}
