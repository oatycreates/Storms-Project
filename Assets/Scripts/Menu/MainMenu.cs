/**
 * File: MainMenu.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 25/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: 
 **/

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class MainMenu : MonoBehaviour 
	{
		void Start() 
		{
			
		}
		
		void Update() 
		{
			
		}

        public void ExitGame()
        {
            Application.Quit();

#if UNITY_EDITOR
            // Stop editor playing, to simulate game exiting in editor
            EditorApplication.ExecuteMenuItem("Edit/Play");
            return;
#endif
        }

        public void StartMatch(string a_level)
        {
            // TODO: Use a loading screen
            
            Application.LoadLevel(a_level);
        }
	}
}
