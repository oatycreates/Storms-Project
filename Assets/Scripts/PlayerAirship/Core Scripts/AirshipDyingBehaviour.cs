using UnityEngine;
using System.Collections;

public class AirshipDyingBehaviour : MonoBehaviour // A simple script that sets the behaviour for the falling airship state. 
{
	private Rigidbody myRigid;
	public float fallAcceleration = 9.8f;
	
	public float timerUntilReset = 4.0f;	//How long should the player watch their ship falling until it resets and takes them to the Roulette screen - experiment with this.
	
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
		myRigid.useGravity = true;
		myRigid.AddForce(Vector3.down * 9.8f, ForceMode.Impulse);
		
		//change the camera behaviour;
		airshipMainCam.camFollowPlayer = false;
		
		//Time unil the player state resets
		timerUntilReset -= Time.deltaTime;
		
		if (timerUntilReset < 0.0f)
		{
			//reset the camera and change the play state
			airshipMainCam.camFollowPlayer = true;
			gameObject.GetComponent<StateManager>().currentPlayerState = PlayerState.Roulette;
		}
	}
}
