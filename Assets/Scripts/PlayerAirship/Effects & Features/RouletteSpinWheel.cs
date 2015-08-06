using UnityEngine;
using System.Collections;

public class RouletteSpinWheel : MonoBehaviour // This script manages the 'Spinning Wheel' effect on the Roulette Wheel
												// Inputs are passed in from Input Manager script VIA the Roulette Behaviour State script.
{
	private Rigidbody m_myRigid;
	private float changeAngularDrag;
	
	public GameObject rotatorJoint;
	private bool pullHandle = false;
	private float rotateAmount;
	
	private bool inputStop;
	private bool inputSpeedUp;
	

	void OnEnable () 
	{

		//Reset Position
		gameObject.transform.localPosition = Vector3.zero;
		gameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
		gameObject.transform.localScale = new Vector3(4, 1, 1);	//try not to change these
	
		//Set/Reset physics components
		m_myRigid = gameObject.GetComponent<Rigidbody>();
		//Set Components of the rigidbody in the start.
		
		m_myRigid.isKinematic = false;
		m_myRigid.useGravity = false;
		
		m_myRigid.drag = 0;
		m_myRigid.angularDrag = 0.001f;	//low on start
		
		m_myRigid.maxAngularVelocity = 14;	//default 7
		
		//get the startRotation of the handle joint
		rotateAmount = 0;
		
		//Make the wheel spin
		Spin();
	}
	
	void Update () 
	{
		if (inputSpeedUp)	// For testing- make the roulette wheel spin faster
		{
			Spin();
		}
		
		if (inputStop)		// Slow the roulette wheel by increasing the rigidbody Drag
		{
			m_myRigid.angularDrag = Mathf.Lerp(m_myRigid.angularDrag, 1, 0.25f);	
			
			pullHandle = true;
		}
		
		
		if (pullHandle)	//Handle effect
		{
			rotateAmount = Mathf.Lerp(rotateAmount, -90, Time.deltaTime * 5.0f);
		}
		
		if (rotatorJoint.transform.localEulerAngles.x == 270)	// Make handle return to original position
		{
			pullHandle = false;
		}
		
		// Make handle return to original position
		if ((!pullHandle) && (!inputStop))
		{
			rotateAmount = Mathf.Lerp(rotateAmount, 0, Time.deltaTime * 10.0f);
		}
		
		//Lock the rotation of the handle to One Axis
		float rotationY = rotatorJoint.transform.localEulerAngles.y;
		float rotationZ = rotatorJoint.transform.localEulerAngles.z;
		//move the joint
		rotatorJoint.transform.localRotation = Quaternion.Euler(rotateAmount, rotationY, rotationZ);
		
		// Use this space to check the Result of the roulettewheel
		
		//NOTE: Right now- this just triggers the State Manager to change the object from ROULETE to NORMAL CONTROL
		if (m_myRigid.angularVelocity.x > -1.0f && m_myRigid.angularVelocity.x < 0)
		{
			gameObject.transform.parent.transform.GetComponentInParent<StateManager>().currentEPlayerState = EPlayerState.Control;
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
}
