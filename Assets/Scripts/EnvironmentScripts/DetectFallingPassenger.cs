/**
 * File: DetectFallingPassenger.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script detects falling passenger pirates that enter the triggerzone. It deactivates the passengers and adds to player score (by subtracting from regular score).
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectStorms
{
    /// <summary>
    /// This script detects falling passenger pirates that enter the triggerzone. It deactivates the passengers and adds to player score (by subtracting from regular score).
    /// </summary>
    public class DetectFallingPassenger : MonoBehaviour
    {
        public Text scoreText;
        //[HideInInspector]
        public int peopleLeftToCatch = 100;
        [HideInInspector]
        public int maxPeople = 100;

        public Color textColour = Color.white;

        private PirateBaseIdentity m_base = null;
        private AnnouncerEffects m_announcer = null;
        public string baseTag;

        void Awake()
        {
            m_base = gameObject.GetComponentInParent<PirateBaseIdentity>();

            // This will be null if the AnnouncerEffects gameobject parent is disabled
            m_announcer = GameObject.FindObjectOfType<AnnouncerEffects>();
            if (m_announcer == null)
            {
                Debug.LogError("Announcer object was null!");
            }
        }

        void Start()
        {
            textColour = Color.black;
            maxPeople = peopleLeftToCatch;
        }


        void Update()
        {
            if (peopleLeftToCatch <= 0)
            {
                peopleLeftToCatch = 0;
            }

            if (scoreText != null)
            {
                scoreText.color = textColour;
                scoreText.text = ((maxPeople - peopleLeftToCatch) + "/" + maxPeople);
            }
        }

        void OnTriggerEnter(Collider a_other)
        {
            if (a_other.tag == "Passengers")
            {
				if (peopleLeftToCatch > 0)
				{
                	peopleLeftToCatch -= 1;
				}
                a_other.gameObject.SetActive(false);

                baseTag = m_base.gameObject.tag;

                if (baseTag == "Player1_")
                {
                    //m_announcer.Pirate();
                    //m_announcer.TestText();
                }
                else if (baseTag == "Player2_")
                {
                    //m_announcer.Navy();
                    //m_announcer.TestText();
                }
                else if (baseTag == "Player3_")
                {
                    //m_announcer.Tinkerer();
                    //m_announcer.TestText();
                }
                else if (baseTag == "Player4_")
                {
                    //m_announcer.Viking();
                    //m_announcer.TestText();
                }
            }
        }
    }
}
