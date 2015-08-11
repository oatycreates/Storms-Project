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
	/// Change the mass of the Airship in editor here
	/// </summary>
	public float adjustableMass = 100.0f;

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
    /// Multiplier for the pitch auto-level force.
    /// </summary>
    public float pitchLimitMult = 0.7f;

    /// <summary>
    /// Multiplier for the pitch auto-level force.
    /// </summary>
    public float rollLimitMult = 0.8f;

    /// <summary>
    /// Percentage of the base propeller animation speed to apply when moving.
    /// </summary>
    public float animThrottleMult = 2.5f;

    /// <summary>
    /// Percentage of the base propeller animation speed to apply when braking.
    /// </summary>
    public float animLowThrottleMult = 0.1f;

    /// <summary>
    /// Handle to the airship camera script.
    /// </summary>
    public AirshipCamBehaviour airshipMainCam;
    
    /// <summary>
    /// Bounce the input variables to the Audio Controller script under 'Particles and Effects' hierarchy branch.
    /// </summary>
    public AirshipAudio audioControl;
    public EngineAudio engineAudioControl;

    // Animation trigger hashes
    private int m_animHatchOpen     = Animator.StringToHash("HatchOpen");
    private int m_animTrapdoorOpen  = Animator.StringToHash("TrapdoorOpen");
    private int m_animPropellerMult = Animator.StringToHash("PropellerMult");

    // Cached variables
    private Rigidbody m_myRigid;
    private Animator m_anim;
	
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
		m_myRigid = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
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
		
		//m_myRigid.mass = 10.0f;
		m_myRigid.mass = adjustableMass;
		m_myRigid.drag = 2.0f;
		m_myRigid.angularDrag = 2.0f;
		
	}


    public void PlayerInputs(
        float a_Vertical, 
        float a_Horizontal, 
        float a_camVertical, 
        float a_camHorizontal, 
        float a_triggers, 
        bool a_faceUp,      // Y - Open hatch
        bool a_faceDown,    // A - Fire cannon forwards
        bool a_faceLeft,    // X - Fire broadside left
        bool a_faceRight)   // B - Fire broadside right
	{
        roll = 0.25f * a_Horizontal + a_camHorizontal;
		pitch = a_Vertical;
		yaw = a_Horizontal;
		throttle = a_triggers;
		
        // Keep the inputs in reasonable ranges, see the standard asset examples for more
		ClampInputs();

        // Open/close the hatches
        m_anim.SetBool(m_animHatchOpen, a_faceUp);
        m_anim.SetBool(m_animTrapdoorOpen, a_faceUp);

        // Spin the propeller
        float origThrottleBounded = throttle * 0.5f + 0.5f; // [-1, 1] to [0, 1]
        float animThrottle = origThrottleBounded * 2.0f; // [0, 1] to [0, 2], 50% now maps to 100% anim throttle
        if (animThrottle > 1)
        {
            // Scale up to the animation throttle multiplier
            animThrottle = origThrottleBounded * (animThrottleMult - 1) + 1;
        }
        else if (animThrottle < 0.05f)
        {
            // Cap to low speed
            animThrottle = animLowThrottleMult;
        }
        m_anim.SetFloat(m_animPropellerMult, animThrottle);
		
	}
	
	void FixedUpdate()
	{
        // Doing this here makes it easier on the physics
		ConstantForwardMovement();
		CalculateTorque();
        CalculateRightingForce();
        
		//Pass values to AudioController
		if (audioControl != null)
		{
			audioControl.AudioInputs(pitch, yaw, roll);
		}
		
		if (engineAudioControl != null)
		{
			engineAudioControl.AudioInput(throttle);
		}
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

    /// <summary>
    /// Attempts to auto-level the player's ship. Counters roll moreso than pitch, ignores yaw.
    /// </summary>
    private void CalculateRightingForce()
    {
        // Calculate a few useful vectors relative to the ship and the world
        Vector3 worldUp = new Vector3(0, 1, 0); // Up relative to the world
        Vector3 shipForward = transform.forward; // Front relative to the ship
        Vector3 shipRight = transform.right; // Right relative to the ship

        var torque = Vector3.zero;

        // Roll
        float rollDot = Vector3.Dot(worldUp, shipRight);
        torque += -rollDot * shipForward * rollForce * rollLimitMult;

        // Pitch
        float pitchDot = Vector3.Dot(-worldUp, shipForward);
        torque += -pitchDot * shipRight * pitchForce * pitchLimitMult;

        // Add all the torque forces together
        m_myRigid.AddTorque(torque);
    }
}
