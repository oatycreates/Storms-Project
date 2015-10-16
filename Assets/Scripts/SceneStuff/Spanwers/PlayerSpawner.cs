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
    public class PlayerSpawner : MonoBehaviour
    {
        [Header("Prefabs")]
        public GameObject navyPlayer;
        public GameObject piratesPlayer;
        public GameObject tinkerersPlayer;
        public GameObject vikingsPlayer;

        void Start()
        {

        }

        void Update()
        {

        }
    }
}
