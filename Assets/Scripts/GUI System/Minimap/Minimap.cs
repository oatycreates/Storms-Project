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
        // TODO: Change display background depending on player count
        // TODO: Make this script run within editor for easy tweaking

        // Map icon images
        [Tooltip("Prefab GUI Image, to represent players within the world")]
        public Image playerImage;
        [Tooltip("Prefab GUI Image, to represent player bases within the world")]
        public Image playerBaseImage;
        [Tooltip("Prefab GUI Image, to represent repair zones within the world")]
        public Image repairZoneImage;
        [Tooltip("Prefab GUI Image, to display player score")]
        public Image scoreIndicator;

        // TODO: Add support for powerups

        // Player icons colour tints
        [Tooltip("Sprite colour tint to use on Player 1 objects within the minimap")]
        public Color player1Colour;
        [Tooltip("Sprite colour tint to use on Player 2 objects within the minimap")]
        public Color player2Colour;
        [Tooltip("Sprite colour tint to use on Player 3 objects within the minimap")]
        public Color player3Colour;
        [Tooltip("Sprite colour tint to use on Player 4 objects within the minimap")]
        public Color player4Colour;

        // Transforms
        private List<Transform> m_playerTransforms;
        private List<Transform> m_baseTransforms;
        private List<Transform> m_repairTransforms;

        // Minimap icon images
        private List<Image> m_playerImages;
        private List<Image> m_baseImages;
        private List<Image> m_repairImages;

        // Cached variables
        private Canvas m_captureCanvas;
        private Camera m_captureCamera;

        public void Awake()
        {
            // Get capture camera reference
            m_captureCamera = GetComponentInChildren<Camera>();

            // Get capture canvas reference and ensure it's at 0,0,0 in local space
            m_captureCanvas = m_captureCamera.GetComponentInChildren<Canvas>();
            m_captureCanvas.transform.localPosition = Vector3.zero;
        }

        void GetTransforms()
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
        }

        void CreateMapIcons()
        {
            // Create player icons
            m_playerImages = new List<Image>(m_playerTransforms.Count);
            
            for (int i = 0; i < m_playerImages.Capacity; ++i)
            {
                Image image = Instantiate(playerImage);
                image.transform.SetParent(m_captureCanvas.transform, false);

                m_playerImages.Add(image);
            }

            //// Create base icons
            //m_baseImages = new List<Image>(m_baseTransforms.Count);
            //
            //for (int i = 0; i < m_baseImages.Capacity; ++i)
            //{
            //    Image image = Instantiate(playerBaseImage);
            //    image.transform.SetParent(m_captureCanvas.transform, false);
            //
            //    m_baseImages.Add(image);
            //}
            //
            //// Create repair zone icons
            //m_repairImages = new List<Image>(m_repairTransforms.Count);
            //
            //for (int i = 0; i < m_repairImages.Capacity; ++i)
            //{
            //    Image image = Instantiate(repairZoneImage);
            //    image.transform.SetParent(m_captureCanvas.transform, false);
            //
            //    m_repairImages.Add(image);
            //}
        }

        void Start()
        {
            GetTransforms();
            CreateMapIcons();
            SetupCaptureCamera();
        }

        public void OnEnable()
        {
            Canvas.willRenderCanvases += OnWillRenderCanvas;
        }

        public void OnDisable()
        {
            Canvas.willRenderCanvases -= OnWillRenderCanvas;
        }

        void OnWillRenderCanvas()
        {
            UpdatePlayerIcons();
            
            //// Update base icons
            //for (int i = 0; i < m_baseTransforms.Count; ++i)
            //{
            //    Vector3 basePosition    = m_baseTransforms[i].position;
            //    Image baseIcon          = m_baseImages[i];
            //
            //    // TODO: Colour based upon tag
            //
            //    baseIcon.transform.position = 
            //        new Vector3(basePosition.x, 0.0f, basePosition.z);
            //}
            //
            //// Update repair zone icons
            //for (int i = 0; i < m_repairTransforms.Count; ++i)
            //{
            //    Vector3 repairPosition  = m_repairTransforms[i].position;
            //    Image repairIcon        = m_repairImages[i];
            //
            //    repairIcon.transform.position = 
            //        new Vector3(repairPosition.x, 0.0f, repairPosition.z);
            //}
        }

        void SetupCaptureCamera()
        {
            LevelBoundsBehaviour levelBounds = GameObject.FindObjectOfType<LevelBoundsBehaviour>();
            Vector3 levelBounds_size = levelBounds.GetComponent<BoxCollider>().size;
            Vector3 levelBounds_pos = levelBounds.transform.position;

            // Set capture node position
            Transform captureNode   = transform.FindChild("Capture Node");
            captureNode.position    = new Vector3(
                levelBounds_pos.x, 
                levelBounds_pos.y + levelBounds_size.y, 
                levelBounds_pos.z);

            // Setup camera frustrum
            m_captureCamera.farClipPlane = levelBounds_size.y;
            m_captureCamera.orthographicSize = Mathf.Max(levelBounds_size.x, levelBounds_size.z) / 2.0f;
        }

        void UpdatePlayerIcons()
        {
            for (int i = 0; i < m_playerTransforms.Count; ++i)
            {
                Vector3 playerPosition      = m_playerTransforms[i].position;
                Transform playerIconTrans   = m_playerImages[i].transform;

                // Set icon position
                playerIconTrans.position = new Vector3(
                    playerPosition.x, 
                    playerPosition.y, 
                    playerPosition.z);

                // Perhaps using Rect transform may fix odd positioning bug?

                // TODO: Colour icon based upon tag

                // Set icon rotation
                //Vector3 playerRotation  = m_playerTransforms[i].rotation.eulerAngles;
                //playerRotation.y        = 0.0f;
                //playerRotation.z        = 0.0f;
                //
                //playerIconTrans.rotation = Quaternion.Euler(playerRotation);
            }
        }
	}
}
