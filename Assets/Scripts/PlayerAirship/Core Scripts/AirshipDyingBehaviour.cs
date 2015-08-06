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

/// <summary>
/// A simple script that sets the behaviour for the falling airship state. 
/// </summary>
public class AirshipDyingBehaviour : MonoBehaviour
{
    /// <summary>
    /// How fast the player ship will fall, defaults to the Earth gravitational constant.
    /// </summary>
	public float fallAcceleration = 9.8f;

    /// <summary>
    /// How long should the player watch their ship falling until it resets and takes them to the Roulette screen - experiment with this.
    /// </summary>
    public float timerUntilReset = 4.0f;

    /// <summary>
    /// Handle to the airship camera script.
    /// </summary>
    public AirshipCamBehaviour airshipMainCam;

    // Cached variables
    private Rigidbody m_myRigid;
	
	void Awake()
	{
		m_myRigid = gameObject.GetComponent<Rigidbody>();
	}
	
	void OnEnable()
	{
		m_myRigid.AddExplosionForce (10.0f, gameObject.transform.position, 3.0f);
	}
	
	void Update()
	{
		m_myRigid.useGravity = true;
		m_myRigid.AddForce(Vector3.down * fallAcceleration, ForceMode.Impulse);
		
		// Change the camera behaviour;
		airshipMainCam.camFollowPlayer = false;
		
		// Time unil the player state resets
		timerUntilReset -= Time.deltaTime;
		
		if (timerUntilReset < 0.0f)
		{
			// Reset the camera and change the play state
			airshipMainCam.camFollowPlayer = true;
			gameObject.GetComponent<StateManager>().currentPlayerState = EPlayerState.Roulette;
		}
	}
}
