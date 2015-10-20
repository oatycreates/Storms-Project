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

namespace ProjectStorms
{
    public enum PlayerColour { pirateRed, navyWhite, tinkererGreen, vikingYellow, spawningPassengers};

    public class AnnouncerEffects : MonoBehaviour
    {
        public GameObject textPrefab;
        private Vector3 randomPos;

        public PlayerColour scoringColour;

        private Color textColour = Color.clear;
        private Color m_pirateRed = new Color(0.779f, 0.126f, 0.126f);
        //private Color m_navyWhite = new Color(1f, 1f, 1f);
        //Actually, change this colour to navy blue...
        private Color m_navyWhite = new Color(0.133f, 0.228f, 0.860f);
        private Color m_tinkererGreen = new Color(0.145f, 0.588f, 0.108f);
        private Color m_vikingYellow = new Color(0.926f, 0.842f, 0.163f);
        private Color m_warning = new Color(0, 0, 0);


        //Spawntimer
        private float spawnTimer = 0.0f;
        private float timerValue = 0.5f;

        void Start()
        {
           // print(m_navyWhite);
           // InvokeRepeating("Spawning", 0, 0.1f);
            //Invoke("Spawning", 0.1f);
        }

        void Update()
        {
            //When to spawn text
            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;

                //SpawnText();
                //InvokeRepeating("SpawnText", 0, 0.01f);
                //Set this in same place we start the spawn timer
            }
            else
            if (spawnTimer <= 0)
            {
                CancelInvoke("SpawnText");
            }

            //Details of spawning text
            if (scoringColour == PlayerColour.pirateRed)
            {
                textColour = m_pirateRed;
            }
            else
            if (scoringColour == PlayerColour.navyWhite)
            {
                textColour = m_navyWhite;
            }
            if (scoringColour == PlayerColour.tinkererGreen)
            {
                textColour = m_tinkererGreen;
            }
            else
            if (scoringColour == PlayerColour.vikingYellow)
            {
                textColour = m_vikingYellow;
            }
            else
            if (scoringColour == PlayerColour.spawningPassengers)
            {
                textColour = m_warning;
            }
        }

       public void SpawnText()
        {
            RandomPos();

           GameObject go = Instantiate(textPrefab, gameObject.transform.position + randomPos, Quaternion.identity) as GameObject;
           go.transform.SetParent(gameObject.transform);
          // go.gameObject.GetComponent<AnnouncerText>().SetColour(playerTeamColour);
           go.gameObject.GetComponent<AnnouncerText>().SetColour(textColour);

           if (textColour == m_pirateRed)
           {
               go.gameObject.GetComponent<AnnouncerText>().messageType = MessageType.scoring;
           }
           else
           if (textColour == m_navyWhite)
           {
               go.gameObject.GetComponent<AnnouncerText>().messageType = MessageType.scoring;
           }
           else
           if (textColour == m_tinkererGreen)
           {
               go.gameObject.GetComponent<AnnouncerText>().messageType = MessageType.scoring;
           }
           else
           if (textColour == m_vikingYellow)
           {
               go.gameObject.GetComponent<AnnouncerText>().messageType = MessageType.scoring;
           }
           else
           if (textColour == m_warning)
           {
               go.gameObject.GetComponent<AnnouncerText>().messageType = MessageType.warning;
           }
           
        }

        void RandomPos()
        {
            float height = Screen.height / 2;
            float width = Screen.width / 2;

            randomPos = new Vector3(Random.Range(-(width/3), (width/3)), Random.Range(-(height/1.5f), (height/1.5f)), 0);
        }



        //Differen public functions to be called
        public void Pirate()
        {
            scoringColour = PlayerColour.pirateRed;
            spawnTimer = timerValue;
            InvokeRepeating("SpawnText", 0, 0.5f);
        }

        public void Navy()
        {
            scoringColour = PlayerColour.navyWhite;
            spawnTimer = timerValue;
            InvokeRepeating("SpawnText", 0, 0.5f);
        }

        public void Tinkerer()
        {
            scoringColour = PlayerColour.tinkererGreen;
            spawnTimer = timerValue;
            InvokeRepeating("SpawnText", 0, 0.5f);
        }

        public void Viking()
        {
            scoringColour = PlayerColour.vikingYellow;
            spawnTimer = timerValue;
            InvokeRepeating("SpawnText", 0, 0.5f);
        }

        public void Spawning()
        {
            scoringColour = PlayerColour.spawningPassengers;
            spawnTimer = timerValue;
            InvokeRepeating("SpawnText", 0, 0.5f);
        }

        /*
        public void TakesTheLead()
        {
            spawnTimer = timerValue;
        }*/
    }

}