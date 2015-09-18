/**
 * File: MinimapRenderer.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 17/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Primary behaviour script for the in-game minimap,
 *      displaying all player's locations, player's score, repair zones
 *      and player's base locations
 **/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ProjectStorms
{
	public class Minimap : MonoBehaviour 
	{
        // Map icon images
        [Space(10)]
        [Tooltip("Prefab GUI Image, to represent players within the world")]
        public Image playerImage;
        [Tooltip("Prefab GUI Image, to represent player bases within the world")]
        public Image playerBaseImage;
        [Tooltip("Prefab GUI Image, to represent repair zones within the world")]
        public Image repairZoneImage;
        [Tooltip("Prefab GUI Image, to display player score")]
        public Image scoreIndicatorImage;
        [Tooltip("Prefab GUI Image, to represent the prison ship")]
        public Image prisonshipImage;

        // Player icons colour tints
        [Space(10)]
        [Tooltip("Sprite colour tint to use on Player 1 objects within the minimap")]
        public Color player1Colour;
        [Tooltip("Sprite colour tint to use on Player 2 objects within the minimap")]
        public Color player2Colour;
        [Tooltip("Sprite colour tint to use on Player 3 objects within the minimap")]
        public Color player3Colour;
        [Tooltip("Sprite colour tint to use on Player 4 objects within the minimap")]
        public Color player4Colour;

        // Prisonship colour tint
        [Space(10)]
        [Tooltip("Flash rate between normal and dropping colour, in seconds")]
        public float prisonshipFlashRate = 0.1f;
        [Tooltip("Sprite colour tint to use on the prison ship icon, when not dropping passengers")]
        public Color prisonshipNormalColour;
        [Tooltip("Sprite colour tint to use on the prison ship icon, when dropping passengers")]
        public Color prisonshipDroppingColour;

        // Player icon resizing
        [Space(10)]
        [Tooltip("Used to scale player icon, as carry more passengers. Ships carrying passengers more than this value won't increase in size, but will be at the maximum size")]
        public float maxPlayerPassengerDisplay;

        // Transforms
        private List<Transform> m_playerTransforms;
        private List<Transform> m_baseTransforms;
        private List<Transform> m_repairTransforms;
        private Transform m_prisonshipTransform;

        // Minimap icon images
        private List<Image> m_playerImages;
        private List<Image> m_baseImages;
        private List<Image> m_repairImages;
        private Image m_prisonshipImage;

        // Passenger spawner
        private bool m_passengersSpawning       = false;
        private bool m_useNormalPrisonColour    = false;
        private float m_currentPrisonFlashTime  = 0.0f;
        private List<SpawnPassengers> m_passengerSpawners;

        // Cached variables
        private Canvas m_captureCanvas;
        private Camera m_captureCamera;
        private Transform m_transform;
        private GameObject m_gameObject;

        // TODO: Add support for powerups
        // TODO: Change display background depending on player count
        // TODO: Make this script run within editor for easy tweaking
        // TODO: Set icons' alpha to background alpha, or have an overall tweakable value
        // TODO: Show when prison ship is dropping passengers through icon colour flashing

        /// <summary>
        /// Returns whether or not any passengers are currently
        /// being spawned
        /// </summary>
        bool passengersSpawning
        {
            get
            {
                // Loop through all spawners and check if any are
                // current spawning
                bool passengersSpawning = false;
                for (int i = 0; i < m_passengerSpawners.Count; ++i)
                {
                    if (m_passengerSpawners[i].currentlySpawning)
                    {
                        passengersSpawning = true;
                    }
                }

                return passengersSpawning;
            }
        }

        public void Awake()
        {
            // Get capture camera reference
            m_captureCamera = GetComponentInChildren<Camera>();

            // Get capture canvas reference and ensure it's at 0,0,0 in local space
            m_captureCanvas = m_captureCamera.GetComponentInChildren<Canvas>();
            m_captureCanvas.transform.localPosition = Vector3.zero;

            // Cache common properties
            m_transform     = transform;
            m_gameObject    = gameObject;
        }

        /// <summary>
        /// Retrieves the transforms for all players, bases, and repair zones
        /// within the map
        /// </summary>
        void GetObjectTransforms()
        {
            // Get all player transforms
            AirshipControlBehaviour[] players =
                GameObject.FindObjectsOfType<AirshipControlBehaviour>();

            m_playerTransforms = new List<Transform>(players.Length);

            for (int i = 0; i < m_playerTransforms.Capacity; ++i)
            {
                m_playerTransforms.Add(players[i].transform);
            }

            // Get all base transforms
            PirateBaseIdentity[] bases =
                GameObject.FindObjectsOfType<PirateBaseIdentity>();

            m_baseTransforms = new List<Transform>(bases.Length);

            for (int i = 0; i < m_baseTransforms.Capacity; ++i)
            {
                m_baseTransforms.Add(bases[i].transform);
            }

            // Get all repair zone transforms
            HealPointBehaviour[] repairZones =
                GameObject.FindObjectsOfType<HealPointBehaviour>();

            m_repairTransforms = new List<Transform>(repairZones.Length);

            for (int i = 0; i < m_repairTransforms.Capacity; ++i)
            {
                m_repairTransforms.Add(repairZones[i].transform);
            }

            // Get prison ship transform
            PrisonFortressRotate prisonship = GameObject.FindObjectOfType<PrisonFortressRotate>();
            m_prisonshipTransform = prisonship.transform;
        }

        /// <summary>
        /// Creates the maps icon using the given prefabs
        /// (assumes prefabs aren't null)
        /// </summary>
        void CreateMapIcons()
        {
            Transform captureCanvas_trans = m_captureCanvas.transform;

            // Create base icons
            m_baseImages = new List<Image>(m_baseTransforms.Count);
            
            for (int i = 0; i < m_baseImages.Capacity; ++i)
            {
                Image image = Instantiate(playerBaseImage);
                image.transform.SetParent(captureCanvas_trans, false);
            
                m_baseImages.Add(image);
            }
            
            // Create repair zone icons
            m_repairImages = new List<Image>(m_repairTransforms.Count);
            
            for (int i = 0; i < m_repairImages.Capacity; ++i)
            {
                Image image = Instantiate(repairZoneImage);
                image.transform.SetParent(captureCanvas_trans, false);
            
                m_repairImages.Add(image);
            }

            // Create prisonship icon
            m_prisonshipImage = Instantiate(prisonshipImage);
            m_prisonshipImage.rectTransform.SetParent(captureCanvas_trans, false);

            // Create player icons
            m_playerImages = new List<Image>(m_playerTransforms.Count);

            for (int i = 0; i < m_playerImages.Capacity; ++i)
            {
                Image image = Instantiate(playerImage);
                image.transform.SetParent(captureCanvas_trans, false);

                m_playerImages.Add(image);
            }
        }

        void Start()
        {
            GetObjectTransforms();
            CreateMapIcons();
            SetupCaptureCamera();

            // Get passenger spawner references
            SpawnPassengers[] passengerSpawners = GameObject.FindObjectsOfType<SpawnPassengers>();
            m_passengerSpawners = new List<SpawnPassengers>(passengerSpawners.Length);

            for (int i = 0; i < m_passengerSpawners.Capacity; ++i)
            {
                m_passengerSpawners.Add(passengerSpawners[i]);
            }
        }

        public void OnEnable()
        {
            Canvas.willRenderCanvases += OnWillRenderCanvas;
        }

        public void OnDisable()
        {
            Canvas.willRenderCanvases -= OnWillRenderCanvas;
        }

        /// <summary>
        /// Callback function for when cavas is going to be rendered.
        /// Will update the minimap's icons when called
        /// </summary>
        void OnWillRenderCanvas()
        {
            UpdatePlayerIcons();
            
            // Update base icons
            for (int i = 0; i < m_baseTransforms.Count; ++i)
            {
                Transform baseTransform             = m_baseTransforms[i];
                Image baseIcon                      = m_baseImages[i];
                RectTransform baseIcon_rectTrans    = baseIcon.rectTransform;
                
                // Colour base icon (may not be required with actual icons)
                ColourIconBasedUponTag(baseIcon, m_baseTransforms[i].tag);
                
                // Position base icon
                PositionIconToTransform(baseIcon_rectTrans, baseTransform);
            }

            // Update prison ship icon
            UpdatePrisonShipIcon();

            // Update repair zone icons
            for (int i = 0; i < m_repairTransforms.Count; ++i)
            {
                Transform repairZone_trans              = m_repairTransforms[i];
                RectTransform repairZone_iconRectTrans  = m_repairImages[i].rectTransform;

                PositionIconToTransform(repairZone_iconRectTrans, repairZone_trans);
            }
        }

        /// <summary>
        /// Positions an icon to the given transform, in minimap-space
        /// </summary>
        /// <param name="a_iconTrans">Rectangle transform of the icon</param>
        /// <param name="a_transform">Transform to move this icon to</param>
        void PositionIconToTransform(RectTransform a_iconTrans, Transform a_transform)
        {
            Vector3 transformPos = a_transform.position;
            Vector3 iconPosition = a_iconTrans.position;

            a_iconTrans.position = new Vector3(transformPos.x, iconPosition.y, transformPos.z);
        }

        /// <summary>
        /// Sets up the capture camera using the level's
        /// "Level Bounds" prefab
        /// </summary>
        void SetupCaptureCamera()
        {
            LevelBoundsBehaviour levelBounds = GameObject.FindObjectOfType<LevelBoundsBehaviour>();

            if (levelBounds == null)
            {
                // Unable to locate level bounds prefab within scene
                // Throw exception, and deactivate minimap
                Debug.LogException(new System.Exception("Unable to find Level Bounds prefab within scene"));
                this.gameObject.SetActive(false);

                return;
            }

            Vector3 levelBounds_size = levelBounds.GetComponent<BoxCollider>().size;
            Vector3 levelBounds_pos = levelBounds.transform.position;

            // Set camera position
            m_captureCamera.transform.position = new Vector3(
                levelBounds_pos.x,
                levelBounds_pos.y + (levelBounds_size.y * 0.5f),
                levelBounds_pos.z);

            // Setup camera frustrum
            m_captureCamera.farClipPlane = levelBounds_size.y;
            m_captureCamera.orthographicSize = Mathf.Max(levelBounds_size.x, levelBounds_size.z) * 0.5f;
        }

        /// <summary>
        /// Updates all player icon's canvas position and rotation
        /// </summary>
        void UpdatePlayerIcons()
        {
            for (int i = 0; i < m_playerTransforms.Count; ++i)
            {
                Transform playerTransform       = m_playerTransforms[i];
                RectTransform playerIconTrans   = m_playerImages[i].rectTransform;

                // Set icon position
                PositionIconToTransform(playerIconTrans, playerTransform);

                // Set icon colour
                ColourIconBasedUponTag(m_playerImages[i], m_playerTransforms[i].tag);

                // Set icon rotation
                float playerRotationY           = playerTransform.rotation.eulerAngles.y;
                playerIconTrans.localRotation   = Quaternion.Euler(new Vector3(0.0f, 0.0f, -playerRotationY));
            }
        }

        /// <summary>
        /// Updates the prison ship icon, and will flash colour tint
        /// when passengers dropping, are detected
        /// </summary>
        void UpdatePrisonShipIcon()
        {
            // Update icon colour
            if (passengersSpawning)
            {
                if (m_currentPrisonFlashTime <= 0.0f)
                {
                    // Reset flashing timer
                    m_currentPrisonFlashTime    = prisonshipFlashRate;
                    // Swap prison ship colour
                    m_useNormalPrisonColour     = !m_useNormalPrisonColour;
                }
                else
                {
                    // Tick prison ship icon flash, timer
                    m_currentPrisonFlashTime -= Time.deltaTime;
                }

                // Assign prison ship icon flashing colour
                m_prisonshipImage.color = 
                    m_useNormalPrisonColour ? prisonshipNormalColour : prisonshipDroppingColour;
            }
            else
            {
                // Assign prison ship normal colour
                m_prisonshipImage.color = prisonshipNormalColour;
            }

            // Update icon position
            PositionIconToTransform(m_prisonshipImage.rectTransform,
                m_prisonshipTransform);
        }

        /// <summary>
        /// Colours a given UI image to a player's colour
        /// </summary>
        /// <param name="a_image">Image to colour</param>
        /// <param name="a_tag">Player tag</param>
        void ColourIconBasedUponTag(Image a_image, string a_tag)
        {
            switch (a_tag)
            {
                case "Player1_":
                    a_image.color = player1Colour;
                    break;

                case "Player2_":
                    a_image.color = player2Colour;
                    break;

                case "Player3_":
                    a_image.color = player3Colour;
                    break;

                case "Player4_":
                    a_image.color = player4Colour;
                    break;

                default:
                    Debug.LogWarning(string.Format("Unknown player tag given for: {0} (given tag: {1}) \n Unable to colour icon",  
                        a_image.name, a_tag));
                    break;
            }
        }
	}
}
