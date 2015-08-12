/**
 * File: InEditorStuff.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Editor-only scripts. Everything here is only for ease of access, and should only effect stuff in the editor.
 *              We should delete the Entire 'InEditor' branch of the airshipGameobject before master build.
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Everything here is only for ease of access, and should only effect stuff in the editor.
/// We should delete the Entire 'InEditor' branch of the airshipGameobject before master build.
/// </summary>
public class InEditorStuff : MonoBehaviour
{

	public Renderer myRenderer;
	private Color m_playerColor = Color.magenta;
	
	public GameObject airshipTopOfHierachy;
	public GameObject canvasChild;
	private Text m_canvasText;
	
	void Start()
	{
		m_canvasText = canvasChild.GetComponentInChildren<Text>();
		/*
		if (Application.isEditor == false)
		{
			canvasChild.SetActive(false);
		}*/
	}

	
	void Update () 
	{
	    // Set color
		if (myRenderer.enabled == true)
		{
			/*
			if (Application.isEditor == true)
			{
				myRenderer.material.color = playerColor;	
			}
			*/
			myRenderer.material.color = m_playerColor;
		
			if (gameObject.tag == "Player1_")
			{
				m_playerColor = Color.magenta;
			}
			
			if (gameObject.tag == "Player2_")
			{
				m_playerColor = Color.cyan;
			}
			
			if (gameObject.tag == "Player3_")
			{
				m_playerColor = Color.green;
			}
			
			if (gameObject.tag == "Player4_")
			{
				m_playerColor = Color.yellow;
			}
		}
		
		// Explain game states
		m_canvasText.text = ("State: " + (airshipTopOfHierachy.GetComponent<StateManager>().currentPlayerState));
	}
}
