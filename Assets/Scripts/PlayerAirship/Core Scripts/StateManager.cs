/**
 * File: RoulletteBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Gives the player something to do when they die. A catch-up mechanic ala Super Mario Kart.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// The state that the local player is currently in.
/// </summary>
public enum EPlayerState
{
    Roulette, 
    Control, 
    Dying, 
    Suicide
};

/// <summary>
/// This script organises all the different 'States' the player can be in. If we need to add more States, make sure to do them here.
/// The State Manager will automatically add these 6 scripts - they are Vital to how the airship works.
/// </summary>
[RequireComponent(typeof(RouletteBehaviour))]
[RequireComponent(typeof(AirshipControlBehaviour))]
[RequireComponent(typeof(AirshipDyingBehaviour))]
[RequireComponent(typeof(AirshipSuicideBehaviour))]
[RequireComponent(typeof(InputManager))]
[RequireComponent(typeof(TagChildren))]
public class StateManager : MonoBehaviour
{
	public EPlayerState currentPlayerState;
	
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
	private Vector3 m_worldStartPos;
	private Quaternion m_worldStartRotation;
	

	void Start () 
	{
		currentPlayerState = EPlayerState.Roulette;
		
		rouletteScript = gameObject.GetComponent<RouletteBehaviour>();
		airshipScript = gameObject.GetComponent<AirshipControlBehaviour>();
		dyingScript = gameObject.GetComponent<AirshipDyingBehaviour>();
		suicideScript = gameObject.GetComponent<AirshipSuicideBehaviour>();
		
		inputManager = gameObject.GetComponent<InputManager>();
		
		// World position & rotation
		m_worldStartPos = gameObject.transform.position;
		m_worldStartRotation = gameObject.transform.rotation;
	}
	
	
	void Update () 
	{
        // Hehehe
		if (Application.isEditor == true)
		{
			DevHacks();
		}

       // The player airship is not being used while the roulette wheel is spinning. (Airship is deactivated).
        if (currentPlayerState == EPlayerState.Roulette)
		{
			// Roulette control
			rouletteScript.enabled = true;
			// Reset position
			rouletteScript.ResetPosition(m_worldStartPos, m_worldStartRotation);
			airshipScript.enabled = false;
			dyingScript.enabled = false;
			suicideScript.enabled = false;
			
			// We don't need to see the airship during the roulette wheel
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

        // Standard player airship control
		if (currentPlayerState == EPlayerState.Control)
		{
			// Standard Physics Control
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

        // Player has no-control over airship, but it's still affected by forces. Gravity is making the airship fall
		if (currentPlayerState == EPlayerState.Dying)
		{
			// No Control, gravity makes airship fall
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

        // Recent addition- this is for the fireship/suicide function - the player has limited control here, needs further experimentation
		if (currentPlayerState == EPlayerState.Suicide)
		{
			// Airship behaves like a rocket
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
	
    /// <summary>
    /// Skip to next EPlayerState.
    /// If we add more states, make sure we add functionality here.
    /// </summary>
	void DevHacks()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			currentPlayerState = EPlayerState.Roulette;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			currentPlayerState = EPlayerState.Control;
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			currentPlayerState = EPlayerState.Dying;	
		}
		
		if (Input.GetKeyDown(KeyCode.Alpha4))
		{
			currentPlayerState = EPlayerState.Suicide;
		}
		
		//inputs
		if (Input.GetButtonDown(gameObject.tag + "Select"))
		{
			if (currentPlayerState == EPlayerState.Roulette)
			{
				currentPlayerState = EPlayerState.Control;
			}
			else
			if (currentPlayerState == EPlayerState.Control)
			{
				currentPlayerState = EPlayerState.Dying;
			}
			else
			if (currentPlayerState == EPlayerState.Dying)
			{
				currentPlayerState = EPlayerState.Suicide;
			}
			else
			if (currentPlayerState == EPlayerState.Suicide)
			{
				currentPlayerState = EPlayerState.Roulette;
			}
		}
		
		if (Input.GetButtonDown(gameObject.tag + "Start"))
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
	}
}
