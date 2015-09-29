/**
 * File: ScaleOnSpawn.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 29/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Scales a weapon object up, once it's spawned.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	/// <summary>
	/// Scale on spawn. Starts object at local scale 0.1f and scales up to local scale 1.
	/// </summary>
	public class ScaleOnSpawn : MonoBehaviour 
	{
		private float scaleFactor = 1;
		public float maxScaleSize = 1;
		
		void OnEnable () 
		{
			scaleFactor = 0.01f;
		}
		
		void Update () 
		{
			
			scaleFactor = Mathf.Clamp (scaleFactor, 0, maxScaleSize);
			
			if (scaleFactor < maxScaleSize)
			{
				scaleFactor += 1 * Time.deltaTime;
			}
			
			gameObject.transform.localScale = new Vector3 (scaleFactor, scaleFactor, 1);
		}
	}
}
