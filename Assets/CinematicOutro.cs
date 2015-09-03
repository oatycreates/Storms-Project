﻿/**
 * File: CinematicScript.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 02/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script controls the 'Cinematic' and camera that is triggered at the end of a gameplay match.
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CinematicOutro : MonoBehaviour 
{
	public GameObject rotator;
	public GameObject pirate;
	private Renderer pirateRenderer;
	public Text winText;
	public Color myColour = Color.white;


	void Awake() 
	{
		pirateRenderer = pirate.GetComponent<Renderer>();
        pirateRenderer.enabled = false;

		//Go to sleep untill I'm needed
		gameObject.SetActive (false);
	}

	void FixedUpdate() 
	{
		rotator.transform.Rotate(Vector3.down * Time.deltaTime * 60);
	}

	public void WinCam(string winnerNumberText, Color winnerColour)
	{
		winText.text = winnerNumberText;
        winText.color = winnerColour;
        pirateRenderer.enabled = true;
		pirateRenderer.material.color = winnerColour;
	}
}
