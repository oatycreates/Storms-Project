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
        private ScoreManager m_scoreManager;

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

            // Get reference to Score Manager
            m_scoreManager = FindObjectOfType<ScoreManager>();
            if (m_scoreManager == null)
            {
                Debug.LogError("Unable to find ScoreManager within scene!");
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

            SetupScoreManager();

            // Spawn each player using the level settings data from the Menu scene
            PlayerSettings[] playersSettings = LevelSettings.Instance.playersSettings;
#if UNITY_EDITOR
            // Fill in some defaults
            EditorFillPlayerSettings(ref playersSettings);
#endif
            for (int i = 0; i < playersSettings.Length; ++i)
            {

                if (playersSettings[i].playing)
                {
                    SpawnPlayer(playersSettings[i], i + 1);
                }
            }

            SpawnBases();

            SetupMasterCamera(m_players);
            SetupLevelBounds(m_players);
        }

#if UNITY_EDITOR
        /// <summary>
        /// Editor only player settings pre-fill.
        /// </summary>
        /// <param name="ao_settings">Settings object to be filled.</param>
        void EditorFillPlayerSettings(ref PlayerSettings[] ao_settings)
        {
            if (ao_settings.Length >= 1)
            {
                // First player - Pirates
                ao_settings[0].faction = Faction.PIRATES;
                ao_settings[0].playing = true;
                ao_settings[0].team = Team.NONE;
            }
            if (ao_settings.Length >= 2)
            {
                // Second player - Navy
                ao_settings[1].faction = Faction.NAVY;
                ao_settings[1].playing = true;
                ao_settings[1].team = Team.NONE;
            }
            if (ao_settings.Length >= 3)
            {
                // Third player - Tinkerers
                ao_settings[2].faction = Faction.TINKERERS;
                ao_settings[2].playing = true;
                ao_settings[2].team = Team.NONE;
            }
            if (ao_settings.Length >= 4)
            {
                // Fourth player - Vikings
                ao_settings[3].faction = Faction.VIKINGS;
                ao_settings[3].playing = true;
                ao_settings[3].team = Team.NONE;
            }
        }
#endif

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
                Debug.LogWarning(string.Format("Unable to find spawner for player: {0}", a_playerNo));
                return;
            }

            // Spawn player, and retain reference within global
            // player references
            GameObject player           = playerSpawner.SpawnPlayer(a_playerSettings.faction);
            m_players[a_playerNo - 1]   = player;
        }

        private void SpawnBases()
        {
            switch (LevelSettings.Instance.gamemode)
            {
                case Gamemode.FFA:
                    // Create bases references array
                    m_bases = new GameObject[LevelSettings.Instance.playersPlaying];

                    int playersPlaying = LevelSettings.Instance.playersPlaying;

                    for (int i = 0; i < playersPlaying; ++i)
                    {
                        int playerNumber = i + 1;

                        BaseSpawner baseSpawner = FindBaseSpawnerForPlayer(playerNumber, BaseSpawnerType.FFA_ONLY);
                        PlayerSettings playerSettings = LevelSettings.Instance.GetPlayerSettings(playerNumber);

                        // Ensure a base spawner was found
                        if (baseSpawner == null)
                        {
                            Debug.LogWarning("Unable to find base spawner for player: " + playerNumber);
                            continue;
                        }

                        // Spawn base, and store reference within bases array
                        GameObject playerBase = baseSpawner.SpawnBase(playerSettings.faction);
                        Debug.Log(string.Format("Spawned base: Player {0}", i + 1));

                        m_bases[i] = playerBase;
                    }
                    break;

                case Gamemode.TEAMS:
                    // Create bases references array
                    m_bases = new GameObject[2];

                    // Spawn Alpha base
                    BaseSpawner alphaBaseSpawner = FindBaseSpawnerForTeam(Team.ALPHA);

                    if (alphaBaseSpawner == null)
                    {
                        Debug.LogWarning("Unable to find spawner for Alpha team");
                    }
                    else
                    {
                        GameObject alphaBase = alphaBaseSpawner.SpawnBase(LevelSettings.Instance.alphaTeamFaction);
                        m_bases[0] = alphaBase;

                        Debug.Log("Spawned Base: Team Alpha");
                    }

                    // Spawn Omega base
                    BaseSpawner omegaBaseSpawner = FindBaseSpawnerForTeam(Team.OMEGA);

                    if (omegaBaseSpawner == null)
                    {
                        Debug.LogWarning("Unable to find spawner for Omega team");
                    }
                    else
                    {
                        GameObject omegaBase = omegaBaseSpawner.SpawnBase(LevelSettings.Instance.omegaTeamFaction);
                        m_bases[1] = omegaBase;

                        Debug.Log("Spawned Base: Team Omega");
                    }
                    break;

                case Gamemode.NONE:
                    Debug.LogError("Unable to spawn bases for gamemode 'NONE'");
                    return;
            }
        }

        private PlayerSpawner FindPlayerSpawnerForPlayer(int a_playerNo, PlayerSpawnerType a_type)
        {
            for (int i = 0; i < m_playerSpawners.Length; ++i)
            {
                if (m_playerSpawners[i] == null)
                {
                    continue;
                }

                // Special case, for teams
                if (a_type != PlayerSpawnerType.FFA_ONLY &&
                    m_playerSpawners[i].spawnerType == a_type)
                {
                    return m_playerSpawners[i];
                }

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

        private BaseSpawner FindBaseSpawnerForTeam(Team a_team)
        {
            BaseSpawnerType spawnerType = BaseSpawnerType.FFA_ONLY;

            switch (a_team)
            {
                case Team.ALPHA:
                    spawnerType = BaseSpawnerType.TEAM_ALPHA;
                    break;

                case Team.OMEGA:
                    spawnerType = BaseSpawnerType.TEAM_OMEGA;
                    break;

                case Team.NONE:
                    Debug.LogWarning("Unable to find base spawner for team NONE");
                    return null;
            }

            for (int i = 0; i < m_baseSpawners.Length; ++i)
            {
                if (m_baseSpawners[i].baseType == spawnerType)
                {
                    // Found base spawner for given team
                    return m_baseSpawners[i];
                }
            }

            // Can't find base spawner for given team
            return null;
        }

        private bool FactionPresentWithinMatch(Faction a_faction)
        {
            // Ensure arguments are valid
            if (a_faction == Faction.NONE)
            {
                Debug.LogWarning("Unable to check if NONE faction is present within match");
            }

            int playersPlaying = LevelSettings.Instance.playersPlaying;

            // Loop through all players
            for (int i = 0; i < playersPlaying; ++i)
            {
                PlayerSettings playerSettings = LevelSettings.Instance.GetPlayerSettings(i);

                if (playerSettings.faction == a_faction)
                {
                    // Found player with requested faction, faction
                    // not present within match
                    return true;
                }
            }

            // Unable to find player with requested faction, faction 
            // not present within match
            return false;
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

        private void SetupScoreManager()
        {
#if UNITY_EDITOR
            // Default in editor
            if (LevelSettings.Instance.gamemode == Gamemode.NONE)
            {
                LevelSettings.Instance.gamemode = Gamemode.FFA;
            }
#endif

            switch (LevelSettings.Instance.gamemode)
            {
                case Gamemode.FFA:
                    m_scoreManager.gameType = EGameType.FreeForAll;
                    break;

                case Gamemode.TEAMS:
                    m_scoreManager.gameType = EGameType.TeamGame;
                    break;

                case Gamemode.NONE:
                    Debug.LogWarning("Gamemode not correctly set within LevelSettings script");
                    break;
            }
        }

        private Camera GetPlayerCamera(GameObject a_player)
        {
            return a_player.GetComponentInChildren<Camera>();
        }
    }
}
