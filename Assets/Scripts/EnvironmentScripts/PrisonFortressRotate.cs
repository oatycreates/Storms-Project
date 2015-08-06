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
public class PrisonFortressRotate : MonoBehaviour
{
    public float rotateForce = 10.0f;

    // Cached variables
    private Rigidbody m_myRigid;

	void Start () 
	{
		m_myRigid = gameObject.GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () 
	{
		m_myRigid.AddRelativeTorque(Vector3.up * rotateForce);
	}
}
