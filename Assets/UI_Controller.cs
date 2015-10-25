/**
 * File: UI_Controller.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 26/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages scoring for each player/team in the game. (This replaces the Announcer Text)
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectStorms
{
	public class UI_Controller : MonoBehaviour 
	{
		public GameObject one;
		public GameObject two;
		public GameObject three;
		public GameObject four;
		
		private float height = Screen.height;
		private float width = Screen.width;
		
	
		void Start () 
		{
			//SetPos();
		}
		
		void Update () 
		{
			height = Screen.height;
			width = Screen.width;
		}
		
		void SetPos()
		{
			Vector3 offsetOne = new Vector3 (-(width/4), (height/5), 0);
			one.transform.position = gameObject.transform.position + offsetOne;
			
			
			Vector3 offsetTwo = new Vector3 ((width/4), (height/5), 0);
			two.transform.position = gameObject.transform.position + offsetTwo;
			
			Vector3 offsetThree = new Vector3 (-(width/4), -(height/5), 0);
			three.transform.position = gameObject.transform.position + offsetThree;
			
			Vector3 offsetFour = new Vector3 ((width/4), -(height/5), 0);
			four.transform.position = gameObject.transform.position + offsetFour;
		}
		
		void SpinText()
		{
			
		}
	}
}
