/**
 * File: PassengerDestroyScript.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Kills players beneath kill-Y.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This script deactivates the Pirate Passengers as soon as they go below a certain height. The name of this script is a misnomer.
/// </summary>
public class PassengerDestroyScript : MonoBehaviour
{
    /// <summary>
    /// Kill-Y.
    /// </summary>
	public float heightTillDeath = -2000.0f;
	private FallingScream m_scream;
	
	void Start()
	{
        m_scream = gameObject.GetComponent<FallingScream>();
	}
	
	void Update () 
	{
		if (gameObject.activeInHierarchy)
		{
			if (gameObject.transform.position.y < heightTillDeath)
			{
                // Check timeout
				gameObject.SetActive(false);
				
				// Reset scream
                if (m_scream != null)
				{
                    m_scream.readyToScream = true;
				}
				
			}
		}
	}
}
