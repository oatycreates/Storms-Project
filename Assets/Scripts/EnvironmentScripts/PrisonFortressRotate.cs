/**
 * File: PrisonFortressRotate.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the rotation and positioning of the prison fortress.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This script rotates the Prison Fortress ship in local space.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(AudioSource))]
public class PrisonFortressRotate : MonoBehaviour
{
    public float rotateForce = 10.0f;
    
    public AudioClip hoverNoise;

    // Cached variables
    private Rigidbody m_myRigid;
    private AudioSource mySource;

	void Start () 
	{
		//Set physics
		m_myRigid = gameObject.GetComponent<Rigidbody>();
		
		m_myRigid.maxAngularVelocity = 0.1f;
		
		//Set audio
		mySource = gameObject.GetComponent<AudioSource>();
		
		mySource.clip = hoverNoise;
		mySource.volume = 0.05f;
		//mySource.pitch = 
		mySource.loop = true;
		mySource.Play ();		
	}
	
	void FixedUpdate () 
	{
		m_myRigid.AddRelativeTorque(Vector3.up * rotateForce);
	}
}
