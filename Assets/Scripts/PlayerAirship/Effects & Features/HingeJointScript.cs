/**
 * File: PrisonFortressKlaxonWarning.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Make a custom 'hinge' rotation controlled through the Airship's movement.
 **/

using UnityEngine;
using System.Collections;

public enum EHingeAxis
{
    hingeX, 
    hingeY, 
    hingeZ
}

/// <summary>
/// Make a custom 'hinge' rotation controlled through the Airship's movement.
/// </summary>
public class HingeJointScript : MonoBehaviour 
{
	public EHingeAxis axis;

	private float m_rotateAmount;
	public float maxRotationDegrees;
	private float m_absRotateAmount;
	private float m_speed = 1.0f;

	public AirshipControlBehaviour airshipControlBehaviour;
	/// <summary>
    /// The hiddenValues is taken from the Airship control behaviour.
	/// </summary>
	private float m_hiddenRoll;
	private float m_hiddenYaw;
	private float m_hiddenPitch;

	/// <summary>
    /// This is a generic value. The axis is determined later.
	/// </summary>
	private float m_turnValue;

	void Start () 
	{
		m_rotateAmount = 0;
		//maxRotationDegrees = 60.0f;
	}

	void FixedUpdate () 
	{
		// Get values from ship movement
		if (airshipControlBehaviour != null)
		{
			m_hiddenRoll = airshipControlBehaviour.roll;
			m_hiddenYaw = airshipControlBehaviour.yaw;
			m_hiddenPitch = airshipControlBehaviour.pitch;

		}
		else
		{
			Debug.Log("No Airship Script Attached");
		}

		// Run this here, so we know which axis we're working with
		GetAxis();

		if (m_turnValue > 0)
		{
			m_rotateAmount = Mathf.Lerp(m_rotateAmount, -m_absRotateAmount, Time.deltaTime* m_speed);
		}
		else if (m_turnValue < 0)
		{
			m_rotateAmount = Mathf.Lerp(m_rotateAmount, m_absRotateAmount, Time.deltaTime* m_speed);
		}
		else if (Mathf.Approximately(m_turnValue, 0))
		{
            // Return to normal
			m_rotateAmount = Mathf.Lerp(m_rotateAmount, 0, Time.deltaTime* m_speed);
		}
	
		SetAxis();
	}

	/// <summary>
    /// Checking which value we should be looking at.
	/// </summary>
	void GetAxis()
	{
		if (axis == EHingeAxis.hingeX)
		{
			m_turnValue = m_hiddenPitch;

			// Clamp the rotation
			m_absRotateAmount = Mathf.Abs (maxRotationDegrees * m_hiddenPitch);
			// This is more sophisticated since the old prototype script
		}
		else if (axis == EHingeAxis.hingeY)
		{
			m_turnValue = m_hiddenYaw;

			// Clamp the rotation
			m_absRotateAmount = Mathf.Abs (maxRotationDegrees * m_hiddenYaw);
		}
		else if (axis == EHingeAxis.hingeZ)
		{
			m_turnValue = m_hiddenRoll;

			// Clamp the rotation
			m_absRotateAmount = Mathf.Abs (maxRotationDegrees * m_hiddenRoll);
		}
	}

	/// <summary>
    /// Update the game object rotation based on axis.
	/// </summary>
	void SetAxis()
	{
		// Apply the rotateamount to force
		float rotateX = gameObject.transform.localEulerAngles.x;
		float rotateY = gameObject.transform.localEulerAngles.y;
		float rotateZ = gameObject.transform.localEulerAngles.z;


		if (axis == EHingeAxis.hingeX)
		{
			gameObject.transform.localRotation = Quaternion.Euler(m_rotateAmount,rotateY, rotateZ);
		}
		else if (axis == EHingeAxis.hingeY)
		{
			gameObject.transform.localRotation = Quaternion.Euler(rotateX, m_rotateAmount, rotateZ);
		}
		else if (axis == EHingeAxis.hingeZ)
		{
			gameObject.transform.localRotation = Quaternion.Euler(rotateX, rotateY, m_rotateAmount);
		}
	}
}
