/**
 * File: ShuntingController.cs
 * Author: Andrew Barbour
 * Maintainer: Andrew Barbour
 * Created: 28/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Controls the shunting ability for player's ships movement
 **/

using UnityEngine;
using System.Collections;

public class ShuntingController : MonoBehaviour 
{
    public float shuntingForce  = 20.0f;
    [Tooltip("Cooldown until player is able to shunt again, in seconds")]
    public float cooldownTime   = 0.5f;

    private Rigidbody m_rigidBody;              // Reference to player's ship rigidbody
    private bool m_leftTriggerDown  = false;    // Will be true if left bumper on player's controller is held
    private bool m_rightTriggerDown = false;    // Will be true if right bumper on player's controller is held
    private bool m_shuntApplied     = false;    // Used to allow shunts on frame which a bumper is pressed, rather than held
    private float m_currentCoolTime = 0.0f;     // Timer until shunt can be used again

    /// <summary>
    /// Returns a quaternion which ignores the rotation on the Y axis
    /// </summary>
    private Quaternion rotationQuaternion
    {
        get
        {
            Vector3 rotationXZ = transform.rotation.eulerAngles;
            rotationXZ.y = 0.0f;
            Quaternion rotationQuat = Quaternion.Euler(rotationXZ);

            return rotationQuat;
        }
    }

    public void Awake()
    {
        m_rigidBody = GetComponent<Rigidbody>();
    }

	void Start() 
    {
	    
	}
	
	void Update() 
    {
        if (m_shuntApplied && !(m_leftTriggerDown || m_rightTriggerDown))
        {
            // Reset
            m_shuntApplied      = false;
            m_currentCoolTime   = 0.0f;
        }
        else
        {
            // Tick cooldown timer
            m_currentCoolTime += Time.deltaTime;
        }
	}

    public void FixedUpdate()
    {
        if (m_currentCoolTime <= cooldownTime)
        {
            return;
        }

        Quaternion rotationQuat = rotationQuaternion;

        Vector3 left    = rotationQuat * -transform.right;
        Vector3 right   = rotationQuat * transform.right;

        // Left shunt
        if (m_leftTriggerDown && !m_shuntApplied)
        {
            ApplyShunt(left);
        }

        // Right shunt
        if (m_rightTriggerDown && !m_shuntApplied)
        {
            ApplyShunt(right);
        }
    }

    void ApplyShunt(Vector3 a_direction)
    {
        m_rigidBody.AddForce(a_direction * shuntingForce, ForceMode.Impulse);
        m_shuntApplied = true;
    }

    public void PlayerInputs(bool a_leftBumper, bool a_rightBumper)
    {
        m_leftTriggerDown   = a_leftBumper;
        m_rightTriggerDown  = a_rightBumper;
    }
}
