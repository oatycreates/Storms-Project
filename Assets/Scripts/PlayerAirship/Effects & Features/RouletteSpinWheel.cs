/**
 * File: RouletteSpinWheel.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Gives the player
 **/

using UnityEngine;
using System.Collections;

public class RouletteSpinWheel : MonoBehaviour // This script manages the 'Spinning Wheel' effect on the Roulette Wheel
												// Inputs are passed in from Input Manager script VIA the Roulette Behaviour State script.
{
	private float changeAngularDrag;
	
	public GameObject rotatorJoint;
	private bool pullHandle = false;
	private float rotateAmount;
	
	private bool inputStop;
	private bool inputSpeedUp;

    /// <summary>
    /// For making the roulette wheel's spin finish on a 90 degree boundary. 
    /// </summary>
    private Quaternion targetFinalRotation = Quaternion.identity;

    // Cached variables
    private Rigidbody m_myRigid;
    private Transform m_trans;
    private Transform m_rotatorTrans;

    void Start()
    {
        m_rotatorTrans = rotatorJoint.transform;
        m_trans = transform;
    }

	void OnEnable() 
	{

		// Reset Position
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		gameObject.transform.localScale = new Vector3(4, 1, 1);	// Try not to change these
	
		// Set/Reset physics components
		m_myRigid = gameObject.GetComponent<Rigidbody>();
		// Set Components of the rigidbody in the start.
		
		m_myRigid.isKinematic = false;
		m_myRigid.useGravity = false;
		
		m_myRigid.drag = 0;
		m_myRigid.angularDrag = 0.001f;	// Low on start

        m_myRigid.maxAngularVelocity = 14;

        targetFinalRotation = Quaternion.identity;
		
		// Get the start rotation of the handle joint
		rotateAmount = 0;
		
		//Make the wheel spin
		Spin();
	}
	
	void Update() 
	{
        // For testing- make the roulette wheel spin faster
		if (inputSpeedUp)
		{
			Spin();
		}

        // Slow the roulette wheel by increasing the rigidbody drag
		if (inputStop)
		{
			m_myRigid.angularDrag = Mathf.Lerp(m_myRigid.angularDrag, 1, 0.25f);	
			
			pullHandle = true;
		}

        //Handle effect
		if (pullHandle)
		{
			rotateAmount = Mathf.Lerp(rotateAmount, -90, Time.deltaTime * 5.0f);
		}
		
        // Make handle return to original position
		if (m_rotatorTrans.localEulerAngles.x == 270)
		{
			pullHandle = false;
		}
		
		// Make handle return to original position
		if ((!pullHandle) && (!inputStop))
		{
			rotateAmount = Mathf.Lerp(rotateAmount, 0, Time.deltaTime * 10.0f);
		}
		
		// Lock the rotation of the handle to One Axis
		float rotationY = m_rotatorTrans.localEulerAngles.y;
		float rotationZ = m_rotatorTrans.localEulerAngles.z;
		// Move the joint
		m_rotatorTrans.localRotation = Quaternion.Euler(rotateAmount, rotationY, rotationZ);

        // Slerp into the final position if applicable
        if (targetFinalRotation != Quaternion.identity)
        {
            m_trans.localRotation = Quaternion.Slerp(m_trans.localRotation, targetFinalRotation, Time.deltaTime * 5.0f);
        }

		//NOTE: Right now- this just triggers the State Manager to change the object from ROULETE to NORMAL CONTROL
		if (m_myRigid.angularVelocity.x > -1.0f && m_myRigid.angularVelocity.x < 0)
        {
            int selectedIndex = Mathf.RoundToInt(m_myRigid.rotation.eulerAngles.x / 90.0f);
            if (Mathf.RoundToInt(m_myRigid.rotation.eulerAngles.x % 90.0f) == 0)
            {
                targetFinalRotation = Quaternion.identity;
                Debug.Log("DONE!");
                //RouletteDone(selectedIndex);
            }
            else
            {
                // Not aligned to the final spin down position, slerp to it
                targetFinalRotation = Quaternion.AngleAxis(selectedIndex * 90.0f, new Vector3(1, 0, 0));
            }
		}
		
	}
	
	void OnDisable()
	{
		m_myRigid.angularVelocity = Vector3.zero;
	}
	
	public void ChangeSpeed(bool a_slow, bool a_faster)
	{
		inputStop = a_slow;
		inputSpeedUp = a_faster;
	}
	
	void Spin()
	{
		m_myRigid.angularDrag = 0.001f;
		m_myRigid.AddRelativeTorque(Vector3.left * 100, ForceMode.Impulse);
	}

    /// <summary>
    /// Called when the roulette wheel finishes spinning.
    /// </summary>
    /// <param name="a_finalSelectionIndex">Index of the roulette wheel once the spin has finished.</param>
    void RouletteDone(int a_finalSelectionIndex)
    {
        // TODO Apply roulette reward buff here

        // Switch to the gameplay state
        gameObject.transform.parent.transform.GetComponentInParent<StateManager>().currentPlayerState = EPlayerState.Control;
    }
}
