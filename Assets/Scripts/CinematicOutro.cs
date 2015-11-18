/**
 * File: CinematicScript.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 02/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script controls the 'Cinematic' and camera that is triggered at the end of a gameplay match.
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectStorms
{
    public class CinematicOutro : MonoBehaviour
    {
        public GameObject rotator;
        public GameObject pirate;
       // private Renderer pirateRenderer;
       // public Text winText;
        //public Color myColour = Color.white;

        private GameObject m_minimap = null;
        
        //Haha new idea
        public GameObject pirateWin;
        public GameObject navyWin;
        public GameObject tinkerersWin;
        public GameObject vikingWin;
        
        //Okay, so we actualy need the win text
        public Text winnerText;
        private string words = "Hello hero";
        private Color colour;
        
        private Color myRed = new Color(0.831f, 0.110f, 0.110f, 1f);
        private Color myNavy = new Color(0.193f, 0.329f, 0.728f, 1f);
        private Color myGreen = new Color(0.274f, 0.662f, 0.088f, 1f);
        private Color myYellow = new Color(0.847f, 0.919f, 0.169f, 1f);
        
        //Last minute win sounds
        private AudioSource my_Audio;
        public AudioClip winSound;

        void Awake()
        {
            //pirateRenderer = pirate.GetComponent<Renderer>();

            //Go to sleep untill I'm needed
            gameObject.SetActive(false);
            
            pirateWin.SetActive(false);
           	navyWin.SetActive(false);
           	tinkerersWin.SetActive(false);
           	vikingWin.SetActive(false);
           	
			my_Audio = gameObject.GetComponent<AudioSource>();
			my_Audio.volume = 0;
			my_Audio.pitch = 0.5f;
        }

        void Start()
        {
            m_minimap = GameObject.Find("Minimap");
            
           my_Audio.clip = winSound;
           my_Audio.Play();

        }
        
        void Update()
        {
        	my_Audio.volume += 0.05f;
        }

        void FixedUpdate()
        {
        	winnerText.text = words;
        	winnerText.color = colour;
        
            rotator.transform.Rotate(Vector3.down * Time.deltaTime * 60);
        }
        
        /*
        public void WinCam(string winnerNumberText, Color winnerColour)
        {
            winText.text = winnerNumberText;
            winText.color = winnerColour;
            //pirateRenderer.material.color = winnerColour;
            print (winnerNumberText);

            // Disable the minimap
            if (m_minimap != null)
            {
                m_minimap.SetActive(false);
            }
        }*/
        
        public void NewWinCam(string factionName)
        {
			if (factionName == "NONAME")
			{
				Debug.Log("error - no faction name set");
			}
			
			if (factionName == null)
			{
				Debug.Log("error - no faction name set");
			}
			
			if (factionName == "PIRATES")
			{
				words = (factionName + "\nWIN!");
				colour = myRed;
				pirateWin.SetActive(true);
			}
			
			if (factionName == "NAVY")
			{
				words = (factionName + "\nWINS!");
				colour = myNavy;
				navyWin.SetActive(true);
			}
			
			if (factionName == "TINKERERS")
			{
				words = (factionName + "\nWIN!");
				colour = myGreen;
				tinkerersWin.SetActive(true);
			}
			
			if (factionName == "VIKINGS")
			{
				words = (factionName + "\nWIN!");
				colour = myYellow;
				vikingWin.SetActive(true);
			}
        
			// Disable the minimap
			if (m_minimap != null)
			{
				m_minimap.SetActive(false);
			}
        }
    }
}
