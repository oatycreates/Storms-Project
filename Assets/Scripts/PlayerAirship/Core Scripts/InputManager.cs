using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour 	//Just the raw controller inputs. These are mainly passed into the airship movement controls, but also effect suicide/fireship and roulette controls.
{
	private AirshipControlBehaviour standardControl;
	private AirshipSuicideBehaviour fireshipControl;
	private RouletteBehaviour 		rouletteControl;
	//We might need to add more script references here as we progress.
	
	void Start()
	{
		standardControl = gameObject.GetComponent<AirshipControlBehaviour>();
		fireshipControl = gameObject.GetComponent<AirshipSuicideBehaviour>();
		rouletteControl = gameObject.GetComponent<RouletteBehaviour>();
	}

	//This input stuff was all figured out in an old script called 'TempDebugScript' 
	
	//It's clever, because it determines which input to look for based off the player tag.
	
	void FixedUpdate () 
	{
	
	//Axis Input
	
		//Left Stick Input													//One Stick to Determine Movement
		float upDown = Input.GetAxis(gameObject.tag + "Vertical");
		float leftRight = Input.GetAxis(gameObject.tag + "Horizontal");
		
		//Right Stick Input - probably for Camera Control
		float camUpDown = Input.GetAxis(gameObject.tag + "CamVertical");			//At this point, we expect the Right Analogue Stick will be used for Camera Control
		float camLeftRight = Input.GetAxis(gameObject.tag + "CamHorizontal");
		
		//Trigger Input -- for acceleration
		float triggers = -Input.GetAxis(gameObject.tag + "Triggers");
		
		//DPad Input - for menus and such
		float dPadUpDown = -Input.GetAxis(gameObject.tag + "DPadVertical");
		float dPadLeftRight = Input.GetAxis(gameObject.tag + "DPadHorizontal");
		
		
	//Button Input
		//Bumpers
		bool bumperLeft = Input.GetButton(gameObject.tag + "BumperLeft");
		bool bumperRight = Input.GetButton(gameObject.tag + "BumperRight");
		
		//Face Buttons
		bool faceDown = Input.GetButton(gameObject.tag + "FaceDown");
		bool faceLeft = Input.GetButton(gameObject.tag + "FaceLeft");
		bool faceRight = Input.GetButton(gameObject.tag + "FaceRight");
		bool faceUp = Input.GetButton(gameObject.tag + "FaceUp");
		
		//Start and Select
		bool select = Input.GetButton(gameObject.tag + "Select");
		bool start = Input.GetButton(gameObject.tag + "Start");
		
		//Analogue Stick Clicks
		bool clickLeft = Input.GetButton(gameObject.tag + "ClickLeft");
		bool clickRight = Input.GetButton(gameObject.tag + "ClickRight");
		
		
	//Send variable data to individual scripts
		rouletteControl.PlayerInput(faceDown, faceUp);	//use the face button inputs to Stop/Start the roulette wheel
		standardControl.PlayerInputs(upDown, leftRight, camUpDown, camLeftRight, triggers);
		fireshipControl.PlayerFireshipInputs(upDown, leftRight);
	}
}
