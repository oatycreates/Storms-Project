/**
 * File: LevelSettings.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 15/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Allows passing of data between menus and 
 *  game levels, to setup game levels
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    public class LevelSettings : MonoBehaviour
    {
        public enum Gamemode
        {
            NONE,
            FFA,
            TEAMS
        }

        public enum Faction
        {
            NONE,
            NAVY,
            PIRATES,
            TINKERERS,
            VIKINGS,
        }

        public enum Team
        {
            NONE,
            ALPHA,
            OMEGA
        }

        public class PlayerSettings
        {
            public bool invertY     = false;
            public bool playing     = false;
            public Faction faction  = Faction.NONE;
            public Team team        = Team.NONE;
        }

        private PlayerSettings[] m_playerSettings;

        public void Awake()
        {
            // Initialise player settings array
            m_playerSettings = new PlayerSettings[4];

            // Ensure this object persists across levels
            DontDestroyOnLoad(this.gameObject);
        }

        public PlayerSettings GetPlayerSettings(int a_playerNo)
        {
            // Ensure requested player number is valid
            if (a_playerNo < 0 ||
                a_playerNo > m_playerSettings.Length)
            {
                Debug.LogError(string.Format("Unable to retrive settings for player {0}", a_playerNo));
                return null; 
            }

            return m_playerSettings[a_playerNo - 1];
        }

        public void SetPlayerSettings(int a_playerNo, PlayerSettings a_settings)
        {
            // Ensure requested player number is valid
            if (a_playerNo < 0 ||
                a_playerNo > m_playerSettings.Length)
            {
                Debug.LogError(string.Format("Unable to set settings for player {0}", a_playerNo));
                return;
            }
        }

        public void Reset()
        {
            for (int i = 0; i < m_playerSettings.Length; ++i)
            {
                m_playerSettings[i].faction = Faction.NONE;
                m_playerSettings[i].invertY = false;
                m_playerSettings[i].playing = false;
                m_playerSettings[i].team    = Team.NONE;
            }
        }
    }
}
