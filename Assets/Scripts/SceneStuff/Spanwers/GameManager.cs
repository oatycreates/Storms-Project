/**
 * File: SpawnManager.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 21/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Controls overall functionality of level spawners
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
    public class GameManager : MonoBehaviour
    {
        private MasterCamera m_masterCamera;
        private LevelBoundsBehaviour m_levelBounds;

        private PlayerSpawner[] m_playerSpawners;
        private BaseSpawner[] m_baseSpawners;

        private GameObject[] m_players;
        private GameObject[] m_bases;

        public GameObject[] players
        {
            get
            {
                return m_players;
            }
        }

        public GameObject[] bases
        {
            get
            {
                return m_bases;
            }
        }

        public void Awake()
        {
            // Get reference to master camera
            m_masterCamera = FindObjectOfType<MasterCamera>();
            if (m_masterCamera == null)
            {
                Debug.LogError("Unable to find Master Camera within scene!");
            }

            // Get reference to Level Bounds
            m_levelBounds = FindObjectOfType<LevelBoundsBehaviour>();
            if (m_levelBounds == null)
            {
                Debug.LogError("Unable to find level bounds within scene!");
            }

            // Get all players spawners
            m_playerSpawners = FindObjectsOfType<PlayerSpawner>();
            if (m_playerSpawners == null ||
                m_playerSpawners.Length == 0)
            {
                Debug.LogWarning("No player spawners within scene, or spawn manager is unable to find them");
            }

            // Get all base spawners
            m_baseSpawners = FindObjectsOfType<BaseSpawner>();
            if (m_baseSpawners == null ||
                m_baseSpawners.Length == 0)
            {
                Debug.LogWarning("No base spawners within scene, or spawn manager is unable to find them");
            }
        }

        private void Start()
        {
            m_players = new GameObject[LevelSettings.Instance.playersPlaying];

            // Spawn each player using the level settings data from the Menu scene
            PlayerSettings[] playersSettings = LevelSettings.Instance.playersSettings;
            for (int i = 0; i < playersSettings.Length; ++i)
            {
                if (playersSettings[i].playing)
                {
                    SpawnPlayer(playersSettings[i], i + 1);
                }
            }

            SetupMasterCamera(m_players);
            SetupLevelBounds(m_players);
        }

        private void SpawnPlayer(PlayerSettings a_playerSettings, int a_playerNo)
        {
            PlayerSpawnerType spawnerType = PlayerSpawnerType.FFA_ONLY;

            // Calculate which spawner type we should search for
            switch (LevelSettings.Instance.gamemode)
            {
                case Gamemode.TEAMS:
                    // Get player's team
                    switch (a_playerSettings.team)
                    {
                        case Team.ALPHA:
                            spawnerType = PlayerSpawnerType.TEAN_ALPHA;
                            break;

                        case Team.OMEGA:
                            spawnerType = PlayerSpawnerType.TEAM_OMEGA;
                            break;

                        case Team.NONE:
                            Debug.LogError(string.Format("Team for player {0} not set correctly, unable to spawn player", a_playerNo));
                            return;
                    }
                    break;

                case Gamemode.FFA:
                    spawnerType = PlayerSpawnerType.FFA_ONLY;
                    break;

                case Gamemode.NONE:
                    Debug.LogError(string.Format("Gamemode not set correctly, unable to spawn player {0}", a_playerNo));
                    return;
            }

            // Find player spawner
            PlayerSpawner playerSpawner = FindPlayerSpawnerForPlayer(a_playerNo, spawnerType);

            // Ensure player spawner was found
            if (playerSpawner == null)
            {
                Debug.LogError(string.Format("Unable to find spawner for player: {0}", a_playerNo));
                return;
            }

            // Spawn player, and retain reference within global
            // player references
            GameObject player           = playerSpawner.SpawnPlayer(a_playerSettings.faction);
            m_players[a_playerNo - 1]   = player;
        }

        private PlayerSpawner FindPlayerSpawnerForPlayer(int a_playerNo, PlayerSpawnerType a_type)
        {
            for (int i = 0; i < m_playerSpawners.Length; ++i)
            {
                if (m_playerSpawners[i].playerNumber == a_playerNo &&
                    m_playerSpawners[i].spawnerType == a_type)
                {
                    // Found player spawner for given player
                    return m_playerSpawners[i];
                }
            }

            // Can't find player spawner for given player
            return null;
        }

        private BaseSpawner FindBaseSpawnerForPlayer(int a_playerNo, BaseSpawnerType a_type)
        {
            for (int i = 0; i < m_baseSpawners.Length; ++i)
            {
                if (m_baseSpawners[i].playerNumber == a_playerNo &&
                    m_baseSpawners[i].baseType == a_type)
                {
                    // Found base spawner for given player
                    return m_baseSpawners[i];
                }
            }

            // Can't find base spawner for given player
            return null;
        }

        private void SetupMasterCamera(GameObject[] a_playersArray)
        {
            // Set enum to correct camera amount
            switch (a_playersArray.Length)
            {
                case 2:
                    m_masterCamera.currentCamera = ECamerasInScene.Two;
                    break;

                case 3:
                    m_masterCamera.currentCamera = ECamerasInScene.Three;
                    break;

                case 4:
                    m_masterCamera.currentCamera = ECamerasInScene.Four;
                    break;

                default:
                    m_masterCamera.currentCamera = ECamerasInScene.One;
                    Debug.LogWarning("Less than 2 players are within the scene, or spawn manager is unable to find them!");
                    return;
            }

            // Set player camera references
            for (int i = 0; i < a_playersArray.Length; ++i)
            {
                Camera playerCam = GetPlayerCamera(a_playersArray[i]);
                AirshipCamBehaviour camScript = playerCam.GetComponentInParent<AirshipCamBehaviour>();
                if (camScript != null)
                {
                    // Set up the camera
                    camScript.InitialiseCam();
                }

                switch (i)
                {
                    case 0:
                        m_masterCamera.cam1 = playerCam;
                        break;

                    case 1:
                        m_masterCamera.cam2 = playerCam;
                        break;

                    case 2:
                        m_masterCamera.cam3 = playerCam;
                        break;

                    case 3:
                        m_masterCamera.cam4 = playerCam;
                        break;

                    default:
                        break;
                }
            }

            // Finished initialising the player camera
            m_masterCamera.InitialiseMasterCamera();
        }

        private void SetupLevelBounds(GameObject[] a_playersArray)
        {
            // Ensure arguments are valid
            if (a_playersArray == null)
            {
                Debug.LogError("Players array within GameManager is null");
            }

            m_levelBounds.player1RigidBody = a_playersArray[0].GetComponent<Rigidbody>();
            m_levelBounds.player2RigidBody = a_playersArray[1].GetComponent<Rigidbody>();
            m_levelBounds.player3RigidBody = a_playersArray[2].GetComponent<Rigidbody>();
            m_levelBounds.player4RigidBody = a_playersArray[3].GetComponent<Rigidbody>();
        }

        private Camera GetPlayerCamera(GameObject a_player)
        {
            return a_player.GetComponentInChildren<Camera>();
        }
    }
}
