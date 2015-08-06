﻿/**
 * File: TagChildren.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Simple script to pass GameObject tags onto the children of this object.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This script will pass itself through EVERY child object of the Airship. This can be useful for collisions, inputs and cam references. 
/// I.e. The Cannonball collisions will check for the object's tag. If all objects, have the same tag as parent, then there is no confusion.
/// </summary>
public class TagChildren : MonoBehaviour
{
	void Start()
	{
		foreach (Transform child in gameObject.transform)
		{
            // Make the child tag == my tag
			child.gameObject.tag = gameObject.tag;
            // Recurse
			child.gameObject.AddComponent<TagChildren>();
		}
	}
}
