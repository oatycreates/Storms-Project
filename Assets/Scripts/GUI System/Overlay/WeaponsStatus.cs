/**
 * File: WeaponsStatus.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 12/11/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Controls the behaviour of the weapons status UI elements
 **/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ProjectStorms
{
    public class WeaponsStatus : MonoBehaviour
    {
        [Header("Sprite References")]
        public Sprite cannonActive;
        public Sprite mineActive;
        public Sprite chaffActive;
        public Sprite whirlwindActive;
        public Sprite boostActive;

        [Space(5)]
        public Sprite cannonInactive;
        public Sprite mineInactive;
        public Sprite chaffInactive;
        public Sprite whirlwindInactive;
        public Sprite boostInactive;

        [Header("Image References")]
        public Image cannonImage;
        public Image mineImage;
        public Image chaffImage;
        public Image whirlwindImage;
        public Image boostImage;

        [Header("Configuration")]
        [Range(1, 4)]
        public int playerNumber = 1;

        //private Image m_weaponStatusImage;

        private Countermeasures m_playerWeaponSystem;
        private StateManager m_playerStateManager;

        public void Awake()
        {
            /*m_weaponStatusImage = GetComponent<Image>();
            if (m_weaponStatusImage == null)
            {
                Debug.LogError("No Image component attached");
            }*/
        }

        void Start()
        {
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null)
            {
                Debug.LogError("Can't find Game Manager within scene");
            }

            GameObject player = gameManager.players[playerNumber - 1];
            m_playerWeaponSystem = player.GetComponent<Countermeasures>();
            if (m_playerWeaponSystem == null)
            {
                Debug.LogError("Unable to find player Counter Measure Script");
            }

            m_playerStateManager = player.GetComponent<StateManager>();
            if (m_playerStateManager == null)
            {
                Debug.LogError("Unable to find player StateManager Script");
            }
        }

        void Update()
        {
            // Set Cannon Sprite
            if (m_playerWeaponSystem.missileCooldown <= 0.0f)
            {
                cannonImage.sprite = cannonActive;
            }
            else
            {
                cannonImage.sprite = cannonInactive;
            }

            // Count mines in scene
            int numMyMinesInScene = 0;
            MineBehaviour[] mines = GameObject.FindObjectsOfType<MineBehaviour>();
            foreach (MineBehaviour mine in mines)
            {
                if (mine.gameObject.CompareTag(m_playerWeaponSystem.gameObject.tag))
                {
                    numMyMinesInScene += 1;
                }
            }

            // Set Mine Sprite
            if (m_playerWeaponSystem.minesCooldown <= 0.0f && numMyMinesInScene < m_playerWeaponSystem.pooledSkyMines)
            {
                mineImage.sprite = mineActive;
            }
            else
            {
                mineImage.sprite = mineInactive;
            }

            // Set Chaff Sprite
            if (m_playerWeaponSystem.chaffCooldown <= 0.0f)
            {
                chaffImage.sprite = chaffActive;
            }
            else
            {
                chaffImage.sprite = chaffInactive;
            }

            // Set Whirlwind Sprite
            if (m_playerWeaponSystem.pinwheelCooldown <= 0.0f)
            {
                whirlwindImage.sprite = whirlwindActive;
            }
            else
            {
                whirlwindImage.sprite = whirlwindInactive;
            }

            // Set Boost Sprite
            if (m_playerStateManager.timeBetweenStall <= 0)
            {
                boostImage.sprite = boostActive;
            }
            else
            {
                boostImage.sprite = boostInactive;
            }
        }
    }
}
