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
        // Prefabs
        [Header("Prefabs")]
        [Tooltip("Prefab which will be the base for player and base icons")]
        public Image iconTemplatePrefab;

        // Player images
        [Header("Players")]
        [Tooltip("Sprite to represent players within the navy faction")]
        public Sprite navyPlayerSprite;
        [Tooltip("Sprite to represent players within the pirates faction")]
        public Sprite piratesPlayerSprite;
        [Tooltip("Sprite to represent players within the tinkerers faction")]
        public Sprite tinkerersPlayerSprite;
        [Tooltip("Sprite to represent players within the vikings faction")]
        public Sprite vikingsPlayerSprite;

        [Header("Player Passenger Display")]
        [Tooltip("Cutoff value at which the ships will scale to, when at or above this passenger count")]
        public uint passengerDisplayCutoff = 10;
        [Tooltip("Scale at which the player icons will scale towards, when reaching the max passenger display value, in passenger count")]
        public float passengerDisplayScale = 2.0f;
        [Tooltip("Speed at which the player's icons will change scale, in scale units per second")]
        public float passengerScaleSpeed = 0.5f;

        // Base images
        [Header("Bases")]
        [Tooltip("Sprite to represent bases within the navy faction")]
        public Sprite navyBaseSprite;
        [Tooltip("Sprite to represent bases within the pirates faction")]
        public Sprite piratesBaseSprite;
        [Tooltip("Sprite to represent bases within the tinkerers faction")]
        public Sprite tinkerersBaseSprite;
        [Tooltip("Sprite to represent bases within the vikings faction")]
        public Sprite vikingsBaseSprite;

        // Repair zones
        [Header("Repair Zones")]
        [Tooltip("Sprite to represent repair zones")]
        public Sprite repairZoneSprite;

        // Prisonship icon variables
        [Header("Prison Ship")]
        [Tooltip("Sprite to represent the prison ship")]
        public Sprite prisonShipSprite;
        [Tooltip("Flash rate between normal and dropping colour, in seconds")]
        public float prisonshipFlashRate = 0.1f;
        [Tooltip("Sprite colour tint to use on the prison ship icon, when not dropping passengers")]
        public Color prisonshipNormalColour;
        [Tooltip("Sprite colour tint to use on the prison ship icon, when dropping passengers")]
        public Color prisonshipDroppingColour;

        // Score Indicator variables
        [Header("Scoring")]
        [Tooltip("Uses texture offsetting for animation, in UV units per second")]
        public float scoreAnimationSpeed = 0.05f;
        [Tooltip("Time it will take for score bars to reach the current score. (Smaller values will speed this up)")]
        public float scoreSmoothTime = 0.5f;
        [Tooltip("Max speed at which the score bars will change")]
        public float maxScoreChangeSpeed = 2.5f;

        // Transforms
        private List<Transform> m_playerTransforms;
        private List<Transform> m_baseTransforms;
        private List<Transform> m_repairTransforms;
        private Transform m_prisonshipTransform;

        // Player passenger count
        private List<PassengerTray> m_passengerTrays;

        // Minimap icon images
        private List<Image> m_playerImages;
        private List<Image> m_baseImages;
        private List<Image> m_repairImages;
        private Image m_prisonshipImage;

        // Passenger spawner
        private bool m_useNormalPrisonColour    = false;
        private float m_currentPrisonFlashTime  = 0.0f;
        private List<SpawnPassengers> m_passengerSpawners;

        // Score Indicators
        private ScoreIndicator m_player2_ScoreIndicator;
        private ScoreIndicator m_player1_ScoreIndicator;
        private ScoreIndicator m_player4_ScoreIndicator;
        private ScoreIndicator m_player3_ScoreIndicator;
        private DetectFallingPassenger m_player2_Base;
        private DetectFallingPassenger m_player1_Base;
        private DetectFallingPassenger m_player4_Base;
        private DetectFallingPassenger m_player3_Base;
        private float m_player2_ScoreIndicatorVelo;
        private float m_player1_ScoreIndicatorVelo;
        private float m_player4_ScoreIndicatorVelo;
        private float m_player3_ScoreIndicatorVelo;

        // In-Scene references
        GameManager m_gameManager;

        // Cached variables
        private Canvas m_captureCanvas;
        private Camera m_captureCamera;
        private Transform m_transform;

        // TODO: Change display background depending on player count

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
            // Get reference to Game Manager within the scene
            m_gameManager = FindObjectOfType<GameManager>();

            // Get capture camera reference
            m_captureCamera = GetComponentInChildren<Camera>();

            // Get capture canvas reference and ensure it's at 0,0,0 in local space
            m_captureCanvas = m_captureCamera.GetComponentInChildren<Canvas>();
            m_captureCanvas.transform.localPosition = Vector3.zero;

            // Cache common properties
            m_transform = transform;
        }

        /// <summary>
        /// Retrieves the transforms for all players, bases, and repair zones
        /// within the map
        /// </summary>
        void PopulateCacheLists()
        {
            // Get all player transforms and passenger trays
            GameObject[] playerObjects  = m_gameManager.players;

            m_playerTransforms  = new List<Transform>(playerObjects.Length);
            m_passengerTrays    = new List<PassengerTray>(playerObjects.Length);

            for (int i = 0; i < playerObjects.Length; ++i)
            {
                // Get player transform
                m_playerTransforms.Add(playerObjects[i].transform);

                // Get player passenger tray
                m_passengerTrays.Add(playerObjects[i].GetComponentInChildren<PassengerTray>());
            }

            // Get all base references
            GameObject[] bases = m_gameManager.bases;

            m_baseTransforms = new List<Transform>(bases.Length);
            for (int i = 0; i < m_baseTransforms.Capacity; ++i)
            {
                m_baseTransforms.Add(bases[i].transform);
            }

            m_player1_Base = bases[0].GetComponentInChildren<DetectFallingPassenger>();
            m_player2_Base = bases[1].GetComponentInChildren<DetectFallingPassenger>();
            m_player3_Base = bases[2].GetComponentInChildren<DetectFallingPassenger>();
            m_player4_Base = bases[3].GetComponentInChildren<DetectFallingPassenger>();

            // Get all repair zone transforms
            HealPointBehaviour[] repairZones =
                GameObject.FindObjectsOfType<HealPointBehaviour>();

            m_repairTransforms = new List<Transform>(repairZones.Length);

            for (int i = 0; i < m_repairTransforms.Capacity; ++i)
            {
                m_repairTransforms.Add(repairZones[i].transform);
            }

            // Get prison ship transform
            GameObject prisonship = GameObject.FindGameObjectWithTag("Objective");
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
                Image image = Instantiate(iconTemplatePrefab);
                image.transform.SetParent(captureCanvas_trans, false);

                m_baseImages.Add(image);
            }

            // Create repair zone icons
            m_repairImages = new List<Image>(m_repairTransforms.Count);

            for (int i = 0; i < m_repairImages.Capacity; ++i)
            {
                Image image = Instantiate(iconTemplatePrefab);
                image.overrideSprite = repairZoneSprite;
                image.transform.SetParent(captureCanvas_trans, false);

                m_repairImages.Add(image);
            }

            // Create prisonship icon
            m_prisonshipImage = Instantiate(iconTemplatePrefab);
            m_prisonshipImage.overrideSprite = prisonShipSprite;
            m_prisonshipImage.rectTransform.SetParent(captureCanvas_trans, false);

            // Create player icons
            m_playerImages = new List<Image>(m_playerTransforms.Count);

            for (int i = 0; i < m_playerImages.Capacity; ++i)
            {
                Image image = Instantiate(iconTemplatePrefab);
                image.transform.SetParent(captureCanvas_trans, false);

                m_playerImages.Add(image);
            }
        }

        /// <summary>
        /// Gets references to the score bar scripts through this
        /// game object's children (changing the hierachy in any way for the
        /// minimap will break this code) and presets their score percentages
        /// to zero
        /// </summary>
        void SetupScoreIndicators()
        {
            // Get score indicator references
            string scoreBarPath = "Minimap Renderer/Minimap Score Indicator Capture/";

            m_player1_ScoreIndicator = m_transform.FindChild(scoreBarPath + "Player 1 Score Bar").GetComponent<ScoreIndicator>();
            m_player2_ScoreIndicator = m_transform.FindChild(scoreBarPath + "Player 2 Score Bar").GetComponent<ScoreIndicator>();
            m_player3_ScoreIndicator = m_transform.FindChild(scoreBarPath + "Player 4 Score Bar").GetComponent<ScoreIndicator>();
            m_player4_ScoreIndicator = m_transform.FindChild(scoreBarPath + "Player 3 Score Bar").GetComponent<ScoreIndicator>();

            // Set initial scores
            m_player1_ScoreIndicator.scorePercent = 0.0f;
            m_player2_ScoreIndicator.scorePercent = 0.0f;
            m_player3_ScoreIndicator.scorePercent = 0.0f;
            m_player4_ScoreIndicator.scorePercent = 0.0f;

            // Set faction for score bars
            PlayerSettings[] playerSettings = LevelSettings.Instance.playersSettings;

            m_player1_ScoreIndicator.faction = playerSettings[0].faction;
            m_player2_ScoreIndicator.faction = playerSettings[1].faction;
            m_player3_ScoreIndicator.faction = playerSettings[2].faction;
            m_player4_ScoreIndicator.faction = playerSettings[3].faction;
        }

        void Start()
        {
            PopulateCacheLists();
            CreateMapIcons();
            SetupCaptureCamera();
            SetupScoreIndicators();

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
            UpdateBaseIcons();
            UpdatePrisonShipIcon();
            UpdateScoreIndicators();

            // Update repair zone icons
            for (int i = 0; i < m_repairTransforms.Count; ++i)
            {
                Transform repairZone_trans = m_repairTransforms[i];
                RectTransform repairZone_iconRectTrans = m_repairImages[i].rectTransform;

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

        void UpdateBaseIcons()
        {
            // Update base icons
            for (int i = 0; i < m_baseTransforms.Count; ++i)
            {
                Transform baseTransform = m_baseTransforms[i];
                Image baseIcon = m_baseImages[i];
                RectTransform baseIcon_rectTrans = baseIcon.rectTransform;

                // Set base texture
                FactionIndentifier identity = baseTransform.GetComponent<FactionIndentifier>();

                switch (identity.faction)
                {
                    case FactionIndentifier.Faction.NAVY:
                        baseIcon.overrideSprite = navyBaseSprite;
                        break;

                    case FactionIndentifier.Faction.PIRATES:
                        baseIcon.overrideSprite = piratesBaseSprite;
                        break;

                    case FactionIndentifier.Faction.TINKERERS:
                        baseIcon.overrideSprite = tinkerersBaseSprite;
                        break;

                    case FactionIndentifier.Faction.VIKINGS:
                        baseIcon.overrideSprite = vikingsBaseSprite;
                        break;
                }

                // Position base icon
                PositionIconToTransform(baseIcon_rectTrans, baseTransform);
            }
        }

        /// <summary>
        /// Updates all player icon's canvas position and rotation,
        /// as well as smoothly interpolates their scale as players
        /// carry more or less passengers
        /// </summary>
        void UpdatePlayerIcons()
        {
            for (int i = 0; i < m_playerTransforms.Count; ++i)
            {
                Image playerIconImage = m_playerImages[i];
                Transform playerTransform = m_playerTransforms[i];
                RectTransform playerIconTrans = m_playerImages[i].rectTransform;

                // Set icon position
                PositionIconToTransform(playerIconTrans, playerTransform);

                // Set icon texture
                FactionIndentifier identity = playerTransform.GetComponent<FactionIndentifier>();

                switch (identity.faction)
                {
                    case FactionIndentifier.Faction.NAVY:
                        playerIconImage.overrideSprite = navyPlayerSprite;
                        break;

                    case FactionIndentifier.Faction.PIRATES:
                        playerIconImage.overrideSprite = piratesPlayerSprite;
                        break;

                    case FactionIndentifier.Faction.TINKERERS:
                        playerIconImage.overrideSprite = tinkerersPlayerSprite;
                        break;

                    case FactionIndentifier.Faction.VIKINGS:
                        playerIconImage.overrideSprite = vikingsPlayerSprite;
                        break;
                }

                // Set icon rotation
                float playerRotationY = playerTransform.rotation.eulerAngles.y;
                playerIconTrans.localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, -playerRotationY));

                // Set scale
                float passengerCount = m_passengerTrays[i].passengerCount;
                float scale = passengerDisplayScale * (passengerCount / passengerDisplayCutoff);

                // Keep scale above 1
                scale = Mathf.Max(scale, 1.0f);

                // Interpolate to new scale
                playerIconTrans.localScale = Vector3.Lerp(playerIconTrans.localScale,
                    new Vector3(scale, scale, scale), 0.5f * Time.deltaTime);
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
                    m_currentPrisonFlashTime = prisonshipFlashRate;
                    // Swap prison ship colour
                    m_useNormalPrisonColour = !m_useNormalPrisonColour;
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
        /// Updates animation speed of score indicators
        /// for changes within the editor to be reflected, or other
        /// gameplay elements affecting it. Also smoothly interpolates
        /// score percentages as player score increases
        /// </summary>
        void UpdateScoreIndicators()
        {
            // Update animation speed
            m_player2_ScoreIndicator.animationSpeed         = scoreAnimationSpeed;
            m_player1_ScoreIndicator.animationSpeed      = scoreAnimationSpeed;
            m_player4_ScoreIndicator.animationSpeed    = scoreAnimationSpeed;
            m_player3_ScoreIndicator.animationSpeed       = scoreAnimationSpeed;

            // Update percentages
            float navyScorePercent      = 1.0f - ((float)m_player2_Base.peopleLeftToCatch / (float)m_player2_Base.maxPeople);
            float pirateScorePercent    = 1.0f - ((float)m_player1_Base.peopleLeftToCatch / (float)m_player1_Base.maxPeople);
            float tinkerersScorePercent = 1.0f - ((float)m_player4_Base.peopleLeftToCatch / (float)m_player4_Base.maxPeople);
            float vikingScorePercent    = 1.0f - ((float)m_player3_Base.peopleLeftToCatch / (float)m_player3_Base.maxPeople);

            m_player2_ScoreIndicator.scorePercent =
                Mathf.SmoothDamp(m_player2_ScoreIndicator.scorePercent, navyScorePercent, ref m_player2_ScoreIndicatorVelo, scoreSmoothTime, maxScoreChangeSpeed);
            m_player1_ScoreIndicator.scorePercent =
                Mathf.SmoothDamp(m_player1_ScoreIndicator.scorePercent, pirateScorePercent, ref m_player1_ScoreIndicatorVelo, scoreSmoothTime, maxScoreChangeSpeed);
            m_player4_ScoreIndicator.scorePercent =
                Mathf.SmoothDamp(m_player4_ScoreIndicator.scorePercent, tinkerersScorePercent, ref m_player4_ScoreIndicatorVelo, scoreSmoothTime, maxScoreChangeSpeed);
            m_player3_ScoreIndicator.scorePercent =
                Mathf.SmoothDamp(m_player3_ScoreIndicator.scorePercent, vikingScorePercent, ref m_player3_ScoreIndicatorVelo, scoreSmoothTime, maxScoreChangeSpeed);
        }
    }
}