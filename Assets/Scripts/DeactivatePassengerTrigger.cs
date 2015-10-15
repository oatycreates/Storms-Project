/**
 * File: PassengerDeactivateTrigger.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 7/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script deactivates the pirate passengers as soon as they enter my trigger.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class DeactivatePassengerTrigger : MonoBehaviour 
	{
	
		void OnTriggerEnter(Collider other)
		{
		//Check for components that are only on the falling prisoners.
			if (other.gameObject.GetComponent<FallingScream>() != null)
			{
				//print("Kill Passenger");
				other.gameObject.SetActive(false);
			}
		}
	}
}
