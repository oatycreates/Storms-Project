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

namespace ProjectStorms
{
    /// <summary>
    /// This script changes the current scene, level, or room.
    /// </summary>
    public class SceneManager : MonoBehaviour
    {
        void Update()
        {
            // Reset the level - Press Both Start AND Select
			if ((Input.GetButton("Player1_Start") && Input.GetButton("Player1_Select")) || (Input.GetButton("Player2_Start") && Input.GetButton("Player2_Select")) || (Input.GetButton("Player3_Start") && Input.GetButton("Player3_Select")) || (Input.GetButton("Player4_Start") && Input.GetButton("Player4_Select")) )
			{
				LoopCurrentLevel();
			}

            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                //Application.LoadLevel(Application.loadedLevelName);
				LoopCurrentLevel();
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

        public void LoopCurrentLevel()
        {
            Application.LoadLevel(Application.loadedLevelName);
        }

        public void TestScene()
        {
            Application.LoadLevel("RoDoTestScene");
        }

		public void CoOpTeamMatch()
		{
			Application.LoadLevel ("TwoTeamMapPrototype");
		}
		                       
    } 
}
