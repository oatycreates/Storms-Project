using UnityEngine;
using System.Collections;

public class AirshipControlBehaviour : MonoBehaviour 	// This is the basic airship control. In time this script will probably be changed a lot.
														// Many of the movement features here were expanded upon and derived from the Unity Standard Vehicle Assets - Plane Controller
														// Unlike the old Storms Project, this game moves Non-Kinematic Rigidbodies through Physics (unlike Kinematic Rigidbody via direct control).
{

	private Rigidbody myRigid;
	
	public float generalSpeed = 50.0f; // try 50 here and see how we go
	
	public float pitchForce = 1000.0f;
	public float yawForce = 1000.0f;
	public float rollForce = 1000.0f; 	// These can be changed in editor.
	
	
	[HideInInspector]
	public float roll;
	[HideInInspector]
	public float pitch;
	[HideInInspector]
	public float yaw;
	[HideInInspector]
	public float throttle;
	
	public AirshipCamBehaviour airshipMainCam;
	
	
	void Awake()
	{
		myRigid = gameObject.GetComponent<Rigidbody>();
	}
	
	void Start () 
	{
		myRigid.velocity = Vector3.zero;
		myRigid.angularVelocity = Vector3.zero;		//not sure if this works__ does 'Start' get called when the script is Activated?
	}
	
	void Update()
	{
		//set cam stuff
		airshipMainCam.camFollowPlayer = true;
	
		//set rigidbody basics
		myRigid.useGravity = false;
		myRigid.isKinematic = false;	//We want physics collisions!
		
		myRigid.mass = 10.0f;
		myRigid.drag = 2.0f;
		myRigid.angularDrag = 2.0f;
	
	}
	
	
	public void PlayerInputs(float a_Vertical, float a_Horizontal, float a_camVertical, float a_camHorizontal, float a_triggers)
	{
		roll = a_Horizontal;
		pitch = a_Vertical;
		yaw = a_Horizontal;
		throttle = a_triggers;
		
		ClampInputs();		// learnt this from standard asset examples
	}
	
	void FixedUpdate()		//Doing this here makes it easier on the physics
	{
		AutoLevel();
		ConstantForwardMovement();
		CalculateTorque();
	}
	
	
	private void ClampInputs()
	{
		roll = Mathf.Clamp(roll, -1, 1);
		pitch = Mathf.Clamp(pitch, -1, 1);
		yaw = Mathf.Clamp(yaw, -1, 1);
		throttle = Mathf.Clamp(throttle, -1, 1);
	}

	private void AutoLevel()
	{
		//I've left this blank for the time being... 
		//Maybe check over standard assets to see how they AUTO LEVEL
	}

	private void ConstantForwardMovement()	//got this working...
	{
		// 				general speed 	half or double general speed	+ 25% of general speed == Always a positive value
		float speedMod = generalSpeed + (throttle * generalSpeed) + (generalSpeed/4);
	
		myRigid.AddRelativeForce(Vector3.forward * speedMod , ForceMode.Acceleration);
		
		var liftDirection = Vector3.Cross(myRigid.velocity, myRigid.transform.right).normalized;	//This finds the 'up' vector. // This was a cool trick from The Standard Vehicle Assets
		
		myRigid.AddForce(liftDirection);
	}

	
	private void CalculateTorque()	// also taken from Standard assets example
	{
		var torque = Vector3.zero;
		
		torque += -pitch * myRigid.transform.right * pitchForce;	
		
		torque += yaw * myRigid.transform.up * yawForce;
		
		torque += -roll * myRigid.transform.forward * rollForce;
		
		myRigid.AddTorque(torque);	//add all the torque forces together
	}
	
	
	
}
