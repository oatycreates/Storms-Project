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
    public class SpawnManager : MonoBehaviour
    {
        private MasterCamera m_masterCamera;

        public void Awake()
        {
            m_masterCamera = FindObjectOfType<MasterCamera>();
            if (m_masterCamera == null)
            {
                Debug.LogError("Unable to find Master Camera within scene!");
            }
        }

        void Start()
        {
            // Get references to players
            InputManager[] inputManagers = FindObjectsOfType<InputManager>();

            GameObject[] playerObjects = new GameObject[inputManagers.Length];
            for (int i = 0; i < playerObjects.Length; ++i)
            {
                playerObjects[i] = inputManagers[i].gameObject;
            }

            // Setup camera settings
            SetupMasterCamera(playerObjects);
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
                    Debug.LogError("Less than 2 players are within the scene, or spawn manager is unable to find them!");
                    return;
            }

            // Set player camera references
            for (int i = 0; i < a_playersArray.Length; ++i)
            {
                Camera playerCam = GetPlayerCamera(a_playersArray[i]);

                if (playerCam != null)
                {
                    Debug.Log("Player cam was null!");
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

        Camera GetPlayerCamera(GameObject a_player)
        {
            return a_player.GetComponentInChildren<Camera>();
        }
    }
}
