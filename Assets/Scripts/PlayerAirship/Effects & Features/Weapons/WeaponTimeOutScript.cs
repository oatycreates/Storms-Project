/**
 * File: WeaponTimeOutScript.cs
 * Author: Patrick Ferguson
 * Maintainers: 
 * Created: 24/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Dpad slowing field weapon.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	/// <summary>
	/// Removes the weapon/countermeasure from the scene after a certain time.
	/// </summary>
	public class WeaponTimeOutScript : MonoBehaviour 
	{
		private float internalTimer = 1;
		public float secondsBeforeTimeout = 60.0f;

		private AudioSource optionalAudio;
		
		void Awake()
		{
			if (gameObject.GetComponent<AudioSource>() != null)
			{
				optionalAudio = gameObject.GetComponent<AudioSource>();
			}
		}
		
		void OnEnable()
		{
			internalTimer = secondsBeforeTimeout;

		}

		void  Update()
		{
		
			internalTimer -= Time.deltaTime;

			//Check audio status
			if (optionalAudio != null)
			{
				if (internalTimer < 3 && internalTimer > 0)
				{
					FadeOutNoise();
				}
			}
			
			//print("InternalTimer " + internalTimer + " Volume " + optionalAudio.volume);
		

			if (internalTimer <= 0) 
			{
                gameObject.SetActive(false);

			}
		}
		
		void FadeOutNoise()
		{
			if (optionalAudio.volume > 0)
			{
				//optionalAudio.volume -= 0.01f;
				optionalAudio.volume = Mathf.Lerp(optionalAudio.volume, 0, Time.deltaTime);
			}
		}
	}
}
