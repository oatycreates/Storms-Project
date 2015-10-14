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
        private enum GameModes
        {
            TEAMS,
            FFA
        }

        private GameModes m_gameMode;
        private string m_mapName;
        private PlayerSetupMenu m_setupMenu;

        public void Awake()
        {
            m_setupMenu = FindObjectOfType<PlayerSetupMenu>();
        }

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

        public void SetMap(string a_name)
        {
            m_mapName = a_name;
        }

        public void SetGamemodeTeams()
        {
            m_gameMode = GameModes.TEAMS;
        }

        public void SetGamemodeFFA()
        {
            m_gameMode = GameModes.FFA;
        }

        public void StartMatch()
        {
            // TODO: Load map with specified settings
        }
	}
}
