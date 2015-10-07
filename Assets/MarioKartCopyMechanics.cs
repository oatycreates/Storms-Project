/**
* File: MarioKartLikeMechanics.cs
	* Author: RowanDonaldson
		* Maintainers: Patrick Ferguson
		* Created: 04/10/2015
		* Copyright: (c) 2015 Team Storms, All Rights Reserved.
		* Description: Hacks!
		**/
		
using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class MarioKartCopyMechanics : MonoBehaviour 
	{
		public AirshipControlBehaviour[] overrideControls;
		public float speed = 400;
		public float leftRightYaw = 5000;
	
		void Awake () 
		{
			foreach (AirshipControlBehaviour airship in overrideControls)
			{
				airship.generalSpeed = speed;
				airship.yawForce = 5000;
			}
		}
		
	}
}
