/**
 * File: MinimapRenderer.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 16/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Controls rendering of the experimental minimap
 **/

using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ProjectStorms.Experimental
{
	public class MinimapRenderer : MonoBehaviour 
	{
        public Text playerImage;
        public Text baseImage;

        public float playerIconScale;
        public float playerBaseIconScale;

        public Color player1Colour;
        public Color player2Colour;
        public Color player3Colour;
        public Color player4Colour;

        public List<Transform> playersTransfroms;
        public List<Transform> playerBasesTranforms;

        Canvas m_canvas;

        List<Text> m_playerImages;
        List<Text> m_playerBaseImages;

        public void Awake()
        {
            // Get canvas reference
            m_canvas = GetComponentInChildren<Canvas>();
            m_canvas.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

            // Create player icons
            m_playerImages      = new List<Text>(playersTransfroms.Count);
            m_playerBaseImages  = new List<Text>(playerBasesTranforms.Count);

            for (int i = 0; i < m_playerImages.Capacity; ++i)
            {
                Text playerIcon     = Instantiate(playerImage);
                Text playerBaseIcon = Instantiate(baseImage);

                // Set parent as canvas
                playerIcon.transform.SetParent(m_canvas.transform, false);
                playerBaseIcon.transform.SetParent(m_canvas.transform, false);

                // Set scale
                playerIcon.transform.localScale = 
                    new Vector3(playerIconScale, playerIconScale, playerIconScale);
                playerBaseIcon.transform.localScale = 
                    new Vector3(playerBaseIconScale, playerBaseIconScale, playerBaseIconScale);

                switch (i)
                {
                        // Player 1
                    case 0:
                        playerIcon.color        = player1Colour;
                        playerBaseIcon.color    = player1Colour;
                        break;

                        // Player 2
                    case 1:
                        playerIcon.color        = player2Colour;
                        playerBaseIcon.color    = player2Colour;
                        break;

                        // Player 3
                    case 2:
                        playerIcon.color        = player3Colour;
                        playerBaseIcon.color    = player3Colour;
                        break;

                        // Player 4
                    case 3:
                        playerIcon.color        = player4Colour;
                        playerBaseIcon.color    = player4Colour;
                        break;
                }

                m_playerImages.Add(playerIcon);
                m_playerBaseImages.Add(playerBaseIcon);
            }
        }

		void Start() 
		{
		    
		}
		
		void Update() 
		{
		    // HACK: Should ensure that this code only executes
            // when needed within production code

            // HACK: Make this code function with X players rather than 4

            // Update base icons
            for (int i = 0; i < m_playerImages.Count; ++i)
            {
                Transform playerTrans   = playersTransfroms[i];
                Text playerIcon         = m_playerImages[i];

                //playerIcon.transform.position = m_camera.WorldToScreenPoint(playerTrans.position);
                //playerIcon.rectTransform.position = WorldToCanvas(m_canvas, playerTrans.position, m_camera);
                playerIcon.transform.position = playerTrans.position;
            }

            // Update base icons
            for (int i = 0; i < m_playerBaseImages.Count; ++i)
            {
                Transform playerBaseTrans   = playerBasesTranforms[i];
                Text playerBaseIcon         = m_playerBaseImages[i];

                //playerBaseIcon.transform.position = m_camera.WorldToScreenPoint(playerBaseTrans.position);
                //playerBaseIcon.rectTransform.position = WorldToCanvas(m_canvas, playerBaseTrans.position, m_camera);
                playerBaseIcon.transform.position = playerBaseTrans.position;
            }
		}

        Vector2 WorldToCanvas(Canvas a_canvas, Vector3 a_worldPosition, Camera a_camera)
        {
            Vector3 viewportPos         = a_camera.WorldToViewportPoint(a_worldPosition);
            RectTransform canvasRect    = a_canvas.GetComponent<RectTransform>();

            return new Vector2((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
                               (viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f));
        }
	}
}
