/**
* File: AnnouncerText.cs
* Author: RowanDonaldson
* Maintainers: Patrick Ferguson
* Created: 15/10/2015
* Copyright: (c) 2015 Team Storms, All Rights Reserved.
* Description: Behaviour for the Text/Screen Effects.
**/
		
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ProjectStorms
{
    public enum MessageType { scoring, warning, winning};

    public class AnnouncerText : MonoBehaviour
    {
        public Text m_text;
        private float lifeTime = 1.0f;
        private float fadeMod = 2.0f;



        public MessageType messageType;

        public List<string> scoringMessages = new List<string>();
        public List<string> warningMessages = new List<string>();
        public List<string> winningMessages = new List<string>();

        private Color m_myColour;

        void Awake()
        {
            m_text = gameObject.GetComponent<Text>();
            m_text.color = Color.clear;
        }

        void OnEnable()
        {

            if (messageType == MessageType.scoring)
            {
                m_text.text = scoringMessages[Random.Range(0, scoringMessages.Count - 1)];
            }
            else
            if (messageType == MessageType.warning)
            {
                m_text.text = warningMessages[Random.Range(0, warningMessages.Count - 1)];
            }
            else
            if (messageType == MessageType.winning)
            {
                m_text.text = winningMessages[Random.Range(0, winningMessages.Count - 1)];
            }
           
           
        }

        void Update()
        {
            //Set colour
            m_text.color = m_myColour;

            lifeTime -= (Time.deltaTime)/fadeMod;
            //lifeTime -= Time.deltaTime;

            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, lifeTime);

            if (lifeTime < 0)
            {
                Destroy(gameObject);
            }
        }

        public void SetColour(Color textColour)
        {
            m_myColour = textColour;
        }
    }
}
