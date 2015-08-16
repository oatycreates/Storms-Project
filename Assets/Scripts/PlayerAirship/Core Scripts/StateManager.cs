/**
 * File: RoulletteBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script organises all the different 'states' the player can be in. If we need to add more states, make sure to do them here.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// The state that the local player is currently in.
/// </summary>
public enum EPlayerState
{
    Pregame, 
    Roulette, 
    Control, 
    Dying, 
    Suicide
};

/// <summary>
/// This script organises all the different 'States' the player can be in. If we need to add more States, make sure to do them here.
/// The State Manager will automatically add these 6 scripts - they are vital to how the airship works.
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
	public GameObject weaponsHierachy;
	
	/// <summary>
    /// Remember the start position and rotation in world space, so we can return here when the player has died.
	/// </summary>
	private Vector3 m_worldStartPos;
	private Quaternion m_worldStartRotation;
	
	private float stallCommit = 1.0f;
	private bool escapeStall = false;
	
	[HideInInspector]
	public float timeBetweenStall = 5.0f;
	

	void Start () 
	{
		//currentPlayerState = EPlayerState.Roulette;
        currentPlayerState = EPlayerState.Pregame;
		
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
		//if (Application.isEditor == true)
		//{
			DevHacks();
		//}
		
		// Make the player commit to the STALL
		if (currentPlayerState == EPlayerState.Dying)
		{
			stallCommit -= Time.deltaTime;
		}
		
		if (stallCommit < 0)
		{
			escapeStall = true;
		}
		else
		{
			escapeStall = false;
		}
		
		// Anti-spam function for stall
		timeBetweenStall -= Time.deltaTime;

        // Before the round begins, have a 'press any button to join' prompt
        if (currentPlayerState == EPlayerState.Pregame)
        {
            PregameUpdate();
        }
		
        // The player airship is not being used while the roulette wheel is spinning, the airship is deactivated
        if (currentPlayerState == EPlayerState.Roulette)
		{
            RouletteUpdate();
		}

        // Standard player airship control
		if (currentPlayerState == EPlayerState.Control)
		{
            ControlUpdate();
		}

        // Player has no-control over airship, but it's still affected by forces. Gravity is making the airship fall
		if (currentPlayerState == EPlayerState.Dying)
		{
            DyingUpdate();
		}

        // Recent addition- this is for the fireship/suicide function - the player has limited control here, needs further experimentation
		if (currentPlayerState == EPlayerState.Suicide)
		{
            SuicideUpdate();
		}

	}

    private void PregameUpdate()
    {
        if (GetAnyButtonDownMyPlayer())
        {
            // Switch to the next state
            currentPlayerState = EPlayerState.Control;
        }
        else
        {
            // Continue the waiting state

            m_rouletteScript.enabled = false;
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
                particlesEffectsHierachy.SetActive(false);
            }

            if (weaponsHierachy != null)
            {
                weaponsHierachy.SetActive(false);
            }
        }
    }

    private bool GetAnyButtonDownMyPlayer()
    {
        string cntPrefix = gameObject.tag;

        // Check if any button is down
        if (Input.GetButton(cntPrefix + "Start")        ||
            Input.GetButton(cntPrefix + "Select")       || 
            Input.GetButton(cntPrefix + "FaceDown")     ||
            Input.GetButton(cntPrefix + "FaceUp")       ||
            Input.GetButton(cntPrefix + "FaceLeft")     ||
            Input.GetButton(cntPrefix + "FaceRight")    || 
            Input.GetButton(cntPrefix + "BumperLeft")   ||
            Input.GetButton(cntPrefix + "BumperRight")  ||
            Input.GetButton(cntPrefix + "Triggers")     ||
            Input.GetButton(cntPrefix + "ClickLeft")    ||
            Input.GetButton(cntPrefix + "ClickRight")   ||
            Input.GetAxisRaw(cntPrefix + "Horizontal")      != 0 ||
            Input.GetAxisRaw(cntPrefix + "Vertical")        != 0 ||
            Input.GetAxisRaw(cntPrefix + "CamHorizontal")   != 0 ||
            Input.GetAxisRaw(cntPrefix + "CamVertical")     != 0)
        {
            return true;
        }

        return false;
    }

    private void SuicideUpdate()
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

        if (weaponsHierachy != null)
        {
            weaponsHierachy.SetActive(false);
        }
    }

    private void DyingUpdate()
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

        if (weaponsHierachy != null)
        {
            weaponsHierachy.SetActive(false);
        }
    }

    private void ControlUpdate()
    {
        // Standard Physics Control
        m_rouletteScript.enabled = false;
        m_airshipScript.enabled = true;
        //m_dyingScript.ResetTimer();
        m_dyingScript.enabled = false;

        m_suicideScript.ResetTimer();
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

        if (weaponsHierachy != null)
        {
            weaponsHierachy.SetActive(true);
        }
    }

    private void RouletteUpdate()
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

        if (weaponsHierachy != null)
        {
            weaponsHierachy.SetActive(false);
        }
    }
	
    /// <summary>
    /// Skip to next EPlayerState.
    /// If we add more states, make sure we add functionality here.
    // We've intentionally left out Roulette for the Time being...........
    /// </summary>
	void DevHacks()
	{
		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			//currentPlayerState = EPlayerState.Roulette;
			currentPlayerState = EPlayerState.Control;
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
		
		//Old code- consider removing
		/*
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
				//currentPlayerState = EPlayerState.Suicide;
			}
			else
			if (currentPlayerState == EPlayerState.Suicide)
			{
				//currentPlayerState = EPlayerState.Roulette;
				currentPlayerState = EPlayerState.Control;
			}
		}
		*/
		if (currentPlayerState == EPlayerState.Control)
		{
			if (Input.GetButtonDown(gameObject.tag + "Select"))
			{
				if (timeBetweenStall < 0)
				{
					currentPlayerState = EPlayerState.Dying;
				}
			}
		}
		
		// Stop spamming stall
		if (currentPlayerState == EPlayerState.Dying || currentPlayerState == EPlayerState.Suicide || currentPlayerState == EPlayerState.Roulette)
		{
			timeBetweenStall = 5.0f;
		}
		
		// Hacky!! Make auto stall an option
		if (currentPlayerState == EPlayerState.Dying)
		{
		
			// If the button is not down, but the player is allowed to escape the stall anyway.
			if (!Input.GetButton(gameObject.tag + "Select"))
			{
				if (escapeStall)
				{
					//Take a Stall value and pass it into the suicide script.
					float timer = m_dyingScript.timerUntilBoost;
				
					currentPlayerState = EPlayerState.Suicide;
					
					m_suicideScript.timerUntilReset = timer;
					
					Debug.Log(timer);
				}
			}
		}
		
		if (currentPlayerState == EPlayerState.Control)
		{
			//Then player is not stalling
			stallCommit = 1.0f;
		}
		
		
		if (Input.GetButtonDown(gameObject.tag + "Start"))
		{
			Application.LoadLevel(Application.loadedLevelName);
		}
	}
}
