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
    [RequireComponent(typeof(Image))]
    public class WeaponsStatus : MonoBehaviour
    {
        [Header("References")]
        public Sprite weaponsActiveSprite;
        public Sprite weaponsInactiveSprite;

        [Header("Configuration")]
        [Range(1, 4)]
        public int playerNumber = 1;

        private Image m_weaponStatusImage;

        private Countermeasures m_playerWeaponSystem;

        public void Awake()
        {
            m_weaponStatusImage = GetComponent<Image>();
            if (m_weaponStatusImage == null)
            {
                Debug.LogError("No Image component attached");
            }
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
                Debug.LogError("Unable to find player Counter Measure");
            }
        }

        void Update()
        {
            if (m_playerWeaponSystem.weaponsActive)
            {
                m_weaponStatusImage.sprite = weaponsActiveSprite;
            }
            else
            {
                m_weaponStatusImage.sprite = weaponsInactiveSprite;
            }
        }
    }
}
