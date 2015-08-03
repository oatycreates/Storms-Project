using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Rigidbody))]
public class RouletteBehaviour : MonoBehaviour 	//This script manages everything in the 'Roulette' / 'Lucky Dip' state. 
												// This only TRIGGERS the Roulette Spin. 
												//See Script: "RouletteSpinWheel" under Effects Assets for roulette functions.
												
												//The roulette state DOESN'T naturally 'Time-Out'. Maybe we should include this?
{
	private Rigidbody myRigid;
	
	public GameObject fireParticleEffect;
	
	public AirshipCamBehaviour airshipMainCam;
	
	public RouletteSpinWheel spinWheel;

	void Awake()
	{
		myRigid = gameObject.GetComponent<Rigidbody>();
	}

	
	void Update()
	{
		//set cam stuff
		airshipMainCam.camFollowPlayer = true;
	
		//turn particles off here - it's a good place to reset
		fireParticleEffect.SetActive(false);
		
		//remove the momentum of the player
		myRigid.velocity = Vector3.zero;
		myRigid.angularVelocity = Vector3.zero;
		
		//Reset the values on the other scripts- this way, they'll be ready the next time we need them
		gameObject.GetComponent<AirshipSuicideBehaviour>().timerUntilReset = 15.0f;
		gameObject.GetComponent<AirshipDyingBehaviour>().timerUntilReset = 4.0f;
		
	}
	
	public void PlayerInput(bool a_stopWheel, bool a_SpinFaster)
	{	
		//Pass these values directly into the spinwheel script
		if (spinWheel != null)	
		{
			spinWheel.ChangeSpeed(a_stopWheel, a_SpinFaster);
		}
		else
		{
			Debug.LogWarning("No Reference to the Wooden Spin Wheel");
		}
	}
	
	public void ResetPosition(Vector3 a_pos, Quaternion a_rot)	//called by state manager
	{
		gameObject.transform.position = a_pos; //(world pos & rotation)
		gameObject.transform.rotation = a_rot;
		
		airshipMainCam.RouletteCam();	// reset the cam position as well!
	}
}
