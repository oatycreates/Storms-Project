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
        private Vector3 randomPos;

        //Values to feed into the text
        private GameObject player1 = null;
        private string player1Name = " ";
        private GameObject player2 = null;
        private string player2Name = " ";
        private Color player1Colour = Color.black;
        private float player1Score = 0;
        private float player1Tray = 0;


        //Set values stuff
        private Color textColour = Color.clear;
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
        public ScoreManager scoreManager;

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
            InvokeRepeating("SpawnText", 0, 0.5f);
            
            condition = Player.PassengersSpawning;
        }

        void Update() 
        {
            SetString(); 

            if (scoreManager.pirateBase1.baseScore < (scoreManager.pirateBase1.baseScore/2))
            {
                condition = Player.IsWinning;
            }

            //Come back to this!
        }
        

       public void SpawnText()
        {
            RandomPos();

            for (int i = 0; i < textObjects.Count; i++)
            {
                if (!textObjects[i].activeInHierarchy)
                {
                    textObjects[i].transform.position = gameObject.transform.position + randomPos;
                    textObjects[i].transform.rotation = Quaternion.identity;

                    //Check for the right script
                    if (textObjects[i].GetComponent<AnnouncerText>() != null)
                    {
                        textObjects[i].GetComponent<AnnouncerText>().m_myWords = textText;
                    }

                    textObjects[i].SetActive(true);

                    break;
                }
            }
        }

        void SetString()
       {
           //Conditional logic - what do I need to trigger each text condition

           if (condition == Player.IsScoring)
           {
               // Need Player, Base

               textText = "<Player> Scores!";
           }

           if (condition == Player.IsWinning)
           {
               // Player, Base, Score
               textText = "<Player> takes the lead!";
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

        void RandomPos()
        {
            float height = Screen.height / 2;
            float width = Screen.width / 2;

            randomPos = new Vector3(Random.Range(-(width/3), (width/3)), Random.Range(-(height/1.5f), (height/1.5f)), 0);
        }


    }

}