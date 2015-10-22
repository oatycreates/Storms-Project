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
		public float rotate = 75;

		private float t = 0.0f;
		public float maxAmplitude = 50;
		public float speedMod = 5;
		private float amplitude = 0;
		private float amplitudeSpeedup = 3.0f;


		private Vector3 localPos;
		public bool zigzag = false;

     

		void Start () 
		{
			localPos = gameObject.transform.localPosition;
		}

		void OnEnabled()
		{
			//reset pos
			gameObject.transform.localPosition = localPos;
			amplitude = 0;

		}


		void FixedUpdate () 
		{
			amplitude = Mathf.Clamp (amplitude, 0, maxAmplitude);

			//amplitude += Time.deltaTime * amplitudeSpeedup;
			amplitude = Mathf.Lerp (amplitude, maxAmplitude, Time.deltaTime / amplitudeSpeedup);

			//print (amplitude);

			if (zigzag)
			{
				Vector3 localPosition = new Vector3 ();

				localPosition.x = amplitude * Mathf.Cos (t);

				t += Time.deltaTime * speedMod;

				gameObject.transform.localPosition = localPosition;
			}

			//Come back to this.
			gameObject.transform.Rotate (Vector3.up * Time.deltaTime * rotate* 10, Space.Self);
		}
	}
}
