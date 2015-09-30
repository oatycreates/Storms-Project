/**
 * File: ExplosionTrigger.cs
 * Author: RowanDonaldson
 * Maintainers: Patrick Ferguson
 * Created: 30/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: How the explosion effects player airships.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	/// <summary>
	/// Explosion trigger - on collision enter.
	/// </summary>
	public class ExplosionTrigger : MonoBehaviour 
	{

		private float scale = 0.1f;
		public float scaleSpeed = 300.0f;
		[HideInInspector]
		public float maxSize = 50;
		
		void Update () 
		{
			if (scale < maxSize)
			{
				scale += scaleSpeed * Time.deltaTime;
			}
			else
			if (scale > maxSize)
			{
				//scale = 0.1f;
				Invoke("TimeOut", 1);
			}
			
			gameObject.transform.localScale = new Vector3 (scale, scale, scale);
		}
		
		void OnTriggerEnter(Collider other)
		{
			other.attachedRigidbody.AddExplosionForce (100, gameObject.transform.position, 0, 100, ForceMode.Impulse);
		}

		void OnEnable()
		{
			scale = 0.1f;
		}

		void TimeOut()
		{
			gameObject.SetActive (false);
		}
	}
}
