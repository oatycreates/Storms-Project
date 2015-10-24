/**
 * File: TextSprayr.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 24/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Controls the canvas and the text that is used during gameplay.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ProjectStorms
{
	[RequireComponent(typeof(Canvas))]
	public class TextSpray : MonoBehaviour 
	{
	//Get all the cameras in the scene
	public MasterCamera camController;
	
	private int quadrantsOnCanvas = 1;
	
	
	//Pool gameobjects
	public int pooledText = 100;
	List<GameObject> scoreText;
	public GameObject scoreTextPrefab;
	
	
		void Start() 
		{
			scoreText = new List<GameObject>();
			
			for (int i = 0; i < pooledText; i++)
			{
				/*
				GameObject singleText = new GameObject();
						
				singleText.AddComponent<CanvasRenderer>();
				singleText.AddComponent<Text>();
				*/
				
				GameObject singleText = Instantiate(scoreTextPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;
				
				singleText.transform.SetParent(gameObject.transform);
				singleText.transform.position = gameObject.transform.position;
				singleText.transform.rotation = Quaternion.identity;
				
				singleText.GetComponent<Text>().text = "Score!";
				singleText.GetComponent<Text>().color = Color.red;
				
				scoreText.Add(singleText);	
				
			}
		}
		
		void Update () 
		{
			CamInfo();
			
		}
		
	
		
		
		
		void CamInfo()
		{
			if (camController != null)
			{
				//Check the number of cameras in scene
				if (camController.currentCamera == ECamerasInScene.One)
				{
					quadrantsOnCanvas = 1;
				}
				else
					if (camController.currentCamera == ECamerasInScene.Two)
				{
					quadrantsOnCanvas = 2;
				}
				else
					if (camController.currentCamera == ECamerasInScene.Three)
				{
					quadrantsOnCanvas = 3;
				}
				else
					if (camController.currentCamera == ECamerasInScene.Four)
				{
					quadrantsOnCanvas = 4;
				}
			}
			else
				//if no cam controller in scene
				if (camController == null)
			{
				quadrantsOnCanvas = 1;
			}
		}
	}
}