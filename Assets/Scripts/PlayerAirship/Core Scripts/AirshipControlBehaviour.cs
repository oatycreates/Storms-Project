/**
 * File: AirshipControlBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Allows the player to control their airship via the InputManager class.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This is the basic airship control. In time this script will probably be changed a lot.
/// Many of the movement features here were expanded upon and derived from the Unity Standard Vehicle Assets - Plane Controller.
/// Unlike the old Storms Project, this game moves Non-Kinematic Rigidbodies through Physics (unlike Kinematic Rigidbody via direct control).
/// </summary>
public class AirshipControlBehaviour : MonoBehaviour
{	
    /// <summary>
    /// Movement speed of the airship.
    /// Try 50 here and see how we go.
    /// </summary>
	public float generalSpeed = 50.0f;

    /// <summary>
    /// How much pitching the vehicle will affect its facing.
    /// </summary>
    public float pitchForce = 1000.0f;
    /// <summary>
    /// How much yawing the vehicle will affect its facing.
    /// </summary>
    public float yawForce = 1000.0f;
    /// <summary>
    /// How much rolling the vehicle will affect its facing.
    /// </summary>
    public float rollForce = 1000.0f;

    /// <summary>
    /// Handle to the airship camera script.
    /// </summary>
    public AirshipCamBehaviour airshipMainCam;

    // Cached variables
    private Rigidbody m_myRigid;
	
	[HideInInspector]
	public float roll;
	[HideInInspector]
	public float pitch;
	[HideInInspector]
	public float yaw;
	[HideInInspector]
	public float throttle;
	
	
	void Awake()
	{
		m_myRigid = gameObject.GetComponent<Rigidbody>();
	}
	
	void Start () 
	{
        // Zero out the rigid body's velocities on start
		m_myRigid.velocity = Vector3.zero;
		m_myRigid.angularVelocity = Vector3.zero;
	}
	
	void Update()
	{
		// Set cam stuff
		airshipMainCam.camFollowPlayer = true;
	
		// Set rigidbody basics
		m_myRigid.useGravity = false;
		m_myRigid.isKinematic = false;	// We want physics collisions!
		
		m_myRigid.mass = 10.0f;
		m_myRigid.drag = 2.0f;
		m_myRigid.angularDrag = 2.0f;
	
	}
	
	
	public void PlayerInputs(float a_Vertical, float a_Horizontal, float a_camVertical, float a_camHorizontal, float a_triggers)
	{
		roll = a_Horizontal;
		pitch = a_Vertical;
		yaw = a_Horizontal;
		throttle = a_triggers;
		
        // Keep the inputs in reasonable ranges, see the standard asset examples for more
		ClampInputs();
	}
	
	void FixedUpdate()
	{
        // Doing this here makes it easier on the physics
		AutoLevel();
		ConstantForwardMovement();
		CalculateTorque();

	}
	
    /// <summary>
    /// Clamps the input values into the [-1, 1] range.
    /// </summary>
	private void ClampInputs()
	{
		roll = Mathf.Clamp(roll, -1, 1);
		pitch = Mathf.Clamp(pitch, -1, 1);
		yaw = Mathf.Clamp(yaw, -1, 1);
		throttle = Mathf.Clamp(throttle, -1, 1);
	}

	private void AutoLevel()
	{
		//I've left this blank for the time being... 
		//Maybe check over standard assets to see how they AUTO LEVEL
	}

    /// <summary>
    /// Moves the player's ship forward with either their constant movement speed or their inputted throttle.
    /// </summary>
	private void ConstantForwardMovement()
	{
		// 				general speed 	half or double general speed	+ 25% of general speed == Always a positive value
		float speedMod = generalSpeed + (throttle * generalSpeed) + (generalSpeed/4);
	
		m_myRigid.AddRelativeForce(Vector3.forward * speedMod , ForceMode.Acceleration);

        // This finds the 'up' vector. It was a cool trick from The Standard Vehicle Assets
		var liftDirection = Vector3.Cross(m_myRigid.velocity, m_myRigid.transform.right).normalized;
		
		m_myRigid.AddForce(liftDirection);
	}

	/// <summary>
	/// Calculates the rotation forces on the ship, see the standard assets example for more.
	/// </summary>
	private void CalculateTorque()
	{
		var torque = Vector3.zero;
		
		torque += -pitch * m_myRigid.transform.right * pitchForce;	
		
		torque += yaw * m_myRigid.transform.up * yawForce;
		
		torque += -roll * m_myRigid.transform.forward * rollForce;

        // Add all the torque forces together
		m_myRigid.AddTorque(torque);

	}
}
