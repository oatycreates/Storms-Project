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
    /// How hard to explode the ship's contents away.
    /// </summary>
    public float explosionForce = 10.0f;

    /// <summary>
    /// How big to make the ship's explosion.
    /// </summary>
    public float explosionRadius = 3.0f;

    /// <summary>
    /// Where to centre the explosion from, this should be a transform relative to the ship prefab.
    /// </summary>
    public Transform explosionCentreTrans;

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
        m_myRigid = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
	}

    void Start()
    {
        if (explosionCentreTrans == null)
        {
            Debug.LogError("The explosion centre transform is not set!");
        }
    }
	
	void OnEnable()
	{
        // Explode the ship
        m_myRigid.AddExplosionForce(explosionForce, explosionCentreTrans.position, explosionRadius);

        // Stop the propeller from moving
        m_anim.SetFloat(m_animPropellerMult, 0.0f);
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
