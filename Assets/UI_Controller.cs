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
		public float textTurnSpeed = 3;
		public float textTimeOnScreen = 1;
	
		public GameObject one;
		private Text oneText;
		private string rememberOneWords;
		private int rememberOneSize;
		private Color rememberOneColour;
		
		public GameObject two;
		private Text twoText;
		private string rememberTwoWords;
		private int rememberTwoSize;
		private Color rememberTwoColour;
		
		
		public GameObject three;
		private Text threeText;
		private string rememberThreeWords;
		private int rememberThreeSize;
		private Color rememberThreeColour;
		
		public GameObject four;
		private Text fourText;
		private string rememberFourWords;
		private int rememberFourSize;
		private Color rememberFourColour;
		
		
		private bool oneHidden = true;
		private bool twoHidden = true;
		private bool threeHidden = true;
		private bool fourHidden = true;
	
		void Awake () 
		{
			//Get Text references
			oneText = one.GetComponent<Text>();
			twoText = two.GetComponent<Text>();
			threeText = three.GetComponent<Text>();
			fourText = four.GetComponent<Text>();
			
			
			//Remember stuff for Reset function
			rememberOneWords = " ";
			rememberOneSize = oneText.fontSize;
			rememberOneColour = oneText.color;
			
			rememberTwoWords = " ";
			rememberTwoSize = twoText.fontSize;
			rememberTwoColour = twoText.color;
			
			rememberThreeWords = " ";
			rememberThreeSize = threeText.fontSize;
			rememberThreeColour = threeText.color;
			
			rememberFourWords = " ";
			rememberFourSize = fourText.fontSize;
			rememberFourColour = fourText.color;
		}
		
		void Start()
		{
			Invoke("StartGameText", 5);
		}
		
		void StartGameText()//GameObject player)
		{
			string  startText = "Collect Passengers!";//= "GO!";
		
			//Make all text show at start
				oneText.text = startText;
				ShowText(one);
			
				twoText.text = startText;
				ShowText(two);
			
				threeText.text = startText;
				ShowText(three);
			
				fourText.text = startText;
				ShowText(four);
		}
		
		public void Score(string factionName)
		{
			//Set text
			string score; 
			
			if (factionName == "NONAME")
			{
				Debug.Log("error - no faction name set");
			}
			
			if (factionName == null)
			{
				Debug.Log("error - no faction name set");
			}
		
			if (factionName == "PIRATES")
			{
				score = (factionName + " SCORE!");
				oneText.text = score;
				ShowText(one);
			}
			
			if (factionName == "NAVY")
			{
				score = (factionName + " SCORE!");
				twoText.text = score;
				ShowText(two);
			}
			
			if (factionName == "TINKERERS")
			{
				score = (factionName + " SCORE!");
				threeText.text = score;
				ShowText(three);
			}
			
			if (factionName == "VIKINGS")
			{
				score = (factionName + " SCORE!");
				fourText.text = score;
				ShowText(four);
			}
		
		}
		
		
		void Update () 
		{
			TotalTextVisiblity();
	
			
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (threeHidden)
				{
					ShowText(three);
				}
				else
				{
					HideText(three);
				}
			}
			
			print("Text: " + threeText.text + "   Size : " + threeText.fontSize + "   Colour: "	+ threeText.color);
		}
	
		void TotalTextVisiblity()
		{
			//Player one stuff	
			if (!oneHidden)
			{	
				one.transform.localRotation = Quaternion.Lerp(one.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * textTurnSpeed);
				Invoke("HideOne", textTimeOnScreen);
			}
			else
			if (oneHidden)
			{
				one.transform.localRotation = Quaternion.Lerp(one.transform.localRotation, Quaternion.Euler(new Vector3(0, -90, 0)), Time.deltaTime * textTurnSpeed);
			}
			
			//Player two visibility
			if (!twoHidden)
			{	
				two.transform.localRotation = Quaternion.Lerp(two.transform.localRotation, Quaternion.Euler(new Vector3(0, 0.01f, 0)), Time.deltaTime * textTurnSpeed);
				Invoke("HideTwo", textTimeOnScreen);
			}
			else
			if (twoHidden)
			{
				two.transform.localRotation = Quaternion.Lerp(two.transform.localRotation, Quaternion.Euler(new Vector3(0, 90, 0)), Time.deltaTime * textTurnSpeed);
			}
			
			//Player three visibility
			if (!threeHidden)
			{	
				three.transform.localRotation = Quaternion.Lerp(three.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * textTurnSpeed);
				Invoke("HideThree", textTimeOnScreen);
			}
			else
			if (threeHidden)
			{
				three.transform.localRotation = Quaternion.Lerp(three.transform.localRotation, Quaternion.Euler(new Vector3(0, -90, 0)), Time.deltaTime * textTurnSpeed);
			}
			
			//Player four visibility
			if (!fourHidden)
			{	
				four.transform.localRotation = Quaternion.Lerp(four.transform.localRotation, Quaternion.Euler(new Vector3(0, 0.01f, 0)), Time.deltaTime * textTurnSpeed);
				Invoke("HideFour", textTimeOnScreen);
			}
			else
				if (fourHidden)
			{
				four.transform.localRotation = Quaternion.Lerp(four.transform.localRotation, Quaternion.Euler(new Vector3(0, 90, 0)), Time.deltaTime * textTurnSpeed);
			}
		}
		
		//This can't be Invoked..
		void HideText(GameObject player)
		{
			if (player == one)
			{
				HideOne();
			}
			else
			if (player == two)
			{
				HideTwo();
			}
			else
			if (player == three)
			{
				HideThree();
			}
			else
			if (player == four)
			{
				HideFour();
			}
		}
		
		//These can be directly referenced by Invoke
		void HideOne()
		{
			oneHidden = true;
			
			//delay the time before resetting the text.
			Invoke ("ResetOne", 1);
		}
		
		void ResetOne()
		{
		 	oneText.text = rememberOneWords;
		 	oneText.fontSize = rememberOneSize;
		 	oneText.color = rememberOneColour;
		}
		
		
		
		void HideTwo()
		{
			twoHidden = true;
			
			Invoke("ResetTwo", 1);
		}
		
		void ResetTwo()
		{
			twoText.text = rememberTwoWords;
			twoText.fontSize = rememberTwoSize;
			twoText.color = rememberTwoColour;
		}
		
		
		
		void HideThree()
		{
			threeHidden = true;
			
			Invoke("ResetThree", 1);
		}
		
		void ResetThree()
		{
			threeText.text = rememberThreeWords;
			threeText.fontSize = rememberThreeSize;
			threeText.color = rememberThreeColour;
		}
		
		
		
		void HideFour()
		{
			fourHidden = true;
			
			Invoke("ResetFour", 1);
		}
		
		void ResetFour()
		{
			fourText.text = rememberFourWords;
			fourText.fontSize = rememberFourSize;
			fourText.color = rememberFourColour;
		}
		
		
		void ShowText(GameObject player)
		{
			if (player == one)
			{
				oneHidden = false;
			}
			else
			if (player == two)
			{
				twoHidden = false;
			}
			else
			if (player == three)
			{
				threeHidden = false;
			}
			else
			if (player == four)
			{
				fourHidden = false;
			}	
		}
		
	}
}
