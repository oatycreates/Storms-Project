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
    //public enum MessageType { playerWinning, playerScoring, playerScoringScore, playerHowManyMoreToGo, playerHasWonGame, playerHasManyPassengers, playerIsBeingMissileTracked, playerIsBehindOtherPlayer, passengersAreSpawning }; 

    public class AnnouncerText : MonoBehaviour
    {
        private Text m_text;
       
        private float lifeTime = 1.0f;
        private float fadeMod = 2.0f;


        [HideInInspector]
        public string m_myWords;
        [HideInInspector]
        public Color m_myColour = Color.clear;

        void Start()
        {
            m_text = gameObject.GetComponent<Text>();
            m_text.color = m_myColour;

            m_myWords = "Score!";
        }

        void OnEnable()
        {
            //Reset life time on enable
            lifeTime = 1.0f;

           
        }
       
	
        void Update()
        {
            //Set colour
            m_text.color = m_myColour;
            m_text.text = m_myWords;

            lifeTime -= (Time.deltaTime)/fadeMod;
            //lifeTime -= Time.deltaTime;

            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, lifeTime);
            
            //Move?
           // gameObject.transform.position = Vector3.Slerp(gameObject.transform.position, gameObject.transform.parent.transform.position, Time.deltaTime * 1.5f);
			gameObject.transform.position = Vector3.Lerp (gameObject.transform.position, transform.parent.transform.position, Time.deltaTime * 1.0f);
			
            if (lifeTime < 0)
            {
                //Destroy(gameObject);
                gameObject.SetActive(false);
            }
        }

        public void SetColour(Color textColour)
        {
            m_myColour = textColour;
        }
    }
}
