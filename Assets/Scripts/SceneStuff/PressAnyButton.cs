using UnityEngine;
using System.Collections;
//Skip to next scene from game menu.
public class PressAnyButton : MonoBehaviour 
{
	public FadeCamWhite fader;

	
	void Update()
	{
		if (Input.anyKeyDown)
		{
			//Fade out to next scene
			fader.fadeStart = false;
			fader.fadeEnd = true;
		}
	}
}
