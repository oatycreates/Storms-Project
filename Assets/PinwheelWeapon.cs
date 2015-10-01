/**
 * File: Pinwheel Weapon.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 29/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Behaviour for rotating pinwheel weapon.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	/// <summary>
	/// Pinwheel weapon - spirals in local axis
	/// </summary>
	public class PinwheelWeapon : MonoBehaviour 
	{
		public float rotate = 1;

		public GameObject blade1;
		public GameObject blade2;


		void Start () 
		{
			//Seperate ();
			if (blade1.GetComponent<PinwheelBlade>() == null)
			{
				blade1.AddComponent<PinwheelBlade>();
			}

			if (blade2.GetComponent<PinwheelBlade>() == null)
			{
				blade2.AddComponent<PinwheelBlade>();
			}
		}

		void OnEnabled()
		{

		}

		private float t = 0.0f;
		public float amplitude = 50;
		public float speedMod = 5;

		void FixedUpdate () 
		{

			Vector3 position = new Vector3 ();

			position.x = amplitude * Mathf.Cos (t);
			//position.z = speed * Time.deltaTime;

			t += Time.deltaTime * speedMod;

			gameObject.transform.position = position;

			//COme back to this.
			gameObject.transform.Rotate (Vector3.up * Time.deltaTime * rotate* 10, Space.Self);
		}


	}
}
