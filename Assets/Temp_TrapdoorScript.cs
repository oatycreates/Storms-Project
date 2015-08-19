using UnityEngine;
using System.Collections;
//This temp script is used to trigger the trapdoor -- replace this with Animation when ready. 
//Most of this script was copied off the 'HingeJointScript'.
public class Temp_TrapdoorScript : MonoBehaviour 
{
	private float rotateAmount;
	public float maxRotationInDegrees;
	private float speed = 3.0f;
	
	public AirshipControlBehaviour controller;
	//Check the input manager for inputs.
	private bool buttonPressed = false;
	public bool flipDirection = false;
	
	private float turnValue;

    // Cached variables
    private Transform m_trans;

    void Awake()
    {
        m_trans = transform;
    }

	void Start () 
	{
		rotateAmount = 0;
	}
	
	void FixedUpdate () 
	{
		if (controller != null)
		{
			buttonPressed = controller.openHatch;
		}

		if (buttonPressed)
		{	
			if (!flipDirection)
			{
				rotateAmount = Mathf.Lerp(rotateAmount, maxRotationInDegrees, Time.deltaTime * speed);
			}
			else if (flipDirection)
			{
				rotateAmount = Mathf.Lerp(rotateAmount, -maxRotationInDegrees, Time.deltaTime * speed);
			}	
		}
		else if (!buttonPressed)
        {
            //Return to normal
			rotateAmount = Mathf.Lerp(rotateAmount, 0, Time.deltaTime * speed);
		}
		
		SetAxis();
	}
	
	void SetAxis()
	{
		float rotateX = gameObject.transform.localEulerAngles.x;
		float rotateY = gameObject.transform.localEulerAngles.y;
		//float rotateZ = gameObject.transform.localEulerAngles.z;

        m_trans.localRotation = Quaternion.Euler(rotateX, rotateY, rotateAmount);
	}
	
}
