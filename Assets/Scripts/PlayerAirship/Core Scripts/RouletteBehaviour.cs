/**
 * File: RoulletteBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Gives the player something to do when they die. A catch-up mechanic ala Super Mario Kart.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This script manages everything in the 'Roulette' / 'Lucky Dip' state. This only TRIGGERS the Roulette Spin. 
/// See Script: "RouletteSpinWheel" under Effects & Features for roulette functions.
/// The roulette state DOESN'T naturally 'Time-Out'. Maybe we should include this?
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class RouletteBehaviour : MonoBehaviour
{
    public GameObject fireParticleEffect;

    public RouletteSpinWheel spinWheel;

    /// <summary>
    /// Handle to the airship camera script.
    /// </summary>
	public AirshipCamBehaviour airshipMainCam;

    // Cached variables
    private Rigidbody m_myRigid;
    private Transform m_trans;
    private AirshipDyingBehaviour m_dyingBehaviour;
    private AirshipStallingBehaviour m_stallingBehaviour;
    private AirshipSuicideBehaviour m_suicideBehaviour;

	void Awake()
	{
		m_myRigid = GetComponent<Rigidbody>();
        m_trans = transform;
        m_dyingBehaviour = GetComponent<AirshipDyingBehaviour>();
        m_stallingBehaviour = GetComponent<AirshipStallingBehaviour>();
        m_suicideBehaviour = GetComponent<AirshipSuicideBehaviour>();
	}

	
	void Update()
	{
		// Set cam stuff
		airshipMainCam.camFollowPlayer = true;
	
		// Turn particles off here - it's a good place to reset
		fireParticleEffect.SetActive(false);
		
		// Remove the momentum of the player
		m_myRigid.velocity = Vector3.zero;
		m_myRigid.angularVelocity = Vector3.zero;
		
		// Reset the values on the other scripts- this way, they'll be ready the next time we need them
        m_suicideBehaviour.timerUntilReset = 15.0f;
        m_stallingBehaviour.timerUntilBoost = 4.0f;
	}
	
	public void PlayerInput(bool a_stopWheel, bool a_SpinFaster)
	{	
		// Pass these values directly into the spin-wheel script
		if (spinWheel != null)	
		{
			spinWheel.ChangeSpeed(a_stopWheel, a_SpinFaster);
		}
		else
		{
			Debug.LogWarning("No Reference to the Wooden Spin Wheel");
		}
	}
	
    /// <summary>
    /// Called by state manager.
    /// </summary>
    /// <param name="a_pos"></param>
    /// <param name="a_rot"></param>
	public void ResetPosition(Vector3 a_pos, Quaternion a_rot)
	{
        // Set world position and rotation
		m_trans.position = a_pos;
		m_trans.rotation = a_rot;

        // Reset the cam position as well!
        airshipMainCam.RouletteCam();
	}
}
