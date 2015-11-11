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
using System.Collections.Generic;

namespace ProjectStorms
{
	public class UI_Controller : MonoBehaviour 
	{
		public float textTurnSpeed = 3;
		public float textTimeOnScreen = 1;

        private GameObject one;
		private Text oneText;
		private string rememberOneWords;
		private int rememberOneSize;
		private Color rememberOneColour;

        private GameObject two;
		private Text twoText;
		private string rememberTwoWords;
		private int rememberTwoSize;
		private Color rememberTwoColour;

        private GameObject three;
		private Text threeText;
		private string rememberThreeWords;
		private int rememberThreeSize;
		private Color rememberThreeColour;
		
		private GameObject four;
		private Text fourText;
		private string rememberFourWords;
		private int rememberFourSize;
		private Color rememberFourColour;

		private bool oneHidden = true;
		private bool twoHidden = true;
		private bool threeHidden = true;
		private bool fourHidden = true;

        private bool m_beenInitialised = false;
        
        //Audio stuff
        private AudioSource m_source;
        public AudioClip catchSound;
        public AudioClip scoreSound;

		public void InitialiseAnnouncerText(GameObject[] a_playerGOs) 
		{		
            // Get Text references
            oneText = a_playerGOs[0].GetComponentInChildren<Text>();
            twoText = a_playerGOs[1].GetComponentInChildren<Text>();
            threeText = a_playerGOs[2].GetComponentInChildren<Text>();
            fourText = a_playerGOs[3].GetComponentInChildren<Text>();

            // Set parent canvas references
            one = oneText.transform.parent.gameObject;
            two = twoText.transform.parent.gameObject;
            three = threeText.transform.parent.gameObject;
            four = fourText.transform.parent.gameObject;

			//Remember stuff for Reset function
			rememberOneWords = " ";
			rememberOneSize =	oneText.fontSize;
			rememberOneColour =	oneText.color;
			
			rememberTwoWords = " ";
			rememberTwoSize = twoText.fontSize;
			rememberTwoColour = twoText.color;
			
			rememberThreeWords = " ";
			rememberThreeSize = threeText.fontSize;
			rememberThreeColour = threeText.color;
			
			rememberFourWords = " ";
			rememberFourSize = fourText.fontSize;
            rememberFourColour = fourText.color;

            m_beenInitialised = true;

            Invoke("StartGameText", 5);
		}
		
		void Awake()
		{
			m_source = gameObject.GetComponent<AudioSource>();
			
			if (m_source == null)
			{
				gameObject.AddComponent<AudioSource>();
				m_source = gameObject.GetComponent<AudioSource>();
			}
			
			m_source.loop = false;
		}
		
		
		
		void StartGameText()//GameObject player)
		{
			string  startText = "Collect Passengers!";
		
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
		
		public void Score(string factionName, int currentScore, int screenQuadrant)
		{
			//Set text
			string score; 	//What everyone else sees.
			
			//score = (factionName + " SCORE!");
			
			
			string wordNumber = currentScore.ToString();
			string passengersLeft;
			
			if (factionName == "NONAME")
			{
				Debug.Log("error - no faction name set");
			}
			
			if (factionName == null)
			{
				Debug.Log("error - no faction name set");
			}
			
			if (screenQuadrant == 0)
			{
				Debug.Log("No screen quadrant set.");
			}
			
			if (screenQuadrant > 4)
			{
				Debug.Log("Wrong screen quadrant.");
			}
		
			
			//if (factionName == "PIRATES")
			if (screenQuadrant == 1)
			{
				score = (factionName + " SCORE!"); 
				passengersLeft = ("Need " + wordNumber + " more!" );
				
				
				oneText.text = score;
				oneText.fontSize = 100;
				oneText.color = rememberOneColour;
				
				twoText.text = score;
				twoText.fontSize = 50;
				twoText.color = rememberOneColour;
				
				threeText.text = score;
				threeText.fontSize = 50;
				threeText.color = rememberOneColour;
				
				fourText.text = score;
				fourText.fontSize = 50;
				fourText.color = rememberOneColour;
				
				ShowText(one);
				ShowText(two);
				ShowText(three);
				ShowText(four);
			}
			
			//if (factionName == "NAVY")
			if (screenQuadrant == 2)
			{
				score = (factionName + " SCORE!"); 
				passengersLeft = ("Need " + wordNumber + " more!" );
				
				
				oneText.text = score;
				oneText.fontSize = 50;
				oneText.color = rememberTwoColour;
				
				twoText.text = score;
				twoText.fontSize = 100;
				twoText.color = rememberTwoColour;
				
				threeText.text = score;
				threeText.fontSize = 50;
				threeText.color = rememberTwoColour;
				
				fourText.text = score;
				fourText.fontSize = 50;
				fourText.color = rememberTwoColour;
				
				ShowText(one);
				ShowText(two);
				ShowText(three);
				ShowText(four);
			}
			
			//if (factionName == "TINKERERS")
			if (screenQuadrant == 3)
			{
				score = (factionName + " SCORE!"); 
				passengersLeft = ("Need " + wordNumber + " more!" );
				
				
				oneText.text = score;
				oneText.fontSize = 50;
				oneText.color = rememberThreeColour;
				
				twoText.text = score;
				twoText.fontSize = 50;
				twoText.color = rememberThreeColour;
				
				threeText.text = score;
				threeText.fontSize = 100;
				threeText.color = rememberThreeColour;
				
				fourText.text = score;
				fourText.fontSize = 50;
				fourText.color = rememberThreeColour;
				
				ShowText(one);
				ShowText(two);
				ShowText(three);
				ShowText(four);
			}
			
			//if (factionName == "VIKINGS")
			if (screenQuadrant == 4)
			{
				score = (factionName + " SCORE!"); 
				passengersLeft = ("Need " + wordNumber + " more!" );
				
				
				oneText.text = score;
				oneText.fontSize = 50;
				oneText.color = rememberFourColour;
				
				twoText.text = score;
				twoText.fontSize = 50;
				twoText.color = rememberFourColour;
				
				threeText.text = score;
				threeText.fontSize = 50;
				threeText.color = rememberFourColour;
				
				fourText.text = score;
				fourText.fontSize = 100;
				fourText.color = rememberFourColour;
				
				ShowText(one);
				ShowText(two);
				ShowText(three);
				ShowText(four);
			}
			
						
			//Audio stuff
			if (!m_source.isPlaying)
			{
				m_source.pitch = 1;
				PlayScoreSound();
			}
			else
			if (m_source.isPlaying)
			{
				//Rest timer
				m_source.time = 0;
				m_source.pitch = 1;
				PlayScoreSound();
			}
			
		}
		
		
		/*
		public void HalfWay(string factionName)
		{
			string halfWay = "HALF WAY!";
			
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
				oneText.text = halfWay;
				oneText.fontSize = 100;
				ShowText(one);
			}
			
			if (factionName == "NAVY")
			{
				twoText.text = halfWay;
				twoText.fontSize = 100;
				ShowText(two);
			}
			
			if (factionName == "TINKERERS")
			{
				threeText.text = halfWay;
				threeText.fontSize = 100;
				ShowText(three);
			}
			
			if (factionName == "VIKINGS")
			{
				fourText.text = halfWay;
				fourText.fontSize = 100;
				ShowText(four);
			}
		}
		*/
		
		public void PassengersInTray(string factionName, int noOfPassengers, bool morePassengers, string airshipTag)
		{
			//print (factionName + "  " + noOfPassengers);
			string people;
			people = noOfPassengers.ToString();
			
			//Save this just in case
			/*
			if (factionName == "NONAME" || factionName == null)
			{
				Debug.Log("error - no faction name set");
			}
			
			if (factionName == "PIRATES")
			{
				oneText.text = people;
				oneText.fontSize = 100;
				//Cancel any existing movment on the object.
				CancelInvoke("HideOne");
				ShowText(one);
			}
			
			if (factionName == "NAVY")
			{
				twoText.text = people;
				twoText.fontSize = 100;
				CancelInvoke("HideTwo");
				ShowText(two);
			}
			
			if (factionName == "TINKERERS")
			{
				threeText.text = people;
				threeText.fontSize = 100;
				CancelInvoke("HideThree");
				ShowText(three);
			}
			
			if (factionName == "VIKINGS")
			{
				fourText.text = people;
				fourText.fontSize = 100;
				CancelInvoke("HideFour");
				ShowText(four);
			}
			*/
			
			if (airshipTag == "Player1_")
			{
					oneText.text = people;
					oneText.fontSize = 100;
				
					CancelInvoke("HideOne");
					ShowText(one);
			}
			else
			if (airshipTag == "Player2_")
			{
				twoText.text = people;
				twoText.fontSize = 100;
				CancelInvoke("HideTwo");
				ShowText(two);
			}
			else
			if (airshipTag == "Player3_")
			{
				threeText.text = people;
				threeText.fontSize = 100;
				CancelInvoke("HideThree");
				ShowText(three);
			}
			else
			if (airshipTag == "Player4_")
			{
				fourText.text = people;
				fourText.fontSize = 100;
				CancelInvoke("HideFour");
				ShowText(four);
			}
			
		
			
			//Audio stuff
			if (morePassengers)
			{
				float pitchPerPassenger =  (float)(noOfPassengers)/25;
				float newPitch = 0.5f + pitchPerPassenger;
				
			
				if (!m_source.isPlaying)
				{	
					//Set pitch according to passengers in tray
					if (newPitch <= 1.3f)
					{
						m_source.pitch = newPitch;
					}
					else
					{
						m_source.pitch = 1.3f;
					}
					
					PlayCatchSound();
				}
				else
				if (m_source.isPlaying)
				{
					//Reset sound
					m_source.time = 0;
					
					//Set pitch according to passengers in tray
					if (newPitch <= 1.3f)
					{
						m_source.pitch = newPitch;
					}
					else
					{
						m_source.pitch = 1.3f;
					}
					
					PlayCatchSound();
				}
			}	
		}

        public void InvertYCam(string factionName, string airshipTag)
        {
            string invert = "Cam Inverted";

            if (factionName == "NONAME")
            {
                Debug.Log("error - no faction name set");
            }

            if (factionName == null)
            {
                Debug.Log("error - no faction name set");
            }
            
			
			if (airshipTag == null)
			{
				Debug.Log("No player tag");
			}
			
			

           // if (factionName == "PIRATES")
           if (airshipTag == "Player1_")
            {
                oneText.text = invert;
                oneText.fontSize = 100;
                //Cancel any existing movment on the object.
                CancelInvoke("HideOne");
               	ShowText(one);
            }

            //if (factionName == "NAVY")
            if (airshipTag == "Player2_")
            {
                twoText.text = invert;
                twoText.fontSize = 100;
                CancelInvoke("HideTwo");
                ShowText(two);
            }

            //if (factionName == "TINKERERS")
            if (airshipTag == "Player3_")
            {
                threeText.text = invert;
                threeText.fontSize = 100;
                CancelInvoke("HideThree");
                ShowText(three);
            }

            //if (factionName == "VIKINGS")
            if (airshipTag == "Player4_")
            {
                fourText.text = invert;
                fourText.fontSize = 100;
                CancelInvoke("HideFour");
                ShowText(four);
            }

        }

        public void NormalYCam(string factionName, string airshipTag)
        {
            string normal = "Cam Normal";

            if (factionName == "NONAME")
            {
                Debug.Log("error - no faction name set");
            }

            if (factionName == null)
            {
                Debug.Log("error - no faction name set");
            }
            
            if (airshipTag == null)
            {
            	Debug.Log("No player tag");
            }
            
           

            //if (factionName == "PIRATES")
            if (airshipTag == "Player1_")
            {
                oneText.text = normal;
                oneText.fontSize = 100;
                //Cancel any existing movment on the object.
                CancelInvoke("HideOne");
                ShowText(one);
            }

            //if (factionName == "NAVY")
            if (airshipTag == "Player2_")
            {
                twoText.text = normal;
                twoText.fontSize = 100;
                CancelInvoke("HideTwo");
                ShowText(two);
            }

            //if (factionName == "TINKERERS")
            if (airshipTag == "Player3_")
            {
                threeText.text = normal;
                threeText.fontSize = 100;
                CancelInvoke("HideThree");
                ShowText(three);
            }

           // if (factionName == "VIKINGS")
           if (airshipTag == "Player4_")
            {
                fourText.text = normal;
                fourText.fontSize = 100;
                CancelInvoke("HideFour");
                ShowText(four);
            }

        }

        public void LockOn(string factionName, string targetAirshipTag)
        {
            string warning = "Incoming Missile";

            if (factionName == "NONAME")
            {
                Debug.Log("error - no faction name set");
            }

            if (factionName == null)
            {
                Debug.Log("error - no faction name set");
            }
            
            if (targetAirshipTag == null)
            {
            	Debug.Log("Target doesn't use player tag");
            }

            //if (factionName == "PIRATES")
            if (targetAirshipTag == "Player1_")
            {
                oneText.text = warning;
                oneText.fontSize = 100;
                //Cancel any existing movment on the object.
                CancelInvoke("HideOne");
                ShowText(one);
            }

            //if (factionName == "NAVY")
            if (targetAirshipTag == "Player2_")
            {
                twoText.text = warning;
                twoText.fontSize = 100;
                CancelInvoke("HideTwo");
                ShowText(two);
            }

            //if (factionName == "TINKERERS")
            if (targetAirshipTag == "Player3_")
            {
                threeText.text = warning;
                threeText.fontSize = 100;
                CancelInvoke("HideThree");
                ShowText(three);
            }

            //if (factionName == "VIKINGS")
            if (targetAirshipTag == "Player4_")
            {
                fourText.text = warning;
                fourText.fontSize = 100;
                CancelInvoke("HideFour");
                ShowText(four);
            }

        }
        
        
		void PlayCatchSound()
		{
			m_source.clip = catchSound;
			m_source.Play();
		}
		
		void PlayScoreSound()
		{
			m_source.clip = scoreSound;
			//m_source.pitch = Random.Range(1.05f, 1.15f);			
			m_source.Play();
		}
		
		void Update () 
		{
		
            if (m_beenInitialised)
            {
                TotalTextVisiblity();

#if UNITY_EDITOR
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

                //print("Text: " + threeText.text + "   Size : " + threeText.fontSize + "   Colour: "	+ threeText.color);

                //print ("Red " + oneText.color + " Navy " + twoText.color + " Green " + threeText.color + " Yellow " + fourText.color);
#endif
            }
		}
	
		void TotalTextVisiblity()
		{
			//Player one stuff	
			if (!oneHidden)
			{	
				one.transform.localRotation = Quaternion.Lerp(one.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * textTurnSpeed);
				if (Mathf.Abs(one.transform.localEulerAngles.y) < 1)
				{
					Invoke("HideOne", textTimeOnScreen);
				}
			}
			else
			if (oneHidden)
			{
				one.transform.localRotation = Quaternion.Lerp(one.transform.localRotation, Quaternion.Euler(new Vector3(0, -90, 0)), Time.deltaTime * (textTurnSpeed/2));
			}
			
			//Player two visibility
			if (!twoHidden)
			{	
				two.transform.localRotation = Quaternion.Lerp(two.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * textTurnSpeed);
				if (Mathf.Abs(two.transform.localEulerAngles.y) < 1)
				{
					Invoke("HideTwo", textTimeOnScreen);
				}
			}
			else
			if (twoHidden)
			{
				two.transform.localRotation = Quaternion.Lerp(two.transform.localRotation, Quaternion.Euler(new Vector3(0, -90, 0)), Time.deltaTime * (textTurnSpeed/2));
			}
			
			//Player three visibility
			if (!threeHidden)
			{	
				three.transform.localRotation = Quaternion.Lerp(three.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * textTurnSpeed);
				if (Mathf.Abs(three.transform.localEulerAngles.y) < 1)
				{
					Invoke("HideThree", textTimeOnScreen);
				}
			}
			else
			if (threeHidden)
			{
				three.transform.localRotation = Quaternion.Lerp(three.transform.localRotation, Quaternion.Euler(new Vector3(0, -90, 0)), Time.deltaTime * (textTurnSpeed/2));
			}
			
			//Player four visibility
			if (!fourHidden)
			{	
				four.transform.localRotation = Quaternion.Lerp(four.transform.localRotation, Quaternion.Euler(new Vector3(0, 0, 0)), Time.deltaTime * textTurnSpeed);
				if (Mathf.Abs(four.transform.localEulerAngles.y) < 1)
				{
					Invoke("HideFour", textTimeOnScreen);
				}
			}
			else
				if (fourHidden)
			{
				four.transform.localRotation = Quaternion.Lerp(four.transform.localRotation, Quaternion.Euler(new Vector3(0, -90, 0)), Time.deltaTime * (textTurnSpeed/2));
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