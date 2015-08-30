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
       	
       	public float transitionTimer = 1.0f;
       	private float startTimerValue;

        void Awake()
        {
            m_trans = transform; //my Transform
            
			shot = EShotLength.LongShot;
			
			//Take a reference value.
			startTimerValue = transitionTimer;
        }

        void Update()
        {
        	//Count down timer
        	transitionTimer -= Time.deltaTime;
        	
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
            }
            else
            if (shot == EShotLength.MidShot)
            {
				cinematicCam.transform.localPosition = new Vector3(0, 0, -(Mathf.Abs(midDistance)));
				
				//twirlSpeed = -Mathf.Abs(twirlSpeed);	//netative direction ?? Maybe a good effect
				twirlSpeed = Mathf.Abs(twirlSpeed);	//positive direction
            }
            else
            if (shot == EShotLength.CloseUp)
            {
				cinematicCam.transform.localPosition = new Vector3(0, 0, -(Mathf.Abs(closeDistance)));
				twirlSpeed = Mathf.Abs(twirlSpeed);	//positive direction
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
            
            //Hacks
            if (Application.isEditor)
            {
            	if (Input.GetKeyDown(KeyCode.Space))
            	{
            		if (shot != EShotLength.CloseUp)
            		{
            			shot += 1;
            		}
            		else
            		{
            			shot = 0;
            		}
            		
            		//reset timer
					transitionTimer = startTimerValue;
            	}

            }
        }
     
    } 
}
