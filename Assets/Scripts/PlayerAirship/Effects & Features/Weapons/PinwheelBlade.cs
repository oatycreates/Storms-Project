/**
 * File: Pinwheel Blade.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 01/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Triggers a physical response if any rigidbody enters its collider.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	/// <summary>
	/// Pinwheel blade trigger. Triggers a physical response if any rigidbody enters its collider (after a brief interval)
	/// </summary>
	public class PinwheelBlade : MonoBehaviour 
	{
		public float pinwheelForce = 100;
		private bool delayFinished = false;

		void OnEnable()
		{
			//Make sure the colliders don't have an impact for the first few updates after pinwheel is spawned
			delayFinished = false;
			Invoke ("DelayOver", 0.5f);
		}


		void OnDisable()
		{
			delayFinished = false;
			//print ("reset_____________");
		}

		void DelayOver()
		{
			delayFinished = true;
			//print ("READY");
		}

		void OnTriggerEnter(Collider other)
		{
			Rigidbody anotherRigidbody = other.gameObject.transform.root.gameObject.GetComponent<Rigidbody> ();

			//Has the delay finished?
			if (delayFinished)
			{
				if (anotherRigidbody != null)
				{
					anotherRigidbody.AddRelativeTorque(new Vector3 (0, -pinwheelForce, 0), ForceMode.Acceleration);
					anotherRigidbody.AddForce(Vector3.up * pinwheelForce*2, ForceMode.Acceleration);
				}
			}
		}
	}
}
