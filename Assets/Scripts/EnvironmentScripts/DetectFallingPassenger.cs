/**
 * File: DetectFallingPassenger.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script detects falling passenger pirates that enter the triggerzone. It deactivates the passengers and adds to player score(by subtracting from regular score).
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
	
/// <summary>
/// This script detects falling passenger pirates that enter the triggerzone. It deactivates the passengers and adds to player score(by subtracting from regular score).
/// </summary>
public class DetectFallingPassenger : MonoBehaviour 
{
	public Text scoreText;
	[HideInInspector]
	public int peopleLeftToCatch = 100;

	public Color textColour;

	void Start()
	{
		textColour = Color.black;
	}


	void Update () 
	{
		scoreText.color = textColour;
		scoreText.text = ("" + peopleLeftToCatch);
	}

	void OnTriggerEnter(Collider a_other)
	{
		if (a_other.tag == "Passengers")
		{
			peopleLeftToCatch -= 1;
			a_other.gameObject.SetActive(false);
		}
	}
}
