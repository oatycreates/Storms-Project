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
	private RouletteBehaviour m_rouletteScript;
	private AirshipControlBehaviour m_airshipScript;
	private AirshipDyingBehaviour m_dyingScript;
	private AirshipSuicideBehaviour m_suicideScript;
	
    /// <summary>
    /// Good to make sure the airship HAS an input manager.
    /// </summary>
	private InputManager m_inputManager;
	
	// References to the different components on the airship
	public GameObject colliders;
	public GameObject meshes;
	public GameObject hinges;
	public GameObject rouletteHierachy;
	public GameObject particlesEffectsHierachy;
	
	/// <summary>
    /// Remember the start position and rotation in world space, so we can return here when the player has died.
	/// </summary>
	private Vector3 m_worldStartPos;
	private Quaternion m_worldStartRotation;
	

	void Start () 
	{
		currentPlayerState = EPlayerState.Roulette;
		
		m_rouletteScript = gameObject.GetComponent<RouletteBehaviour>();
		m_airshipScript = gameObject.GetComponent<AirshipControlBehaviour>();
		m_dyingScript = gameObject.GetComponent<AirshipDyingBehaviour>();
		m_suicideScript = gameObject.GetComponent<AirshipSuicideBehaviour>();
		
		m_inputManager = gameObject.GetComponent<InputManager>();
		
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
			m_rouletteScript.enabled = true;
			// Reset position
			m_rouletteScript.ResetPosition(m_worldStartPos, m_worldStartRotation);
			m_airshipScript.enabled = false;
			m_dyingScript.enabled = false;
			m_suicideScript.enabled = false;
			
			// We don't need to see the airship during the roulette wheel
			if (colliders != null)
			{
				colliders.SetActive(false);
			}
			
			if (meshes != null)
			{
				meshes.SetActive(false);
			}

			if (hinges != null)
			{
				hinges.SetActive(false);
			}

			if (rouletteHierachy != null)
			{
				rouletteHierachy.SetActive(true);
			}
			
			if (particlesEffectsHierachy != null)
			{
				particlesEffectsHierachy.SetActive(false);
			}
		}

        // Standard player airship control
		if (currentPlayerState == EPlayerState.Control)
		{
			// Standard Physics Control
			m_rouletteScript.enabled = false;
			m_airshipScript.enabled = true;
			m_dyingScript.enabled = false;
			m_suicideScript.enabled = false;
			
			if (colliders != null)
			{
				colliders.SetActive(true);
			}
			
			if (meshes != null)
			{
				meshes.SetActive(true);
			}

			if (hinges != null)
			{
				hinges.SetActive(true);
			}
			
			if (rouletteHierachy != null)
			{
				rouletteHierachy.SetActive(false);
			}
			
			if (particlesEffectsHierachy != null)
			{
				particlesEffectsHierachy.SetActive(true);
			}
		}

        // Player has no-control over airship, but it's still affected by forces. Gravity is making the airship fall
		if (currentPlayerState == EPlayerState.Dying)
		{
			// No Control, gravity makes airship fall
			m_rouletteScript.enabled = false;
			m_airshipScript.enabled = false;
			m_dyingScript.enabled = true;
			m_suicideScript.enabled = false;
			
			
			if (colliders != null)
			{
				colliders.SetActive(true);
			}
			
			if (meshes != null)
			{
				meshes.SetActive(true);
			}

			if (hinges != null)
			{
				hinges.SetActive(true);
			}
			
			if (rouletteHierachy != null)
			{
				rouletteHierachy.SetActive(false);
			}
			
			if (particlesEffectsHierachy != null)
			{
				particlesEffectsHierachy.SetActive(true);
			}
		}

        // Recent addition- this is for the fireship/suicide function - the player has limited control here, needs further experimentation
		if (currentPlayerState == EPlayerState.Suicide)
		{
			// Airship behaves like a rocket
			m_rouletteScript.enabled = false;
			m_airshipScript.enabled = false;
			m_dyingScript.enabled = false;
			m_suicideScript.enabled = true;
			
			if (colliders != null)
			{
				colliders.SetActive(true);
			}
			
			if (meshes != null)
			{
				meshes.SetActive(true);
			}

			if (hinges != null)
			{
				hinges.SetActive(true);
			}
			
			if (rouletteHierachy != null)
			{
				rouletteHierachy.SetActive(false);
			}
			
			if (particlesEffectsHierachy != null)
			{
				particlesEffectsHierachy.SetActive(true);
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
