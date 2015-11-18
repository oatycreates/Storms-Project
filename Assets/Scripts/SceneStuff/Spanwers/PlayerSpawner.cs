/**
 * File: PlayerSpawner.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 15/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Spawns a player into the scene, using
 *  the settings from the Main Menu scene
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    public enum PlayerSpawnerType
    {
        FFA_ONLY,
        TEAN_ALPHA,
        TEAM_OMEGA
    }

    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject navyPlayer;
        public GameObject piratesPlayer;
        public GameObject tinkerersPlayer;
        public GameObject vikingsPlayer;

        [Header("Spawner Settings")]
        public PlayerSpawnerType spawnerType    = PlayerSpawnerType.FFA_ONLY;
        [Tooltip("Non-zero based, player 1 is pirates")]
        public int playerNumber                 = 1;

        // Script references
        private ScoreManager m_scoreManager;

        public void Awake()
        {
            // Ensure prefabs are set correctly
            if (navyPlayer      == null ||
                piratesPlayer   == null ||
                tinkerersPlayer == null ||
                vikingsPlayer   == null)
            {
                Debug.LogError("Prefabs not set for player spawner!");
                return;
            }

            // Get reference to score manager
            m_scoreManager = FindObjectOfType<ScoreManager>();
            if (m_scoreManager == null)
            {
                Debug.LogError("Unable to find Score Manager within scene!");
            }

            // Ensure player number is set correctly
            if (playerNumber < 1 ||
                playerNumber > 4)
            {
                Debug.LogWarning(string.Format("Invalid player number for player spawner {0}", name));
            }
        }

        public GameObject SpawnPlayer(Faction a_faction)
        {
            // Ensure valid parameters are given
            if (a_faction == Faction.NONE)
            {
                Debug.LogError("Cannot spawn player with NONE faction");
                return null;
            }

            // Ensure we can spawn in this gamemode
            if (spawnerType == PlayerSpawnerType.FFA_ONLY &&
                m_scoreManager.gameType != EGameType.FreeForAll)
            {
                // Don't spawn FFA player in teams gamemode
                return null;
            }
            else if ((spawnerType == PlayerSpawnerType.TEAN_ALPHA || spawnerType == PlayerSpawnerType.TEAM_OMEGA) &&
                     m_scoreManager.gameType != EGameType.TeamGame)
            {
                // Don't spawn Team player in FFA gamemode
                return null;
            }

            // Get correct prefab
            GameObject prefab = null;

            switch (a_faction)
            {
                case Faction.NONE:
                    Debug.LogError("Unable to load prefab for NONE faction");
                    return null;

                case Faction.NAVY:
                    prefab = navyPlayer;
                    break;

                case Faction.PIRATES:
                    prefab = piratesPlayer;
                    break;

                case Faction.TINKERERS:
                    prefab = tinkerersPlayer;
                    break;

                case Faction.VIKINGS:
                    prefab = vikingsPlayer;
                    break;
            }

            // Create player ship
            GameObject spawnedPlayer = Instantiate(prefab, transform.position, transform.rotation) as GameObject;

            // Set player tag
            switch (playerNumber)
            {
                case 1:
                    spawnedPlayer.tag = "Player1_";
                    break;

                case 2:
                    spawnedPlayer.tag = "Player2_";
                    break;

                case 3:
                    spawnedPlayer.tag = "Player3_";
                    break;

                case 4:
                    spawnedPlayer.tag = "Player4_";
                    break;

                default:
                    Debug.LogWarning("Unable to tag player with number: " + playerNumber);
                    break;
            }

            DestroyImmediate(this.gameObject);

           // Debug.Log(string.Format("Spawned player: {0}", playerNumber));
            return spawnedPlayer;
        }
    }
}
