/**
 * File: PirateBaseIdentity.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script keeps track of the pirate base and identity - checks current score and tags.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This script keeps track of the pirate base and identity - checks current score and tags.
/// </summary>
public class PirateBaseIdentity : MonoBehaviour 
{

	public DetectFallingPassenger baseTriggerZone;

	[HideInInspector]
	public int baseScore;
	[HideInInspector]
	public int teamNumber;
	[HideInInspector]
	public Color baseColour;

	public Renderer myRenderer;

    public Renderer flagRenderer;
    public Renderer flagBackRenderer;

	void Start() 
	{
		baseColour = Color.clear;
	}

	void Update() 
	{
		if (gameObject.tag == "Player1_")
		{
			teamNumber = 1;
			myRenderer.material.color = Color.magenta;
			baseColour = myRenderer.material.color;

		}
		else if (gameObject.tag == "Player2_")
		{
			teamNumber = 2;
			myRenderer.material.color = Color.cyan;
			baseColour = myRenderer.material.color;
		}
		else if (gameObject.tag == "Player3_")
		{
			teamNumber = 3;
			myRenderer.material.color = Color.green;
			baseColour = myRenderer.material.color;
		}
		else if (gameObject.tag == "Player4_")
		{
			teamNumber = 4;
			myRenderer.material.color = Color.yellow;
			baseColour = myRenderer.material.color;
		}
		else if (gameObject.tag == "Untagged")
		{
			Debug.Log("NO TAG!!");
		}

        // Set the flag's colour
        Color flagColour = baseColour;
        flagColour.a *= 0.75f;
        flagRenderer.material.color = flagColour;
        flagBackRenderer.material.color = flagColour;

		// Set the colour of text in DetectFallingPassengersScript
		baseTriggerZone.textColour = myRenderer.material.color;

		// Set the score for the ScoreManager script
        baseScore = baseTriggerZone.peopleLeftToCatch;
	}

    public void ResetPirateBase(int a_baseScore)
    {
        // Set score values
        baseTriggerZone.peopleLeftToCatch = a_baseScore;
        baseScore = a_baseScore;
    }
}
