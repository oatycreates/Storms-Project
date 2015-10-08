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

namespace ProjectStorms
{
	public class DynamicMenuCam : MonoBehaviour 
	{
		public GameObject target1;
		public GameObject target2;
		public GameObject target3;
		public GameObject target4;
		
		public int currentPos = 1;
		//private int leftRight = 0;
		private bool maxPush = false;
		
		public SceneManager sceneManager;
	
	
		void Start () 
		{
			currentPos = 1;
		}
		
		void Update () 
		{
			if (currentPos > 4)
			{
				currentPos = 1;
			}
			
			if (currentPos < 1)
			{
				currentPos = 4;
			}
			
			
			
		
			if (currentPos == 1)
			{
				gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, target1.transform.rotation, Time.fixedDeltaTime * 2);
				gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target1.transform.position, Time.fixedDeltaTime * 5);
			}
			else
			if (currentPos == 2)
			{
				gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, target2.transform.rotation, Time.fixedDeltaTime * 2);
				gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target2.transform.position, Time.fixedDeltaTime * 5);
			}
			else
			if (currentPos == 3)
			{
				gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, target3.transform.rotation, Time.fixedDeltaTime * 2);
				gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target3.transform.position, Time.fixedDeltaTime * 5);
			}
			else
			if (currentPos == 4)
			{
				gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, target4.transform.rotation, Time.fixedDeltaTime * 2);
				gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, target4.transform.position, Time.fixedDeltaTime * 5);
			}
			
			InputData();
		}
		
		void InputData()
		{	
			if (Input.GetButtonDown ("Player1_FaceDown") || Input.GetButtonDown ("Player2_FaceDown") ||Input.GetButtonDown ("Player3_FaceDown") ||Input.GetButtonDown ("Player4_FaceDown"))
			{
				currentPos += 1;
			}
			
			if (Input.GetButtonDown("Player1_Start") || Input.GetButtonDown("Player2_Start") || Input.GetButtonDown("Player3_Start") || Input.GetButtonDown("Player4_Start"))
			{
				sceneManager.MenuScene();
			}
		
							/*		
			if (!maxPush)
			{
				if (Input.GetAxis("Player1_Horizontal") == 1 || Input.GetAxis("Player2_Horizontal") == 1 || Input.GetAxis("Player3_Horizontal") == 1|| Input.GetAxis("Player4_Horizontal") == 1)
				{
					currentPos += 1;
					maxPush = true;
				}
				else
				if (Input.GetAxis("Player1_Horizontal") == -1 || Input.GetAxis("Player2_Horizontal") == -1 || Input.GetAxis("Player3_Horizontal") == -1|| Input.GetAxis("Player4_Horizontal") == -1)
				{
					currentPos -= 1;
					maxPush = true;
				}
			}
			
			
			//Reset button push
			if (Input.GetAxis("Player1_Horizontal") != 0 || Input.GetAxis("Player2_Horizontal") != 0 || Input.GetAxis("Player3_Horizontal") != 0|| Input.GetAxis("Player4_Horizontal") != 0)
			{
				maxPush = true;
			}
			*/
			print(currentPos);
		}
	}
}
