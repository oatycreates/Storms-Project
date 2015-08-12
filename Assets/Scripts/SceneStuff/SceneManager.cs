/**
 * File: SceneManager.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the switching between scenes.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// This script changes the current scene, level, or room.
/// </summary>
public class SceneManager : MonoBehaviour
{
	void Update () 
	{
	    if (Application.isEditor == true)
	    {
	        if (Input.GetKeyDown(KeyCode.M))
	        {
		        MenuScene();
	        }
			
	        if (Input.GetKeyDown(KeyCode.G))
	        {
		        GameScene();
	        }
			
	        if (Input.GetKeyDown(KeyCode.C))
	        {
		        CreditsScene();
	        }
        }

        // Reset the level
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            Application.LoadLevel(Application.loadedLevelName);
        }
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
	
	public void MenuScene()
	{
		Application.LoadLevel("MenuScene");
	}
	
	public void GameScene()
	{
		Application.LoadLevel("GameScene");
	}
	
	public void CreditsScene()
	{
		Application.LoadLevel("CreditsScene");
	}
	
	public void SplashScreen()
	{
		Application.LoadLevel("SplashScreen");
	}
}
