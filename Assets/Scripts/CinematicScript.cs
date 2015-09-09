/**
 * File: CinematicScript.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 28/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script manages a basic camera orbiting of the scene to show off level and gameplay features.
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum EShotLength{LongShot, MidShot, CloseUp}

namespace ProjectStorms
{
    /// <summary>
    /// This script manages a basic camera orbiting of the scene to show off level and gameplay features.
    /// </summary>
    public class CinematicScript : MonoBehaviour
    {
    
    	public EShotLength shot;
    	
    	public float longDistance = 100.0f;
    	
    	public float midDistance = 50.0f;
    	
    	public float closeDistance = 10.0f;
    
        public float twirlSpeed = 20;
        
       
        // Cached variables
       	private Transform m_trans = null;
       	public GameObject cinematicCam;
       	//Let the camera focus on something, probably the trapdoor area on the prison fortress?
       	public GameObject optionalLookTarget;
       	
       	//Use the transition timer if no audio is present.
       	public float transitionTimer = 1.0f;
       	private float startTimerValue;
       	
       	//Text fade
       	public Text startText;
      	private float textAlpha = 0;
      	
      	//Audio Source
      	private AudioSource mySource;
      	private float pitch;

		//Delay the trumpet
		private bool delay = true;
		private float delayTimer = 1.5f;

		//bool to skip intro
		private bool skip = false;
       	

        void Awake()
        {
            m_trans = transform; //my Transform
            
			shot = EShotLength.LongShot;
			
			//Take a reference value.
			startTimerValue = transitionTimer;
			
			mySource = gameObject.GetComponent<AudioSource>();
			
			//Start the sound immediately?
			//mySource.Play();
			//pitch = 1f;
        }

        void Update()
        {
			//How long should the delay last Before the Trumpet Sounds
			delayTimer -= Time.deltaTime;

			if (delayTimer < 0)
			{
				if (delay)
				{
					mySource.Play();
					pitch = 1.0f;
				}

				delay = false;
			}

        	//Count down timer
        	if (mySource.clip == null)
        	{
        		transitionTimer -= Time.deltaTime;
        	}
        	
        	if (transitionTimer < 0)
        	{
        		//Go to the next state
        		if (shot != EShotLength.CloseUp)
        		{
					shot += 1;
				}
				//Reset the timer
				transitionTimer = startTimerValue;
        	}
        	
        
            // Rotate in local space
           	//m_trans.Rotate(Vector3.up, twirlSpeed * Time.deltaTime, Space.Self);	//NOOO!! Rotate in World space   	
           	m_trans.Rotate(Vector3.up, twirlSpeed * Time.deltaTime, Space.World);
            
            if (shot == EShotLength.LongShot)
            {
            	cinematicCam.transform.localPosition = new Vector3(0, 0, -(Mathf.Abs (longDistance)));
            	twirlSpeed = Mathf.Abs(twirlSpeed);	//positive direction
            	
            	//Next _ if the trumpet
				if (delay==false)
				{
	            	if (!mySource.isPlaying)
	            	{
	            		shot = EShotLength.MidShot;
	            		mySource.Play ();
	            		pitch = 1.15f;
	            	}
				}
            }
            else
            if (shot == EShotLength.MidShot)
            {
				cinematicCam.transform.localPosition = new Vector3(0, 0, -(Mathf.Abs(midDistance)));
				
				//twirlSpeed = -Mathf.Abs(twirlSpeed);	//netative direction ?? Maybe a good effect
				twirlSpeed = Mathf.Abs(twirlSpeed);	//positive direction
				
				//Next
				if (!mySource.isPlaying)
				{
					shot = EShotLength.CloseUp;
					mySource.Play ();
					pitch = 1.25f;
				}
            }
            else
            if (shot == EShotLength.CloseUp)
            {
				cinematicCam.transform.localPosition = new Vector3(0, 0, -(Mathf.Abs(closeDistance)));
				twirlSpeed = Mathf.Abs(twirlSpeed);	//positive direction
				
				//Fade in text
				textAlpha = Mathf.Lerp(textAlpha, 1, Time.deltaTime * 0.3f);

				//Skip the scene 
				Invoke("Skipping", 0.5f);

            }
            
            //Optional look target
            if (optionalLookTarget != null)
            {
            	cinematicCam.transform.LookAt(optionalLookTarget.transform.position);
            }
            else
            if (optionalLookTarget == null)
            {
            	cinematicCam.transform.LookAt(gameObject.transform.position);
            }
            
			//text - fade up
			if (startText != null)
			{
				Color myAlpha = startText.color;
				
				myAlpha.a = textAlpha;
				
				startText.color = myAlpha;	
			}
			
			//set audio pitch
			mySource.pitch = pitch;
		

			//Skip ahead

			if (InputManager.GetAnyButtonDown ("Player1_") || InputManager.GetAnyButtonDown ("Player2_") || InputManager.GetAnyButtonDown ("Player3_") || InputManager.GetAnyButtonDown ("Player4_"))
			{
				if (shot == EShotLength.CloseUp)
				{
					if (!skip)
					{
						gameObject.SetActive(false);
					}
				}
				else
				if (shot != EShotLength.CloseUp) 
				{
					if (!skip)
					{
						shot = EShotLength.CloseUp;
						skip = true;
					}
				}
			}

        }

		void Skipping()
		{
			skip = false;
		}
     
    } 
}
