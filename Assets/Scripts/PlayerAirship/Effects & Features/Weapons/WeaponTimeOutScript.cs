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

		public Renderer attachedRenderer;
		private float rendererAlpha = 1;
		private float startAlphaValue;

		void Awake()
		{
			//Keep a reference of the alpha value
			if (attachedRenderer != null)
			{
				startAlphaValue = attachedRenderer.material.color.a;
			}
		}

		void OnEnable()
		{
			internalTimer = secondsBeforeTimeout;

			//Get the object's materaial alpha
			if (attachedRenderer != null)
			{
				rendererAlpha = attachedRenderer.material.color.a;
			}
		}

		void  Update()
		{
			if (internalTimer > 0) 
			{
				internalTimer -= Time.deltaTime;
			} 
			else
			if (internalTimer <= 0) 
			{
				//Go to sleep on timeout if there is no renderer
				if (attachedRenderer == null) {
					gameObject.SetActive (false);
				}
			
				//Otherwise, fade out the colour, THEN go to sleep
				if (attachedRenderer != null) 
				{
					rendererAlpha -= 0.1f * Time.deltaTime;
					attachedRenderer.material.color = new Color (attachedRenderer.material.color.r, attachedRenderer.material.color.g, attachedRenderer.material.color.b, rendererAlpha);
				
					//When invisible, go to sleep
					if (rendererAlpha <= 0)
					{
						gameObject.SetActive(false);
					}
				}
			}
		}

		void Disable()
		{
			if (attachedRenderer != null)
			{
				//reset opacity to start alpha
				attachedRenderer.material.color = new Color (attachedRenderer.material.color.r, attachedRenderer.material.color.g, attachedRenderer.material.color.b, startAlphaValue);
			}

			//print (gameObject.name + " has gone to sleep.");
		}
	}
}
