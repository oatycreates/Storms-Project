/**
 * File: PressAnyButton.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Skip to next scene from game menu.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// Skip to next scene from game menu.
/// </summary>
public class PressAnyButton : MonoBehaviour 
{
	public FadeCamWhite fader;

	void Update()
	{
		if (Input.anyKeyDown)
		{
			// Fade out to next scene
			fader.fadeStart = false;
			fader.fadeEnd = true;
		}
	}
}
