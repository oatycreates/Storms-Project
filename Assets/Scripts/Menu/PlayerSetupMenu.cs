/**
 * File: PlayerSetupMenu.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 9/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Controls the behaviour of the Player Setup sub menus
 **/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
    public class PlayerSetupMenu : MonoBehaviour
    {
        [System.Serializable]
        private class Player
        {
            public bool ready       = false;
            public Faction faction  = Faction.NONE;
            public Team team        = Team.NONE;
        }

        private int playersReadyCount
        {
            get
            {
                int count = 0;

                // Iterate through array, counting which players are ready
                for (int i = 0; i < m_playersReady.Length; ++i)
                {
                    if (m_playersReady[i])
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        // References to child objects
        [Header("References")]
        public GameObject startGameMenu;
        public GameObject playerSubmenus;
        public GameObject backButton;
        public Button[] navyButtons         = new Button[4];
        public Button[] piratesButtons      = new Button[4];
        public Button[] tinkerersButtons    = new Button[4];
        public Button[] vikingsButtons      = new Button[4];

        [Header("Countdown")]
        public Text countdownText;
        public int countdownTime = 5;

        // Countdown variables
        private float m_currentCountdownTime    = 0.0f;
        private bool m_allPlayersReady          = false;

        // Team variables
        private Faction m_team1Faction;
        private Faction m_team2Faction;

        private bool[] m_playersReady = new bool[4];

        // Level script references
        private MainMenu m_mainMenu;

        public void Awake()
        {
            m_mainMenu = FindObjectOfType<MainMenu>();

            // Ensure coundown text reference and Start Game Menu
            // isn't null
            if (countdownText == null)
            {
                Debug.LogError("Countdown text reference not set!");
            }
            if (startGameMenu == null)
            {
                Debug.LogError("Start Game Menu reference not set!");
            }
            if (playerSubmenus == null)
            {
                Debug.LogError("Player Submenu reference not set!");
            }
            if (backButton == null)
            {
                Debug.LogError("Back button reference not set!");
            }
        }

        private void Update()
        {
            if (m_allPlayersReady)
            {
                UpdateCountDown();
            }
        }

        public void ResetButtons()
        {
            EnableAllTeamButtons();

            m_team1Faction = Faction.NONE;
            m_team2Faction = Faction.NONE;
        }

        private void UpdateCountDown()
        {
            if (m_currentCountdownTime >= (float)countdownTime)
            {
                m_mainMenu.StartMatch();
                //Debug.Log("Match starting! (not implemented)");
            }
            else
            {
                // Tick timer
                m_currentCountdownTime += Time.deltaTime;

                // Update display
                int currentTime = countdownTime - (int)m_currentCountdownTime;
                countdownText.text = currentTime.ToString();
            }
        }

        private void ShowStartGameMenu()
        {
            // Flag players as ready
            m_allPlayersReady = true;
            
            // Hide player sub menus and show start game menu
            startGameMenu.SetActive(true);
            playerSubmenus.SetActive(false);
            backButton.SetActive(false);
        }

        public void CancelStartGame()
        {
            // Set menus
            startGameMenu.SetActive(false);
            playerSubmenus.SetActive(true);

            // Reset timer and unflag players as ready
            m_currentCountdownTime  = 0.0f;
            m_allPlayersReady       = false;

            // Reset player preferences
            for (int i = 0; i < m_playersReady.Length; ++i)
            {
                m_playersReady[i] = false;
            }

            for (int i = 1; i < 5; ++i)
            {
                LevelSettings.Instance.ResetPlayer(i);
            }
        }

        private Faction GetFaction(string a_faction)
        {
            switch (a_faction)
            {
                case "Navy":
                    return Faction.NAVY;

                case "Pirates":
                    return Faction.PIRATES;

                case "Tinkerers":
                    return Faction.TINKERERS;

                case "Vikings":
                    return Faction.VIKINGS;

                default:
                    Debug.LogError("Invalid faction!");
                    return Faction.NONE;
            }
        }

        private void ReadyPlayer(int a_id, Faction a_faction)
        {
            // Ensure arguments are valid
            if (a_id < 1 ||
                a_id > 4)
            {
                Debug.LogError("Invalid player!");
                return;
            }
            else if (a_faction == Faction.NONE)
            {
                Debug.LogError("Invalid faction!");
                return;
            }

            PlayerSettings playerSettings = new PlayerSettings();

            // Set player preferences, and flag
            // player as ready
            m_playersReady[a_id - 1]    = true;
            playerSettings.playing      = true;
            playerSettings.faction      = a_faction;

            if (m_mainMenu.isTeamsGameMode)
            {
                // Set Team 1's faction
                if (m_team1Faction == Faction.NONE &&
                    playerSettings.faction != m_team2Faction)
                {
                    m_team1Faction = a_faction;
                }
                // Set Team 2's faction
                else if (m_team2Faction == Faction.NONE &&
                         playerSettings.faction != m_team1Faction)
                {
                    m_team2Faction = a_faction;
                }

                if (playersReadyCount == 3 &&
                    m_team1Faction != Faction.NONE)
                {
                    // Special case where three players are ready
                    // and on the same team, last player shouldn't
                    // be able to join that team

                    Button[] teamOneButtons = null;
                    switch (m_team1Faction)
                    {
                        case Faction.NAVY:
                            teamOneButtons = navyButtons;
                            break;

                        case Faction.PIRATES:
                            teamOneButtons = piratesButtons;
                            break;

                        case Faction.TINKERERS:
                            teamOneButtons = tinkerersButtons;
                            break;

                        case Faction.VIKINGS:
                            teamOneButtons = vikingsButtons;
                            break;

                        case Faction.NONE:
                            Debug.LogError("Team 1's faction set to NONE, when expect not NONE");
                            break;
                    }

                    for (int i = 0; i < teamOneButtons.Length; ++i)
                    {
                        teamOneButtons[i].interactable = false;
                    }
                }
                else
                {
                    // Ensure once both team's factions are set
                    // that remaining players can only select these factions
                    DisableNonTeamFactions();
                }
            }

            // Set player's faction
            if (a_faction == m_team1Faction)
            {
                playerSettings.team = Team.ALPHA;
            }
            else if (a_faction == m_team2Faction)
            {
                playerSettings.team = Team.OMEGA;
            }

            // Show Start Game menu if all players
            // are ready
            if (m_playersReady[0] &&
                m_playersReady[1] &&
                m_playersReady[2] &&
                m_playersReady[3])
            {
                ShowStartGameMenu();
            }

            // Update player settings
            LevelSettings.Instance.SetPlayerSettings(a_id, playerSettings);

            Debug.Log(string.Format("Player {0} ready with faction: {1}", a_id, a_faction));
        }

        private void DisableNonTeamFactions()
        {
            if (m_team1Faction == Faction.NONE ||
                m_team2Faction == Faction.NONE)
            {
                return;
            }

            // Disable all buttons so we have a clean slate
            // to work with
            for (int i = 0; i < navyButtons.Length; ++i)
            {
                navyButtons[i].interactable = false;
            }

            for (int i = 0; i < piratesButtons.Length; ++i)
            {
                piratesButtons[i].interactable = false;
            }

            for (int i = 0; i < tinkerersButtons.Length; ++i)
            {
                tinkerersButtons[i].interactable = false;
            }

            for (int i = 0; i < vikingsButtons.Length; ++i)
            {
                vikingsButtons[i].interactable = false;
            }

            // Enable only team one's faction buttons
            Button[] teamOne = null;
            switch (m_team1Faction)
            {
                case Faction.NAVY:
                    teamOne = navyButtons;
                    break;

                case Faction.PIRATES:
                    teamOne = piratesButtons;
                    break;

                case Faction.TINKERERS:
                    teamOne = tinkerersButtons;
                    break;

                case Faction.VIKINGS:
                    teamOne = vikingsButtons;
                    break;

                case Faction.NONE:
                    Debug.LogWarning("Team one faction not set during DisableNonTeamFactions() call");
                    break;
            }

            for (int i = 0; i < teamOne.Length; ++i)
            {
                teamOne[i].interactable = true;
            }

            // Enable only team two's buttons
            Button[] teamTwo = null;
            switch (m_team2Faction)
            {
                case Faction.NAVY:
                    teamTwo = navyButtons;
                    break;

                case Faction.PIRATES:
                    teamTwo = piratesButtons;
                    break;

                case Faction.TINKERERS:
                    teamTwo = tinkerersButtons;
                    break; ;

                case Faction.VIKINGS:
                    teamTwo = vikingsButtons;
                    break;

                case Faction.NONE:
                    Debug.LogWarning("Team two faction not set during DisableNonTeamFactions() call");
                    break;
            }

            for (int i = 0; i < teamTwo.Length; ++i)
            {
                teamTwo[i].interactable = true;
            }
        }

        private void EnableAllTeamButtons()
        {
            for (int i = 0; i < navyButtons.Length; ++i)
            {
                navyButtons[i].interactable = true;
            }

            for (int i = 0; i < piratesButtons.Length; ++i)
            {
                piratesButtons[i].interactable = true;
            }

            for (int i = 0; i < tinkerersButtons.Length; ++i)
            {
                tinkerersButtons[i].interactable = true;
            }

            for (int i = 0; i < vikingsButtons.Length; ++i)
            {
                vikingsButtons[i].interactable = true;
            }
        }

        public void ReadyPlayer1(string a_faction)
        {
            Faction faction = GetFaction(a_faction);
            ReadyPlayer(1, faction);
        }

        public void ReadyPlayer2(string a_faction)
        {
            Faction faction = GetFaction(a_faction);
            ReadyPlayer(2, faction);
        }

        public void ReadyPlayer3(string a_faction)
        {
            Faction faction = GetFaction(a_faction);
            ReadyPlayer(3, faction);
        }

        public void ReadyPlayer4(string a_faction)
        {
            Faction faction = GetFaction(a_faction);
            ReadyPlayer(4, faction);
        }

        public void UnreadyPlayer(int a_id)
        {
            // Ensure arguments are valid
            if (a_id < 1 ||
                a_id > 4)
            {
                Debug.LogError("Invalid player!");
                return;
            }

            PlayerSettings playerSettings = LevelSettings.Instance.GetPlayerSettings(a_id);

            // Renable all factions if only one player is ready
            if (playersReadyCount == 2)
            {
                EnableAllTeamButtons();

                if (playerSettings.faction == m_team1Faction)
                {
                    m_team1Faction = Faction.NONE;
                }
                else if (playerSettings.faction == m_team2Faction)
                {
                    m_team2Faction = Faction.NONE;
                }
            }
            else if (playersReadyCount < 2)
            {
                if (playerSettings.faction == m_team1Faction)
                {
                    m_team1Faction = Faction.NONE;
                }
                else if (playerSettings.faction == m_team2Faction)
                {
                    m_team2Faction = Faction.NONE;
                }
            }
            else if (playersReadyCount == 3 && 
                     m_team2Faction == Faction.NONE)
            {
                EnableAllTeamButtons();
            }

            // Reset player preferences,
            // and unready player
            m_playersReady[a_id - 1] = false;
            LevelSettings.Instance.ResetPlayer(a_id);

            Debug.Log(string.Format("Player {0} unready", a_id));
        }
    }
}
