/**
 * File:	CustomMenuMissile.cs
 * Author: Rowan Donaldson
 * Maintainer: Pat Ferguson
 * Created: 25/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Makes Demo Missile Move in the instruction scene
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class CustomMenuMissile : MonoBehaviour 
	{
		private Vector3 startPos;
	
		void Start()
		{
			startPos = gameObject.transform.position;
			
			Invoke("Reset", 1);
		}
		
		void Update () 
		{
			gameObject.transform.Translate(Vector3.forward, Space.Self);
		}
	
		void Reset()
		{
			gameObject.transform.position = startPos;
			
			Invoke("Reset", 1);
		}
	}
}