/**
 * File: AirshipCamBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the camera's following of each player.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// Basic lerp follow airship. Don't change the cam height/width/ pixel position here - do that in the master cam controller.
/// However, we can tell the camera where to go and what positiont to take here.
/// </summary>
public class AirshipCamBehaviour : MonoBehaviour
{
	[HideInInspector]
	public bool camFollowPlayer = true;

	public GameObject camPosTarget;
	public GameObject camLookTarget;
	
	public float camSpeed = 5.0f;
	
	/// <summary>
    /// Keep a reference to the start position, so we can reset to the roulette position.
	/// </summary>
	private Vector3 m_myStartPos;
	private Quaternion m_myStartRot;

	void Start () 
	{
        // Detach from parent on start!
		gameObject.transform.parent = null;
		
		m_myStartPos = gameObject.transform.position;
		m_myStartRot = gameObject.transform.rotation;
	}
	
	void Update () 
	{
		if (camFollowPlayer)
		{
			FollowCam();
		}
		else
		if (!camFollowPlayer)
		{
			WatchCam();
		}
	}
	
	public void FollowCam()
	{
		if (camPosTarget != null)
		{
			gameObject.transform.rotation = Quaternion.Lerp(gameObject.transform.rotation, camPosTarget.transform.rotation, Time.deltaTime * camSpeed);
		}
		
		if (camPosTarget != null)
		{
			gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, camPosTarget.transform.position, Time.deltaTime * camSpeed);
		}
	}
	
	
	public void WatchCam()
	{
				
		if (camLookTarget != null)
		{
			gameObject.transform.LookAt(camLookTarget.transform.position);
		}
	}
	
    /// <summary>
    /// Reset the camera back for the roulette state.
    /// </summary>
	public void RouletteCam()
	{
		gameObject.transform.position = m_myStartPos;
		gameObject.transform.rotation = m_myStartRot;
	}
}
