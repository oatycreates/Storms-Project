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
		
		//Use faction identifier
		private FactionIndentifier myFaction;

		public Color demoColour;

		//takeOutFlagColourstuff
		/*
        public Renderer myRenderer;

        public Renderer flagRenderer;
        public Renderer flagBackRenderer;
        private float m_flagAlpha = 1.0f;
        */

        public LineRenderer dropzoneLineRenderer;
        public float dropZoneAlpha = 1.0f;


        void Start()
        {
            baseColour = Color.clear;

            //m_flagAlpha = flagRenderer != null ? flagRenderer.material.color.a : 1.0f;
            myFaction = gameObject.GetComponent<FactionIndentifier>();
        }

        void Update()
        {        
			if (teamGame == false)
			{
				if (myFaction.faction == FactionIndentifier.Faction.PIRATES)
				{
					teamNumber = 1;
					baseColour = new Color(184, 53, 53);
				}
				else
				if (myFaction.faction == FactionIndentifier.Faction.NAVY)
				{
					teamNumber = 2;
					baseColour = new Color(18, 70, 169);
				}
				else
				if (myFaction.faction == FactionIndentifier.Faction.TINKERERS)
				{
					teamNumber = 3;
					baseColour = new Color(32, 156, 33);
				}
				else
				if (myFaction.faction == FactionIndentifier.Faction.VIKINGS)
				{
					teamNumber = 4;
					baseColour = new Color(204, 186, 35);
				}
				else
				{
					Debug.Log("No faction selected. Add Faction Indentifier.");
				}
			
				/*
                string myTag = gameObject.tag;
	            if (myTag.CompareTo("Player1_") == 0)
	            {
	                teamNumber = 1;
	                
                    if (myRenderer != null)
                    {
                        myRenderer.material.color = Color.red;
                        baseColour = myRenderer.material.color;
                    }

	            }
                else if (myTag.CompareTo("Player2_") == 0)
	            {
	                teamNumber = 2;
	                
                    if (myRenderer != null)
                    {
                        myRenderer.material.color = Color.blue;
                        baseColour = myRenderer.material.color;
                    }
	            }
                else if (myTag.CompareTo("Player3_") == 0)
	            {
	                teamNumber = 3;
	                
                    if (myRenderer != null)
                    {
                        myRenderer.material.color = Color.green;
                        baseColour = myRenderer.material.color;
                    }
	            }
                else if (myTag.CompareTo("Player4_") == 0)
	            {
	                teamNumber = 4;
	                
                    if (myRenderer != null)
                    {
                        myRenderer.material.color = Color.yellow;
                        baseColour = myRenderer.material.color;
                    }
	            }
                else if (myTag.CompareTo("Untagged") == 0)
	            {
	                Debug.Log(gameObject.name +  " player - NO TAG!!");
	            }
	            */
			}
			else
			if (teamGame)
			{
				/*
                if (myRenderer != null)
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
                }*/
			}
             

            // Set the flag's colour
            /*
            Color flagColour = baseColour;
            flagColour.a = m_flagAlpha;
            if (flagRenderer != null)
            {
                flagRenderer.material.color = flagColour;
            }
            if (flagBackRenderer != null)
            {
                flagBackRenderer.material.color = flagColour;
            }
            */

            // Set the drop-zone's colour
            Color dropzoneColour = baseColour;
            dropzoneColour.a = dropZoneAlpha;
            if (dropzoneLineRenderer != null)
            {
                dropzoneLineRenderer.SetColors(dropzoneColour, dropzoneColour);
            }
			/*
            if (myRenderer != null)
            {
                // Set the colour of text in DetectFallingPassengersScript
                baseTriggerZone.textColour = myRenderer.material.color; 
            }*/

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
