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
    public class AnnouncerText : MonoBehaviour
    {

        public Text m_text;
        public float lifeTime = 1.0f;
        public float fadeMod = 1.5f;

        public List<string> happyNoise = new List<string>();

        private Color m_myColour;

        void Awake()
        {
            m_text = gameObject.GetComponent<Text>();
        }

        void OnEnable()
        {
          
           m_text.text =  happyNoise[Random.Range(0, happyNoise.Count - 1)];
           
        }

        void Update()
        {
            //Set colour
            m_text.color = m_myColour;

            lifeTime -= (Time.deltaTime)/fadeMod;

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
