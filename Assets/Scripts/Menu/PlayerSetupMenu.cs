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

namespace ProjectStorms
{
    public class PlayerSetupMenu : MonoBehaviour
    {
        public enum Faction
        {
            NONE,
            NAVY,
            PIRATES,
            TINKERERS,
            VIKINGS,
        }

        [System.Serializable]
        private class Player
        {
            public bool ready;
            public Faction faction;
        }

        [Header("Countdown")]
        public GameObject startGameMenu;
        public GameObject playerSubmenus;
        public Text countdownText;
        public int countdownTime = 5;

        private float m_currentCountDownTime    = 0.0f;
        private bool m_playersReady             = false;

        private MainMenu m_mainMenu;

        // Player states
        private Player[] m_players;

        public void Awake()
        {
            m_mainMenu = GameObject.FindObjectOfType<MainMenu>();

            // Initialise array for storing player settings
            m_players = new Player[4];

            for (int i = 0; i < m_players.Length; ++i)
            {
                m_players[i]            = new Player();
                m_players[i].ready      = false;
                m_players[i].faction    = Faction.NONE;
            }

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
        }

        private void Start()
        {

        }

        private void Update()
        {
            if (m_playersReady)
            {
                UpdateCountDown();
            }
        }

        private void UpdateCountDown()
        {
            if (m_currentCountDownTime >= (float)countdownTime)
            {
                //m_mainMenu.StartMatch("levelName");
                Debug.Log("Match starting! (not implemented)");
            }
            else
            {
                // Tick timer
                m_currentCountDownTime += Time.deltaTime;

                // Update display
                int currentTime = countdownTime - (int)m_currentCountDownTime;
                countdownText.text = currentTime.ToString();
            }
        }

        private void ShowStartGameMenu()
        {
            // Flag players as ready
            m_playersReady = true;
            
            // Hide player sub menus and show start game menu
            startGameMenu.SetActive(true);
            playerSubmenus.SetActive(false);
        }

        public void CancelStartGame()
        {
            // Set menus
            startGameMenu.SetActive(false);
            playerSubmenus.SetActive(true);

            // Reset timer and unflag players as ready
            m_currentCountDownTime  = 0.0f;
            m_playersReady          = false;

            // Reset player preferences
            for (int i = 0; i < m_players.Length; ++i)
            {
                m_players[i].ready      = false;
                m_players[i].faction    = Faction.NONE;
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

            // Set player preferences, and flag
            // player as ready
            m_players[a_id - 1].ready = true;
            m_players[a_id - 1].faction = a_faction;

            // Show Start Game menu if all players
            // are ready
            if (m_players[0].ready &&
                m_players[1].ready &&
                m_players[2].ready &&
                m_players[3].ready)
            {
                ShowStartGameMenu();
            }

            Debug.Log(string.Format("Player {0} ready with faction: {1}", a_id, a_faction));
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

            // Reset player preferences,
            // and unready player
            m_players[a_id - 1].ready  = false;
            m_players[a_id - 1].faction = Faction.NONE;

            Debug.Log(string.Format("Player {0} unready", a_id));
        }
    }
}
