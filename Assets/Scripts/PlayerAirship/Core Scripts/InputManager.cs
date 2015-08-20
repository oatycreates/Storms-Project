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

/// <summary>
/// Just the raw controller inputs. These are mainly passed into the airship movement controls, but also effect suicide/fireship and roulette controls.
/// </summary>
public class InputManager : MonoBehaviour
{
	private AirshipControlBehaviour m_standardControl;
	private AirshipSuicideBehaviour m_fireshipControl;
	private RouletteBehaviour 		m_rouletteControl;
	private RotateCam 				m_rotateCam;


	// TODO We might need to add more script references here as we progress
    
#if (UNITY_IOS || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_TIZEN || UNITY_WP8 || UNITY_WP8_1)
    /// <summary>
    /// Cached reference to the platform's gyroscope.
    /// </summary>
	private Gyroscope m_gyro = null;

    /// <summary>
    /// Last rotation of the gyroscope.
    /// </summary>
    private Quaternion m_lastGyroRot = Quaternion.identity;

    /// <summary>
    /// Reference rotation of the device on start.
    /// </summary>
    private Quaternion m_referenceRot = Quaternion.identity;

    /// <summary>
    /// For filtering the gyroscope data.
    /// </summary>
    private float m_lowPassFilterFactor = 0.1f;
#endif

    void Start()
	{
		m_standardControl = gameObject.GetComponent<AirshipControlBehaviour>();
		m_fireshipControl = gameObject.GetComponent<AirshipSuicideBehaviour>();
		m_rouletteControl = gameObject.GetComponent<RouletteBehaviour>();
		m_rotateCam 	 = gameObject.GetComponent<RotateCam>();

#if (UNITY_IOS || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_TIZEN || UNITY_WP8 || UNITY_WP8_1)
        // Mobile control
        if (SystemInfo.supportsGyroscope)
        {
            m_gyro = Input.gyro;
            m_gyro.enabled = true;

            m_lastGyroRot = m_gyro.attitude;

            // Reset reference frame
            m_referenceRot = Quaternion.Inverse(m_lastGyroRot);
        }
#endif
    }

    /// <summary>
    /// This input stuff was all figured out in an old script called 'TempDebugScript'.
    /// It's clever, because it determines which input to look for based off the player tag.
    /// </summary>
    void FixedUpdate()
    {
        #region Axis Input
        // Left Stick Input	- One Stick to Determine Movement
        float upDown = Input.GetAxis(gameObject.tag + "Vertical");
        float leftRight = Input.GetAxis(gameObject.tag + "Horizontal");

        // Right Stick Input - Probably for Camera Control
        float camUpDown = Input.GetAxis(gameObject.tag + "CamVertical");
        float camLeftRight = Input.GetAxis(gameObject.tag + "CamHorizontal");

        // Trigger Input - For acceleration
        float triggers = -Input.GetAxis(gameObject.tag + "Triggers");

        // DPad Input - For menus and such
        float dPadUpDown = -Input.GetAxis(gameObject.tag + "DPadVertical");
        float dPadLeftRight = Input.GetAxis(gameObject.tag + "DPadHorizontal");
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
        bool start = Input.GetButton(gameObject.tag + "Start");

        // Analogue Stick Clicks
        bool clickLeft = Input.GetButton(gameObject.tag + "ClickLeft");
        bool clickRight = Input.GetButton(gameObject.tag + "ClickRight");
        #endregion

#if (UNITY_IOS || UNITY_ANDROID || UNITY_BLACKBERRY || UNITY_TIZEN || UNITY_WP8 || UNITY_WP8_1)
        // Mobile control for player 1
        if (m_gyro != null && tag.CompareTo("Player1_") == 0)
        {
            Quaternion rot = m_gyro.attitude;
            Vector3 gyroDown = m_gyro.gravity;

            // Set reference rotation if player wishes
            if (Input.touchCount >= 4)
            {
                // Reset reference frame on 4 finger multi-touch
                m_referenceRot = Quaternion.Inverse(rot);
            }

            // Filter the gyro rot
            rot = Quaternion.Slerp(m_lastGyroRot, rot, m_lowPassFilterFactor);

            // Work out the player's rotation
            Vector3 relativeRot = (rot * m_referenceRot).eulerAngles;

            // Convert the raw rotation value into a useful offset value
            Vector3 rotConv = relativeRot;
            if (relativeRot.x > 180.0f)
            {
                relativeRot.x = -(360.0f - relativeRot.x);
            }
            if (relativeRot.y > 180.0f)
            {
                relativeRot.y = -(360.0f - relativeRot.y);
            }
            if (relativeRot.z > 180.0f)
            {
                relativeRot.z = -(360.0f - relativeRot.z);
            }
            rotConv = relativeRot * (1 / 180.0f);

            // Deadzones
            float controlDeadzone = 0.2f;
            float rollDeadzone = 0.75f;

            // Pitch
            upDown = rotConv.y;
            if (Mathf.Abs(upDown) < controlDeadzone)
            {
                upDown = 0;
            }

            // Yaw
            leftRight = -rotConv.z;
            if (Mathf.Abs(leftRight) < controlDeadzone)
            {
                leftRight = 0;
            }

            // Roll
            faceLeft = rotConv.x > rollDeadzone;
            faceRight = rotConv.x < -rollDeadzone;

            // Dropping on tripple finger input
            faceUp = Input.touchCount == 3;
            faceDown = false;

            Debug.Log("Count: " + Input.touchCount + ", rot: " + rot.eulerAngles + ", raw: " + m_lastGyroRot.eulerAngles + ", relative: " + relativeRot + ", relative perc: " + rotConv + ", leftDot: " + leftRight + ", upDown: " + upDown);

            // TODO Accelerate on drag up on the right side of the screen

            // TODO Reverse on drag down on the left side of the screen

            // TODO If you shake it violently the prisoners drop

            m_lastGyroRot = rot;
        }
#endif

        // Send variable data to individual scripts
        m_rouletteControl.PlayerInput(faceDown, faceUp);	// Use the face button inputs to Stop/Start the roulette wheel
        m_standardControl.PlayerInputs(upDown, leftRight, camUpDown, camLeftRight, triggers, bumperLeft, bumperRight, faceUp, faceDown, faceLeft, faceRight);
        m_fireshipControl.PlayerFireshipInputs(upDown, leftRight);
        m_rotateCam.PlayerInputs(camUpDown, camLeftRight, triggers, bumperLeft, bumperRight, clickLeft, clickRight);
    }
}
