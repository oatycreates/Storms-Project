using UnityEngine;
using System.Collections;

public enum PlayerState {Roulette, Control, Dying, Suicide}

														//The State Manager will automatically add these 6 scripts - they are Vital to how the airship works
[RequireComponent(typeof(RouletteBehaviour))]
[RequireComponent(typeof(AirshipControlBehaviour))]
[RequireComponent(typeof(AirshipDyingBehaviour))]
[RequireComponent(typeof(AirshipSuicideBehaviour))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(TagChildren))]
public class StateManager : MonoBehaviour 	//This script organises all the different 'States' the player can be in. //If we need to add more States, make sure to do them here
{
	public PlayerState currentPlayerState;
	
	//References to all the different scripts
	private RouletteBehaviour rouletteScript;
	private AirshipControlBehaviour airshipScript;
	private AirshipDyingBehaviour dyingScript;
	private AirshipSuicideBehaviour suicideScript;
	
	private InputManager inputManager;	//Good to make sure the airship HAS an input manager.
	
	//References to the different components on the Airship
	public GameObject colliders;
	public GameObject meshes;
	public GameObject rouletteHierachy;
	
	//Remember the start position and rotation in world space, so we can return here when the player has died.
	private Vector3 worldStartPos;
	private Quaternion worldStartRotation;
	

	void Start () 
	{
		currentPlayerState = PlayerState.Roulette;
		
		rouletteScript = gameObject.GetComponent<RouletteBehaviour>();
		airshipScript = gameObject.GetComponent<AirshipControlBehaviour>();
		dyingScript = gameObject.GetComponent<AirshipDyingBehaviour>();
		suicideScript = gameObject.GetComponent<AirshipSuicideBehaviour>();
		
		inputManager = gameObject.GetComponent<InputManager>();
		
		//world position
		worldStartPos = gameObject.transform.position;
		worldStartRotation = gameObject.transform.rotation;
	}
	
	
	void Update () 
	{
		if (Application.isEditor == true)	//hehehe
		{
			DevHacks();
		}
	
		if (currentPlayerState == PlayerState.Roulette)		//The player airship is not being used while the roulette wheel is spinning. (Airship is deactivated).
		{
			//Roulette control
			rouletteScript.enabled = true;
			//reset position
			rouletteScript.ResetPosition(worldStartPos, worldStartRotation);
			airshipScript.enabled = false;
			dyingScript.enabled = false;
			suicideScript.enabled = false;
			
			//We don't need to see the airship during the roulette wheel
			if (colliders != null)
			{
				colliders.SetActive(false);
			}
			
			if (meshes != null)
			{
				meshes.SetActive(false);
			}
			
			if (rouletteHierachy != null)
			{
				rouletteHierachy.SetActive(true);
			}
		}
		
		
		if (currentPlayerState == PlayerState.Control)	//Standard player airship control
		{
			//Standard Physics Control
			rouletteScript.enabled = false;
			airshipScript.enabled = true;
			dyingScript.enabled = false;
			suicideScript.enabled = false;
			
			if (colliders != null)
			{
				colliders.SetActive(true);
			}
			
			if (meshes != null)
			{
				meshes.SetActive(true);
			}
			
			if (rouletteHierachy != null)
			{
				rouletteHierachy.SetActive(false);
			}
		}
		
		
		if (currentPlayerState == PlayerState.Dying)	// Player has no-control over airship, but it's still affected by forces. Gravity is making the airship fall.
		{
			//No Control, gravity makes airship fall
			rouletteScript.enabled = false;
			airshipScript.enabled = false;
			dyingScript.enabled = true;
			suicideScript.enabled = false;
			
			
			if (colliders != null)
			{
				colliders.SetActive(true);
			}
			
			if (meshes != null)
			{
				meshes.SetActive(true);
			}
			
			if (rouletteHierachy != null)
			{
				rouletteHierachy.SetActive(false);
			}
		}
		
		
		if (currentPlayerState == PlayerState.Suicide) 	// Recent addition- this is for the fireship/suicide function - the player has limited control here. //Needs further experimentation.
		{
			////Airship behaves like a rocket
			rouletteScript.enabled = false;
			airshipScript.enabled = false;
			dyingScript.enabled = false;
			suicideScript.enabled = true;
			
			
			if (colliders != null)
			{
				colliders.SetActive(true);
			}
			
			if (meshes != null)
			{
				meshes.SetActive(true);
			}
			
			if (rouletteHierachy != null)
			{
				rouletteHierachy.SetActive(false);
			}
		}
		
	}
	
	void DevHacks()	//skip to next PlayerState	//If we add more states, make sure we add functionality here.
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			currentPlayerState = PlayerState.Roulette;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			currentPlayerState = PlayerState.Control;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			currentPlayerState = PlayerState.Dying;	
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			currentPlayerState = PlayerState.Suicide;
		}
		
		//inputs
		
		
		if (Input.GetButtonDown(gameObject.tag + "Select"))
		{
			if (currentPlayerState == PlayerState.Roulette)
			{
				currentPlayerState = PlayerState.Control;
			}
			else
			if (currentPlayerState == PlayerState.Control)
			{
				currentPlayerState = PlayerState.Dying;
			}
			else
			if (currentPlayerState == PlayerState.Dying)
			{
				currentPlayerState = PlayerState.Suicide;
			}
			else
			if (currentPlayerState == PlayerState.Suicide)
			{
				currentPlayerState = PlayerState.Roulette;
			}
		}
		
		if (Input.GetButtonDown(gameObject.tag + "Start"))
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
	}
}
