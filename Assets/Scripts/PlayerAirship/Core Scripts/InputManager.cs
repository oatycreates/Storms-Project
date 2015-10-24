/**
 * File: InputManager.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages player input across various devices, functions for the player linked to each gamepad.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if (UNITY_STANDALONE)
using XInputDotNetPure;
#endif

namespace ProjectStorms
{
    /// <summary>
    /// Just the raw controller inputs. These are mainly passed into the airship movement controls, but also effect suicide/fireship and roulette controls.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        /// A collection object to contain a particular piece of rumble information to be blended.
        /// </summary>
        [System.Serializable]
        public class ControllerRumbleInfo
        {
            /// <summary>
            /// Index of the player to apply this rumble to.
            /// </summary>
            public int playerIndex = 0;
            /// <summary>
            /// Left motor roll percentage.
            /// </summary>
            public float leftMotor = 0.0f;
            /// <summary>
            /// Right motor roll percentage.
            /// </summary>
            public float rightMotor = 0.0f;
            /// <summary>
            /// How long the rumble goes for.
            /// </summary>
            public float rumbleDurr = 0.0f;
            /// <summary>
            /// Timestamp at the start of the rumble.
            /// </summary>
            public float rumbleStart = 0.0f;

            /// <summary>
            /// Default constructor. Additional rumbles for a player are additive.
            /// </summary>
            /// <param name="a_playerTag">Index of the player to apply this rumble to. 0 is player 1.</param>
            /// <param name="a_motorLeft">Vibration value for the left controller motor.</param>
            /// <param name="a_motorRight">Vibration value for the right controller motor.</param>
            /// <param name="a_rumbleDurr">How long to rumble for in seconds.</param>
            public ControllerRumbleInfo(int a_playerIndex, float a_motorLeft, float a_motorRight, float a_rumbleDurr)
            {
                playerIndex = a_playerIndex;
                leftMotor = a_motorLeft;
                rightMotor = a_motorRight;
                rumbleDurr = a_rumbleDurr;
                rumbleStart = Time.time;
            }

            public bool IsRumbleExpired()
            {
                return Time.time - rumbleStart >= rumbleDurr;
            }
        }
        private static List<ControllerRumbleInfo> ms_currRumbles = new List<ControllerRumbleInfo>();

        private AirshipControlBehaviour m_standardControl;
        private AirshipSuicideBehaviour m_fireshipControl;
        private RouletteBehaviour m_rouletteControl;
        private RotateCam m_rotateCam;
        private BroadsideWeaponsController m_broadSideWeapons;
		private Countermeasures m_countermeasures;
		private StateManager m_stateManager;

        // TODO: We might need to add more script references here as we progress

        /// <summary>
        /// All of the player controller tags in order.
        /// </summary>
        private static string[] ms_playerTags = {"Player1_", "Player2_", "Player3_", "Player4_"};

        public void Awake()
        {
            m_standardControl   = GetComponent<AirshipControlBehaviour>();
            m_fireshipControl   = GetComponent<AirshipSuicideBehaviour>();
            m_rouletteControl   = GetComponent<RouletteBehaviour>();
            m_rotateCam         = GetComponent<RotateCam>();
            m_broadSideWeapons  = GetComponent<BroadsideWeaponsController>();
			m_countermeasures 	= GetComponent<Countermeasures> ();
			m_stateManager 		= GetComponent<StateManager>();
        }

        void Start()
        {

        }

        /// <summary>
        /// This input stuff was all figured out in an old script called 'TempDebugScript'.
        /// It's clever, because it determines which input to look for based off the player tag.
        /// 
        /// InputManager update is set to run before anything else.
        /// </summary>
        void Update()
        {
            // Apply any stored rumble information
            ApplyRumble();

            #region Axis Input
            // Left Stick Input	- One Stick to Determine Movement
            float upDown = Input.GetAxis(gameObject.tag + "Vertical");
            float leftRight = Input.GetAxis(gameObject.tag + "Horizontal");

            // Right Stick Input - Probably for Camera Control
            float camUpDown = Input.GetAxis(gameObject.tag + "CamVertical");
            float camLeftRight = Input.GetAxis(gameObject.tag + "CamHorizontal");

            // Dpad buttons
            float dpadHoriz = Input.GetAxis(gameObject.tag + "DPadHorizontal");
            float dpadVert = Input.GetAxis(gameObject.tag + "DPadVertical");
            bool dpadDown = dpadVert < 0;
            bool dpadUp = dpadVert > 0;
            bool dpadLeft = dpadHoriz < 0;
            bool dpadRight = dpadHoriz > 0;

            // Trigger Input - For acceleration
            float triggers = -Input.GetAxis(gameObject.tag + "Triggers");

            // DPad Input - For menus and such
            //float dPadUpDown = -Input.GetAxis(gameObject.tag + "DPadVertical");
            //float dPadLeftRight = Input.GetAxis(gameObject.tag + "DPadHorizontal");
            #endregion

            #region Button Input
            // Bumpers
            bool bumperLeft = Input.GetButton(gameObject.tag + "BumperLeft");
            bool bumperRight = Input.GetButton(gameObject.tag + "BumperRight");

            // Face Buttons
            bool faceDown = Input.GetButton(gameObject.tag + "FaceDown");
            bool faceLeft = Input.GetButton(gameObject.tag + "FaceLeft");
            bool faceRight = Input.GetButton(gameObject.tag + "FaceRight");
            bool faceUp = Input.GetButton(gameObject.tag + "FaceUp");

            // Start and Select
            bool select = Input.GetButton(gameObject.tag + "Select");
            //bool start = Input.GetButton(gameObject.tag + "Start");

            // Analogue Stick Clicks
            bool clickLeft = Input.GetButton(gameObject.tag + "ClickLeft");
            bool clickRight = Input.GetButton(gameObject.tag + "ClickRight");
            #endregion

            // Send variable data to individual scripts
            //m_rouletteControl.PlayerInput(faceDown, faceUp);	// Use the face button inputs to Stop/Start the roulette wheel
            m_standardControl.PlayerInputs(upDown, leftRight, camUpDown, camLeftRight, triggers, bumperLeft, bumperRight, faceUp, faceDown, faceLeft, faceRight, dpadHoriz, dpadVert);
            m_fireshipControl.PlayerFireshipInputs(upDown, leftRight, select);
            m_rotateCam.PlayerInputs(camUpDown, camLeftRight, triggers, faceDown, bumperLeft, bumperRight, clickLeft, clickRight);
            //m_broadSideWeapons.PlayerInputs(bumperLeft, bumperRight);
			//m_countermeasures.DPad (dpadDown, dpadUp, dpadLeft, dpadRight);
            m_countermeasures.FacePad(faceDown, faceUp, faceLeft, faceRight);
            m_stateManager.BoostButton(select, dpadUp, dpadDown);
      	}

        private static void ApplyRumble()
        {
            // Expire old rumble infos
            for (int i = 0; i < ms_currRumbles.Count; )
            {
                if (ms_currRumbles[i].IsRumbleExpired())
                {
                    // Remove the element at the index
                    ms_currRumbles.RemoveAt(i);
                }
                else
                {
                    // Only iterate if an element is not removed.
                    ++i;
                }
            }

            float sumLeft = 0;
            float sumRight = 0;
            for (int playerTag = 0; playerTag < ms_playerTags.Length; ++playerTag)
            {
                // Sum all rumbles with this tag
                sumLeft = 0;
                sumRight = 0;
                
                for (int i = 0; i < ms_currRumbles.Count; ++i)
                {
                    if (ms_currRumbles[i].playerIndex == playerTag)
                    {
                        sumLeft  += ms_currRumbles[i].leftMotor;
                        sumRight += ms_currRumbles[i].rightMotor;
                    }
                }
                
#if (UNITY_STANDALONE)
                // Apply rumble, will clear if sum is zero
                GamePad.SetVibration((PlayerIndex)playerTag, sumLeft, sumRight);
#endif
            }
        }

        /// <summary>
        /// Checks whether any input has been pressed for the input player.
        /// </summary>
        /// <param name="a_playerTag">Input player's tag.</param>
        /// <returns>True if any button was pressed, false if not.</returns>
        public static bool GetAnyButtonDown(string a_playerTag)
        {
            // Check if any button is down
            if (Input.GetButton(a_playerTag + "Start") ||
                Input.GetButton(a_playerTag + "Select") ||
                Input.GetButton(a_playerTag + "FaceDown") ||
                Input.GetButton(a_playerTag + "FaceUp") ||
                Input.GetButton(a_playerTag + "FaceLeft") ||
                Input.GetButton(a_playerTag + "FaceRight") ||
                Input.GetButton(a_playerTag + "BumperLeft") ||
                Input.GetButton(a_playerTag + "BumperRight") ||
                Input.GetButton(a_playerTag + "ClickLeft") ||
                Input.GetButton(a_playerTag + "ClickRight") ||
                Mathf.Abs(Input.GetAxisRaw(a_playerTag + "Triggers")) >= 0.25f ||
                Mathf.Abs(Input.GetAxisRaw(a_playerTag + "Horizontal")) >= 0.25f ||
                Mathf.Abs(Input.GetAxisRaw(a_playerTag + "Vertical")) >= 0.25f ||
                Mathf.Abs(Input.GetAxisRaw(a_playerTag + "CamHorizontal")) >= 0.25f ||
                Mathf.Abs(Input.GetAxisRaw(a_playerTag + "CamVertical")) >= 0.25f)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Makes the input controller vibrate.
        /// </summary>
        /// <param name="a_playerTag">Player tag. E.g. "Player1_"</param>
        /// <param name="a_motorLeft">Vibration value for the left controller motor. Should have a maximum of 1. Is a percentage value.</param>
        /// <param name="a_motorRight">Vibration value for the right controller motor. Should have a maximum of 1. Is a percentage value.</param>
        /// <param name="a_rumbleDurr">How long to rumble for in seconds.</param>
        /// <param name="a_shouldShakeScreen">True if the screen should also be shaken with the maximum</param>
        public static void SetControllerVibrate(string a_playerTag, float a_motorLeft, float a_motorRight, float a_rumbleDurr, bool a_shouldShakeScreen = false)
        {
            // Ignore null vibrations
            if (a_motorLeft != 0 || a_motorRight != 0)
            {
                // Find the player of the input tag
                for (int i = 0; i < ms_playerTags.Length; ++i)
                {
                    if (ms_playerTags[i].CompareTo(a_playerTag) == 0)
                    {
                        // Store the vibration for later
                        ms_currRumbles.Add(new ControllerRumbleInfo(i, Mathf.Min(a_motorLeft, 1.0f), Mathf.Min(a_motorRight, 1.0f), a_rumbleDurr));

                        if (a_shouldShakeScreen)
                        {
                            ShakeScreenForPlayer(a_playerTag, Mathf.Max(a_motorLeft, a_motorRight), a_rumbleDurr);
                        }
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Shakes the the screen for the input player.
        /// </summary>
        /// <param name="a_playerTag">Player tag. E.g. "Player1_"</param>
        /// <param name="a_shakeStr">Amplitude of the shake.</param>
        /// <param name="a_shakeDurr">Duration of the shake.</param>
        private static void ShakeScreenForPlayer(string a_playerTag, float a_shakeStr, float a_shakeDurr)
        {
            AirshipCamBehaviour[] cams = GameObject.FindObjectsOfType<AirshipCamBehaviour>();
            foreach (AirshipCamBehaviour cam in cams)
            {
                if (cam.CompareTag(a_playerTag))
                {
                    cam.ShakeCam(a_shakeStr, a_shakeDurr);
                    break;
                }
            }
        }
    } 
}
