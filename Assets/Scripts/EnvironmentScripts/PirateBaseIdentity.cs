/**
 * File: PirateBaseIdentity.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script keeps track of the pirate base and identity - checks current score and tags.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This script keeps track of the pirate base and identity - checks current score and tags.
    /// </summary>
    public class PirateBaseIdentity : MonoBehaviour
    {

        public DetectFallingPassenger baseTriggerZone;

        [HideInInspector]
        public int baseScore;
        [HideInInspector]
        public int teamNumber;
        [HideInInspector]
        public Color baseColour;
		[HideInInspector]
		public bool teamGame = false;
		[HideInInspector]
		public bool omegaBlack = false;

        public Renderer myRenderer;

        public Renderer flagRenderer;
        public Renderer flagBackRenderer;
        private float m_flagAlpha = 1.0f;

        public LineRenderer dropzoneLineRenderer;
        public float dropZoneAlpha = 1.0f;


        void Start()
        {
            baseColour = Color.clear;

            m_flagAlpha = flagRenderer.material.color.a;
        }

        void Update()
        {
			if (teamGame == false)
			{
	            if (gameObject.tag == "Player1_")
	            {
	                teamNumber = 1;
	                myRenderer.material.color = Color.red;
	                baseColour = myRenderer.material.color;

	            }
	            else if (gameObject.tag == "Player2_")
	            {
	                teamNumber = 2;
	                myRenderer.material.color = Color.blue;
	                baseColour = myRenderer.material.color;
	            }
	            else if (gameObject.tag == "Player3_")
	            {
	                teamNumber = 3;
	               myRenderer.material.color = Color.green;
	                baseColour = myRenderer.material.color;
	            }
	            else if (gameObject.tag == "Player4_")
	            {
	                teamNumber = 4;
	                myRenderer.material.color = Color.yellow;
	                baseColour = myRenderer.material.color;
	            }
	            else if (gameObject.tag == "Untagged")
	            {
	                Debug.Log("Four player - NO TAG!!");
	            }
			}
			else
			if (teamGame)
			{
				//Set the colour for the team base (black or white)
				if (!omegaBlack)
				{
					myRenderer.material.color = Color.red;
				}
				else
				if (omegaBlack)
				{
					myRenderer.material.color = Color.black;
				}

				baseColour = myRenderer.material.color;
			}
             

            // Set the flag's colour
            Color flagColour = baseColour;
            flagColour.a = m_flagAlpha;
            flagRenderer.material.color = flagColour;
            flagBackRenderer.material.color = flagColour;

            // Set the drop-zone's colour
            Color dropzoneColour = baseColour;
            dropzoneColour.a = dropZoneAlpha;
            dropzoneLineRenderer.SetColors(dropzoneColour, dropzoneColour);

            // Set the colour of text in DetectFallingPassengersScript
            baseTriggerZone.textColour = myRenderer.material.color;

            // Set the score for the ScoreManager script
            baseScore = baseTriggerZone.peopleLeftToCatch;
        }

        public void ResetPirateBase(int a_baseScore)
        {
            // Set score values
            baseTriggerZone.peopleLeftToCatch = a_baseScore;
            baseTriggerZone.maxPeople = a_baseScore;
            baseScore = a_baseScore;
        }
    } 
}
