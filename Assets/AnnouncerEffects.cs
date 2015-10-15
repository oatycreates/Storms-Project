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
    public enum PlayerColour { pirateRed, navyWhite, tinkererGreen, vikingYellow};

    public class AnnouncerEffects : MonoBehaviour
    {
        public GameObject textPrefab;
        private Vector3 randomPos;

        public PlayerColour scoringColour;

        private Color textColour;
        private Color m_pirateRed = new Color(0.779f, 0.126f, 0.126f);
        private Color m_navyWhite = new Color(1f, 1f, 1f);
        private Color m_tinkererGreen = new Color(0.145f, 0.588f, 0.108f);
        private Color m_vikingYellow = new Color(0.926f, 0.842f, 0.163f);


        //Spawntimer
        private float spawnTimer = 0.0f;
        public float timerValue = 1.5f;

        void Start()
        {

            //InvokeRepeating("SpawnText", 1, 1);
        }

        void Update()
        {
            if (spawnTimer >0)
            {
                spawnTimer -= Time.deltaTime;

                SpawnText();
            }

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
        }

       public void SpawnText()
        {
            RandomPos();

           GameObject go = Instantiate(textPrefab, gameObject.transform.position + randomPos, Quaternion.identity) as GameObject;
           go.transform.SetParent(gameObject.transform);
          // go.gameObject.GetComponent<AnnouncerText>().SetColour(playerTeamColour);
           go.gameObject.GetComponent<AnnouncerText>().SetColour(textColour);
        }

        void RandomPos()
        {
            float height = Screen.height / 2;
            float width = Screen.width / 2;

            randomPos = new Vector3(Random.Range(-width, width), Random.Range(-height, height), 0);
        }


        public void Pirate()
        {
            scoringColour = PlayerColour.pirateRed;
            spawnTimer = timerValue;
        }

        public void Navy()
        {
            scoringColour = PlayerColour.navyWhite;
            spawnTimer = timerValue;
        }

        public void Tinkerer()
        {
            scoringColour = PlayerColour.tinkererGreen;
            spawnTimer = timerValue;
        }

        public void Viking()
        {
            scoringColour = PlayerColour.vikingYellow;
            spawnTimer = timerValue;
        }
    }

}