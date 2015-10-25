/**
 * File: Controls and keeps track of all the announcer text - children gameobject.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 15/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Create, set text and keep track of text objects.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum Player {        IsScoring,
                            IsWinning, 
                            HalfWay,
                            HowManyMoreToGo, 
                            HasWonGame, 
                            HasManyPassengers, 
                            IsBeingMissileTracked, 
                            IsBehindOtherPlayer, 
                            PassengersSpawning,
                            SoulsLost
                    }; 


namespace ProjectStorms
{

    public class AnnouncerEffects : MonoBehaviour
    {
        //State management
        public Player condition;


        //Object pooling
        public GameObject textPrefab;
        public int pooledAmount = 2000;
        List<GameObject> textObjects;
        

        //Give the text a random pos in local space
        private float width;
        private float height;
        
        private Vector3 randomPos;
        private Vector3 topLeft;
        private Vector3 topRight;
        private Vector3 topHalf;
        private Vector3 bottomLeft;
        private Vector3 bottomRight;
        private Vector3 bottomHalf;
        
        private Vector3 spawnOffset;

        //Values to feed into the text
        private GameObject player1 = null;
        private string player1Name = " ";
        private GameObject player2 = null;
        private string player2Name = " ";
        private Color player1Colour = Color.black;
        private float player1Score = 0;
        private float player1Tray = 0;


        //Set values stuff
        [HideInInspector]
        public Color textColour = Color.clear;
        private string textText = "Bump, nothing yet";

        /*
        private Color m_pirateRed = new Color(0.779f, 0.126f, 0.126f);
        //private Color m_navyWhite = new Color(1f, 1f, 1f);
        //Actually, change this colour to navy blue...
        private Color m_navyWhite = new Color(0.133f, 0.228f, 0.860f);
        private Color m_tinkererGreen = new Color(0.145f, 0.588f, 0.108f);
        private Color m_vikingYellow = new Color(0.926f, 0.842f, 0.163f);
        private Color m_warning = new Color(0, 0, 0);
         */

        //Get a component on the same gameobject
        //public ScoreManager scoreManager;

        void Start()
        {
            //Always introduce the list first
            textObjects = new List<GameObject>();

            //pool the text prefabs
            for (int i = 0; i < pooledAmount; i++)
            {
                GameObject singleTextObject = Instantiate(textPrefab, gameObject.transform.position, Quaternion.identity) as GameObject;

                singleTextObject.transform.SetParent(gameObject.transform);

                //Check for the right script
                if (singleTextObject.GetComponent<AnnouncerText>() == null)
                {
                    singleTextObject.AddComponent<AnnouncerText>();
                }

                singleTextObject.SetActive(false);

                textObjects.Add(singleTextObject);
            }
      
        
           //Use the demo TestText function
			textText = "Score!";
			Invoke("SpawnText", 1);
           
            //InvokeRepeating("SpawnText", 0, 0.1f);
        }

        void Update() 
        {    
        	width = Screen.width;
        	height = Screen.height;
        
            SetString(); 

        }
          
       //TEXT CONDITIONS       
		//Choose what the text says.
        void SetString()
       {
           //Conditional logic - what do I need to trigger each text condition

           if (condition == Player.IsScoring)
           {
               // Need Player, Base

               textText = "<Player> Scores!";
              // textText = "Score!";
           }

           if (condition == Player.IsWinning)
           {
               // Player, Base, Score
               textText = "<Player> takes the lead!";
           }
           
           if (condition == Player.HalfWay)
           {
           		//Player, Base, Score
           		textText = "Half Way!";
           }

           if (condition == Player.HowManyMoreToGo)
           {
               // Player, Base, Score
               //textText = "Only <int> left!";
               //or 
               textText = "<Player> Needs <int>";
           }

           if (condition == Player.HasWonGame)
           {
               // Player, Base, Score
               textText = "<Player> Wins!!!";
           }


           if (condition == Player.HasManyPassengers)
           {
               // Player, Passengers in Tray
               textText = "Greedy <Player> Escaping!";
           }

           if (condition == Player.IsBeingMissileTracked)
           {
               // Player 1, Missile, Target Lock
               textText = "Missile Tracking <Player>";
           }

           if (condition == Player.IsBehindOtherPlayer)
           {
               // Player 1, Player 2, Direcional Info
               textText = "<Player2> Chasing <Player1>";
           }

           if (condition == Player.PassengersSpawning)
           {
               // Passenger Spawner
               textText = "Rescue the Passengers!";
           }

           if (condition == Player.SoulsLost)
           {
               textText = "<int> souls lost";
           }
       }

		//TEXT POSITIONS
		//Choose where to spawn text, then spawn it.
        public void RandomPos()
        {
            randomPos = new Vector3(Random.Range(-(width/5), (width/5)), Random.Range(-(height/3.5f), (height/3.5f)), 0);
            spawnOffset = randomPos;
            
            SpawnText();
        }
        
        public void TopLeft()
        {
        	topLeft = new Vector3 (Random.Range(-width/2, 0), Random.Range(0, 10), 0);
        	spawnOffset = topLeft;
			SpawnText();
        }
        
        public void TopRight()
        {
        	topRight = new Vector3 (Random.Range(0, width/2), Random.Range(0, 10), 0);
        	spawnOffset = topRight;
			SpawnText();
        }
        
       	public void BottomLeft()
        {
        	bottomLeft = new Vector3(Random.Range(-width/2, 0), Random.Range(-height/2, (-height/2)+10), 0);
        	spawnOffset = bottomLeft;
			SpawnText();
        }
        
       	public void BottomRight()
        {
			bottomRight = new Vector3(Random.Range(0, width/2), Random.Range(-height/2, (-height/2)+10), 0);
        	spawnOffset = bottomRight;
			SpawnText();
        }

		//For two player mode or Team Match
		public void TopHalf()
		{
			topHalf = new Vector3 (Random.Range(-width/2, width/2), 0, 0);
			spawnOffset = topHalf;
			
			SpawnText();
		}
		
		public void BottomHalf()
		{
			bottomHalf = new Vector3 (Random.Range(-width/2, width/2),  Random.Range(-height/2, (-height/2)-10), 0);
			spawnOffset = bottomHalf;
			
			SpawnText();
		}
		
		
		//Finally, spawn the text
		public void SpawnText()
		{
			
			for (int i = 0; i < textObjects.Count; i++)
			{
				if (!textObjects[i].activeInHierarchy)
				{
					textObjects[i].transform.position = gameObject.transform.position + spawnOffset;
					textObjects[i].transform.rotation = Quaternion.identity;
					
					//Check for the right script
					if (textObjects[i].GetComponent<AnnouncerText>() != null)
					{
						textObjects[i].GetComponent<AnnouncerText>().m_myColour = textColour;
						textObjects[i].GetComponent<AnnouncerText>().m_myWords = textText;
					}
					
					textObjects[i].SetActive(true);
					
					break;
				}
			}
		}
		
    }

}