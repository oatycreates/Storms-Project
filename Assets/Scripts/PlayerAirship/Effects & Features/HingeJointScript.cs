using UnityEngine;
using System.Collections;

public enum HingeAxis{hingeX, hingeY, hingeZ}

//Make a custom 'hinge' rotation controlled through the Airship's movement.
public class HingeJointScript : MonoBehaviour 
{
	public HingeAxis axis;

	private float rotateAmount;
	public float maxRotationDegrees;
	private float absRotateAmount;
	private float speed = 1.0f;

	public AirshipControlBehaviour airshipControlBehaviour;
	//The hiddenValues is taken from the Airship control behaviour.
	private float hiddenRoll;
	private float hiddenYaw;
	private float hiddenPitch;

	//This is a generic value. The axis is determined later
	private float turnValue;



	void Start () 
	{
		rotateAmount = 0;
		maxRotationDegrees = 60.0f;
	}

	void FixedUpdate () 
	{
		// Get values from ship movement.
		if (airshipControlBehaviour != null)
		{
			hiddenRoll = airshipControlBehaviour.roll;
			hiddenYaw = airshipControlBehaviour.yaw;
			hiddenPitch = airshipControlBehaviour.pitch;

		}
		else
		{
			Debug.Log("No Airship Script Attached");
		}

		//Run this here, so we know which axis we're working with.
		GetAxis ();


		if (turnValue > 0)
		{
			rotateAmount = Mathf.Lerp(rotateAmount, -absRotateAmount, Time.deltaTime* speed);
		}
		else
		if (turnValue < 0)
		{
			rotateAmount = Mathf.Lerp(rotateAmount, absRotateAmount, Time.deltaTime* speed);
		}
		else
		//Return to normal
		if (turnValue == 0)
		{
			rotateAmount = Mathf.Lerp(rotateAmount, 0, Time.deltaTime* speed);
		}
	

		SetAxis ();

	}

	//Checking which value we should be looking at
	void GetAxis()
	{
		if (axis == HingeAxis.hingeX)
		{
			turnValue = hiddenRoll;

			//Clamp the rotation
			absRotateAmount = Mathf.Abs (maxRotationDegrees * hiddenRoll);
			//This is more sophisticated since the old prototype script.
		}
		else
		if (axis == HingeAxis.hingeY)
		{
			turnValue = hiddenYaw;

			//Clamp the rotation
			absRotateAmount = Mathf.Abs (maxRotationDegrees * hiddenYaw);
		}
		else
		if (axis == HingeAxis.hingeZ)
		{
			turnValue = hiddenPitch;

			//Clamp the rotation
			absRotateAmount = Mathf.Abs (maxRotationDegrees * hiddenPitch);
		}
	}


	//Update the game object rotation based on axis.
	void SetAxis()
	{

		//Apply the rotateamount to force
		
		float rotateX = gameObject.transform.localEulerAngles.x;
		float rotateY = gameObject.transform.localEulerAngles.y;
		float rotateZ = gameObject.transform.localEulerAngles.z;


		if (axis == HingeAxis.hingeX)
		{
			gameObject.transform.localRotation = Quaternion.Euler (rotateAmount,rotateY, rotateZ);
		}
		else		
		if (axis == HingeAxis.hingeY)
		{
			gameObject.transform.localRotation = Quaternion.Euler (rotateX, rotateAmount, rotateZ);
		}
		else
		if (axis == HingeAxis.hingeZ)
		{
			gameObject.transform.localRotation = Quaternion.Euler (rotateX, rotateY, rotateAmount);
		}
	}
}
