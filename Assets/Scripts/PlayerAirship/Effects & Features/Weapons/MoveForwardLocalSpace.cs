/**
 * File: MoveForwardLocalSpace.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 01/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Moves Gameobject forward in local space.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	/// <summary>
	/// Moves Gameobject forward in local space. Specifically, this is used on the Root parent of the Pinwheel object.	
	/// </summary>
	public class MoveForwardLocalSpace : MonoBehaviour 
	{
		public float speed = 5.0f;
		private float startSpeed;
		private bool shouldIDecelerate = false;
		public float decelerationFactor = 2.0f;
		//public float slowDownDelay = 5;

		void Awake()
		{
			startSpeed = speed;
		}

		void OnEnable()
		{
			//Invoke ("SlowDown", slowDownDelay);

			//Reset Speed
			speed = startSpeed;
		}

		void Update () 
		{
			//speed = Mathf.Clamp (speed, 0, startSpeed);

			gameObject.transform.Translate (Vector3.forward * speed * Time.deltaTime, Space.Self);

			//print(speed);

			//if (shouldIDecelerate)
			//{
				speed = Mathf.Lerp(speed, 0, Time.deltaTime*decelerationFactor);
			//}
		}
		/*
		void SlowDown()
		{
			//shouldIDecelerate = true;
		}*/
	}
}
