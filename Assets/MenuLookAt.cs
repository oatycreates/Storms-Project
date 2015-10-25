/**
 * File: MenuLookAt.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 25/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Makes objects in the menu look at the camera.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class MenuLookAt : MonoBehaviour
	{
		public GameObject creditsCamera;
		public float turnSpeed = 3;
		
		void Update () 
		{
			if (creditsCamera != null)
			{
				Vector3 targetDir = creditsCamera.transform.position - transform.position;
				float step = turnSpeed * Time.deltaTime;
				Vector3 newDir = Vector3.RotateTowards(transform.forward, -targetDir, step, 0.0F);
				Debug.DrawRay(transform.position, newDir, Color.red);
				transform.rotation = Quaternion.LookRotation(newDir);
			}
			else
			if (creditsCamera == null)
			{
				Debug.Log("No credits camera assigned");
			}
		}
	}
}
