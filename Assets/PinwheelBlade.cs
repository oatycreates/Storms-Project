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
	/// Pinwheel blade trigger. Triggers a physical response if any rigidbody enters its collider	
	/// </summary>
	public class PinwheelBlade : MonoBehaviour 
	{
		public float pinwheelForce = 100;

		void OnTriggerEnter(Collider other)
		{
			Rigidbody anotherRigidbody = other.gameObject.transform.root.gameObject.GetComponent<Rigidbody> ();

			if (anotherRigidbody != null)
			{
				anotherRigidbody.AddRelativeTorque(new Vector3 (0, -pinwheelForce, 0), ForceMode.Acceleration);
				anotherRigidbody.AddForce(Vector3.down * pinwheelForce*2, ForceMode.Acceleration);
			}
		}
	}
}
