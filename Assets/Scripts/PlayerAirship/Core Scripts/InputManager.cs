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
    private ShuntingController      m_shuntingControl;

    // TODO: We might need to add more script references here as we progress

    public void Awake()
    {
        m_standardControl   = GetComponent<AirshipControlBehaviour>();
        m_fireshipControl   = GetComponent<AirshipSuicideBehaviour>();
        m_rouletteControl   = GetComponent<RouletteBehaviour>();
        m_rotateCam         = GetComponent<RotateCam>();
        m_shuntingControl   = GetComponent<ShuntingController>();
    }

	void Start()
	{
		
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

        // Send variable data to individual scripts
		m_rouletteControl.PlayerInput(faceDown, faceUp);	// Use the face button inputs to Stop/Start the roulette wheel
       	m_standardControl.PlayerInputs(upDown, leftRight, camUpDown, camLeftRight, triggers, bumperLeft, bumperRight, faceUp, faceDown, faceLeft, faceRight);
		m_fireshipControl.PlayerFireshipInputs(upDown, leftRight);
		m_rotateCam.PlayerInputs(camUpDown, camLeftRight, triggers, faceUp, bumperLeft, bumperRight, clickLeft, clickRight);
        m_shuntingControl.PlayerInputs(bumperLeft, bumperRight);
	}	
}
