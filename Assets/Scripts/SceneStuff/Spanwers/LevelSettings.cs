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

    [System.Serializable]
    public class PlayerSettings
    {
        public bool invertY = false;
        public bool playing = false;
        public Faction faction = Faction.NONE;
        public Team team = Team.NONE;
    }

    public class LevelSettings : Singleton<LevelSettings>
    {
        private PlayerSettings[] m_playerSettings;

        /// <summary>
        /// Protected constructor, to ensure this script is
        /// used as a singleton
        /// </summary>
        protected LevelSettings() {}

        public void Awake()
        {
            // Initialise player settings array
            m_playerSettings = new PlayerSettings[4];

            for (int i = 0; i < m_playerSettings.Length; ++i)
            {
                m_playerSettings[i] = new PlayerSettings();
            }
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

            // Set requested player settings
            m_playerSettings[a_playerNo - 1].faction    = a_settings.faction;
            m_playerSettings[a_playerNo - 1].invertY    = a_settings.invertY;
            m_playerSettings[a_playerNo - 1].playing    = a_settings.playing;
            m_playerSettings[a_playerNo - 1].team       = a_settings.team;
            m_playerSettings[a_playerNo - 1].playing    = a_settings.playing;
        }

        public void ResetPlayer(int a_playerNo)
        {
            // Ensure parameters are valid
            if (a_playerNo < 1 ||
                a_playerNo > 4)
            {
                Debug.LogError("Unable to reset player settings for player:" + a_playerNo);
                return;
            }

            // Reset requested player settings
            m_playerSettings[a_playerNo - 1].faction = Faction.NONE;
            m_playerSettings[a_playerNo - 1].invertY = false;
            m_playerSettings[a_playerNo - 1].playing = false;
            m_playerSettings[a_playerNo - 1].team = Team.NONE;
        }
    }
}
