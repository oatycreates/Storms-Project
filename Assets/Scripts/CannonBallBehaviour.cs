/**
 * File: CannonBallBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 14/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This has all the behaviour for the cannonballs.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This has all the behaviour for the cannonballs.
/// </summary>
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(SphereCollider))]
public class CannonBallBehaviour : MonoBehaviour 
{
    /// <summary>
    /// Physical collider for the cannon ball.
    /// </summary>
    public Collider cannonBallCollider;

    /// <summary>
    /// For ignoring collisions with the owner's player ship & own base.
    /// </summary>
    public Collider selfCollisionTrigger;

	private float m_timer;

    /// <summary>
    /// Whether the collider has been disabled.
    /// </summary>
    private bool m_disabledCollider = false;

    /// <summary>
    /// Time the cannon-ball last self-triggered.
    /// </summary>
    private float m_lastSelfTriggerTime = 0.0f;

	void OnEnable()
	{
		m_timer = 5.0f;
	}

    void Start()
    {

    }
	
	void Update()
    {
		m_timer -= Time.deltaTime;
		
		if (m_timer <= 0)
		{
			gameObject.SetActive(false);
		}
	}

    void FixedUpdate()
    {
        m_lastSelfTriggerTime -= Time.deltaTime;
        if (m_disabledCollider && m_lastSelfTriggerTime <= 0)
        {
            // Re-enable the collider
            cannonBallCollider.enabled = true;

            // Reset collider disable status
            m_disabledCollider = false;
        }
    }

    void OnTriggerEnter(Collider a_other)
    {
        // Disable collider to prevent self-collision
        if (gameObject.CompareTag(a_other.tag))
        {
            cannonBallCollider.enabled = false;
            m_disabledCollider = true;

            m_lastSelfTriggerTime = 0.25f;
        }
    }

    void OnTriggerStay(Collider a_other)
    {
        // Disable collider to prevent self-collision
        if (gameObject.CompareTag(a_other.tag))
        {
            cannonBallCollider.enabled = false;
            m_disabledCollider = true;

            m_lastSelfTriggerTime = 0.25f;
        }
    }

	/*
	private AudioSource mySource;
	public AudioClip explosionNoise;

	private float timeOut = 5.0f;
	private float rememberTimeOut;
	
	private SphereCollider myCollider;
	
	void Awake()
	{
		mySource = gameObject.GetComponent<AudioSource>();
		myCollider = gameObject.GetComponent<SphereCollider>();
		
		mySource.clip = explosionNoise;
	}
	
	void Start()
	{
		rememberTimeOut = timeOut;
		
	}
	

	void OnEnable () 
	{
		timeOut = rememberTimeOut;	
		
		myCollider.isTrigger = true;
		
		// Turn the trigger back on once the cannonball has spawned.
		Invoke("TriggerOff", 1.0f);
	}
	
	
	void Update () 
	{
		timeOut -= Time.deltaTime;
		
		if (timeOut < 0)
		{
			gameObject.SetActive(false);
		}
		
		// Or if the cannonball has somehow fallen too far
		if (gameObject.transform.position.y < -2000)
		{
			gameObject.SetActive(false);
		}
	}
	
	void TriggerOff()
	{
		myCollider.isTrigger = false;
	}
	
	
	void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag != gameObject.tag)
		{	
			//Sound Stuff!!!
			if (explosionNoise != null)
			{
				mySource.Play();
			}
			else
			{
				Debug.Log("No sound on Cannonball!");
			}
			
			//Leave an impact
			if (other.gameObject.GetComponent<Rigidbody>() != null)
			{
				other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(5, gameObject.transform.position, 5.0f);
			}
			
			
			//Wipe me out!
			myCollider.isTrigger = true;
			gameObject.SetActive(false);
			
		}
	}
	*/
}
