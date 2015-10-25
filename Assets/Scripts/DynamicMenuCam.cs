/**
 * File:	DynamicMenuCam.cs
 * Author: Rowan Donaldson
 * Maintainer: Pat Ferguson
 * Created: 09/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Makes Cam Move around Menus
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
	public class DynamicMenuCam : MonoBehaviour 
	{
	
        public GameObject[] target;
		
		public int currentPos = 1;
		public SceneManager sceneManager;
		public float lerpSpeed = 5;
		
		public bool useLerp = true;
		public bool activeOnStart = false;
		public bool useTimer = false;
		public float cycleTimer = 2.0f;
		private float internalTimer = 1;
		
		public GameObject optionalKeepChild;
	
		void Awake()
		{
			//Get rid of all children on Start
			gameObject.transform.DetachChildren();
			//Excpept for the optional keepchild
			if (optionalKeepChild != null)
			{
				optionalKeepChild.transform.SetParent(gameObject.transform);
			}
		}
	
		void Start () 
		{
			currentPos = 1;
			
			//Reset timer
			internalTimer = cycleTimer;
			
			if (activeOnStart)
			{
				Next();
			}
			
		}
		
		void Update () 
		{
			//print(Time.time);
		
			//CountDown timer
			if (useTimer)
			{
				internalTimer -= Time.deltaTime;
				
				if (internalTimer < 0)
				{
					Next();
				}
			}
			
		
		
			//Change the current pos
			if (currentPos > target.Length-1)
			{
				currentPos = 1;
			}
			
			if (currentPos < 0)
			{
				currentPos = 4;
			}

            for (int i = 0; i < target.Length; i++ )
            {
                if (currentPos == i)
                {                
                    gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, target[i].transform.rotation, Time.fixedDeltaTime * 2);
                    if (useLerp)
                    {
                  		gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target[i].transform.position, Time.fixedDeltaTime * lerpSpeed);		
                  	}
                  	else
                  	if (!useLerp)
                  	{
                  		gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target[i].transform.position, lerpSpeed);
                  	}	
                }

            }

                InputData();
		}
		
		void Next()
		{
			currentPos += 1;
			internalTimer = cycleTimer;
		}
		
		void Previous()
		{
			currentPos -= 1;
			internalTimer = cycleTimer;
		}
		
		
		void InputData()
		{	
			if (Input.GetButtonDown ("Player1_FaceDown") || Input.GetButtonDown ("Player2_FaceDown") ||Input.GetButtonDown ("Player3_FaceDown") ||Input.GetButtonDown ("Player4_FaceDown") || Input.GetKeyDown(KeyCode.Space))
			{
				Next();
			}
            else
            if (Input.GetButtonDown("Player1_FaceRight") || Input.GetButtonDown("Player2_FaceRight") || Input.GetButtonDown("Player3_FaceRight") || Input.GetButtonDown("Player4_FaceRight") || Input.GetKeyDown(KeyCode.Return))
            {
                Previous();
            }
			
			if (Input.GetButtonDown("Player1_Start") || Input.GetButtonDown("Player2_Start") || Input.GetButtonDown("Player3_Start") || Input.GetButtonDown("Player4_Start") || Input.GetKeyDown(KeyCode.Escape))
			{
				Debug.Log("Go to Menu");
				sceneManager.MenuScene();
			}
		
			
		}
	}
}
