﻿/**
 * File: AirshipSuicideBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the controlled kamakazi of the player ship.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This ship determines how the ship moves once the player has decided to "commit suicide" and play as a kamikazi 'Fire Ship'.
/// Compared to regular movement, this time the airship controls like a missile - Player has less control.
/// </summary>
public class AirshipSuicideBehaviour : MonoBehaviour
{
	public GameObject fireShipParticles;
	
    /// <summary>
    /// How long untill the player defaults back to the roulette selection screen?
    /// </summary>
	public float timerUntilReset = 15.0f;
	
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
    /// Handle to the airship camera script.
    /// </summary>
    public AirshipCamBehaviour airshipMainCam;

    // Animation trigger hashes
    private int m_animPropellerMult = Animator.StringToHash("PropellerMult");

    // Cached variables
    private Rigidbody m_myRigid;
    private Animator m_anim;
	
	void Awake()
	{
        m_myRigid = gameObject.GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
	}
	
	void Start() 
	{
		
	}

    void OnEnable()
    {

    }
	
	void Update()
	{
		// Set cam stuff
		airshipMainCam.camFollowPlayer = true;
	
		// Turn on particles
		fireShipParticles.SetActive(true);
		
		m_myRigid.useGravity = false;		
		
		// Time unil the player state resets
		timerUntilReset -= Time.deltaTime;
		
		if (timerUntilReset < 0.0f)
		{
			gameObject.GetComponent<StateManager>().currentPlayerState = EPlayerState.Roulette;
		}
	}
	
	
	void FixedUpdate()
	{
		Rocket();
		CalculateTorque();
	}
	
	public void PlayerFireshipInputs(
        float a_Vertical,
        float a_Horizontal)
	{
		pitch = a_Vertical;
		yaw = a_Horizontal;

        // Keep the inputs in reasonable ranges, see the standard asset examples for more
        ClampInputs();
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
}
