using UnityEngine;
using System.Collections;

public class AirshipSuicideBehaviour : MonoBehaviour //This ship determines how the ship moves once the player has decided to "commit suicide"
{													// and play as a kamikazi 'Fire Ship'
													//Compared to regular movent, this time the airship controls like a missile - Player has less control.

	private Rigidbody myRigid;
	public GameObject fireShipParticles;
	
	public float timerUntilReset = 15.0f;	// How long untill the player defaults back to the roulette selection screen?
	
	//Less inputs than the standard airship controller
	public float pitchForce = 1000.0f;
	public float yawForce = 1000.0f;
	//Currently, there is no input for 'Roll' - maybe this needs to be added?
	
	[HideInInspector]
	public float pitch;
	[HideInInspector]
	public float yaw;
	
	public AirshipCamBehaviour airshipMainCam;
	
	void Awake()
	{
		myRigid = gameObject.GetComponent<Rigidbody>();
	}
	
	void Start () 
	{
		
	}
	
	void Update()
	{
		//set cam stuff
		airshipMainCam.camFollowPlayer = true;
	
		//turn on particles
		fireShipParticles.SetActive(true);
		
		myRigid.useGravity = false;		
		
		//Time unil the player state resets
		timerUntilReset -= Time.deltaTime;
		
		if (timerUntilReset < 0.0f)
		{
			gameObject.GetComponent<StateManager>().currentPlayerState = PlayerState.Roulette;
		}
	}
	
	
	void FixedUpdate()
	{
		Rocket();
		CalculateTorque();
	}
	
	public void PlayerFireshipInputs(float a_Vertical, float a_Horizontal)
	{
		pitch = a_Vertical;
		yaw = a_Horizontal;
		
		ClampInputs();	//got this trick from the standard vehicle assets
	}
	
	void ClampInputs()
	{
		pitch = Mathf.Clamp(pitch, -1, 1);
		yaw = Mathf.Clamp(yaw, -1, 1);
	
	}
	
	private void Rocket()	//got this working...
	{
		
		myRigid.AddRelativeForce(Vector3.forward * 250, ForceMode.Impulse);
		
		var liftDirection = Vector3.Cross(myRigid.velocity, myRigid.transform.right).normalized;	//This finds the 'up' vector.
		
		myRigid.AddForce(liftDirection);
	}
	
	
	private void CalculateTorque()	// also taken from Standard assets example
	{
		var torque = Vector3.zero;
		
		torque += -pitch * myRigid.transform.right * pitchForce;	
		
		torque += yaw * myRigid.transform.up * yawForce;
		//no roll here
		
		myRigid.AddTorque(torque);	//add all the torque forces together
	}
}
