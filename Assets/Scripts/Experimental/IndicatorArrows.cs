/**
 * File: IndicatorArrows.cs
 * Author: Patrick Ferguson
 * Maintainers: 
 * Created: 16/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Shows where other players are relative to the player's screen.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class IndicatorArrows : MonoBehaviour
    {
        /// <summary>
        /// The player prefab.
        /// </summary>
        public GameObject playerPrefab = null;

        /// <summary>
        /// The arrows to draw.
        /// 0 - left
        /// 1 - right
        /// 2 - top
        /// 3 - down
        /// 4 - top-left
        /// 5 - top-right
        /// 6 - bottom-right
        /// 7 - bottom-left
        /// </summary>
        public Texture2D[] arrows;

        /// <summary>
        /// Distance from each corner to consider as a corner.
        /// </summary>
        private float m_diagMargin = 32;

        void Awake()
        {
            // Make the margin as big as the diagonal textures
            m_diagMargin = Mathf.Max(arrows[4].width, arrows[4].height) / 2.0f;
        }

		void Start() 
		{
		
		}
		
		void Update() 
		{
		
		}

        /// <summary>
        /// Raises the GUI render event.
        /// </summary>
        void OnGUI()
        {
            // TODO: Update this code to function with the updated Unity 5 canvas APIs.

            Camera cam = GetComponent<Camera>();

            UpdateIndicatorArrows(cam);
        }

        private void UpdateIndicatorArrows(Camera a_cam)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag(playerPrefab.name);
            foreach (GameObject player in objs)
            {
                Vector2 projPlayer = a_cam.WorldToViewportPoint(player.transform.position);

                // Expand the camera's rect by the texture's size to improve visuals
                Rect camRect = a_cam.rect;
                camRect.xMin -= m_diagMargin;
                camRect.xMax += m_diagMargin;
                camRect.yMin -= m_diagMargin;
                camRect.yMax += m_diagMargin;

                // Only draw arrows if the player is off the screen
                if (!a_cam.rect.Contains(projPlayer))
                {
                    float halfScreenWidth = a_cam.pixelWidth / 2.0f;
                    float halfScreenHeight = a_cam.pixelHeight / 2.0f;

                    // Calculate offset from center of screen
                    //projPlayer -= new Vector2(0.5f, 0.5f);

                    // Scale to window size
                    //projPlayer.Normalize();
                    //projPlayer.x *= halfScreenWidth;
                    //projPlayer.y *= halfScreenHeight;

                    // Convert back to screen position
                    //projPlayer += new Vector2(halfScreenWidth, halfScreenHeight);

                    projPlayer.x *= a_cam.pixelWidth;
                    projPlayer.y *= a_cam.pixelHeight;

                    Rect screenCoords = Rect.MinMaxRect(0, 0, 1, 1);
                    Texture2D arrowTex = null;

                    if (projPlayer.x > m_diagMargin && projPlayer.x < a_cam.pixelWidth - m_diagMargin)
                    {
                        // Top or bottom half of the screen

                        if (projPlayer.y >= halfScreenHeight)
                        {
                            // Top half
                            arrowTex = arrows[2];
                            screenCoords = new Rect(projPlayer.x - arrowTex.width / 2.0f, 0,
                                                    arrowTex.width, arrowTex.height);
                        }
                        else
                        {
                            // Bottom half
                            arrowTex = arrows[3];
                            screenCoords = new Rect(projPlayer.x - arrowTex.width / 2.0f, a_cam.pixelHeight - arrowTex.height,
                                                    arrowTex.width, arrowTex.height);
                        }
                    }
                    else if (projPlayer.y > m_diagMargin && projPlayer.y < a_cam.pixelHeight - m_diagMargin)
                    {
                        // Left or Right half of the screen

                        if (projPlayer.x <= halfScreenWidth)
                        {
                            // Left half
                            arrowTex = arrows[0];
                            screenCoords = new Rect(0, a_cam.pixelHeight - projPlayer.y - arrowTex.height / 2,
                                                    arrowTex.width, arrowTex.height);
                        }
                        else
                        {
                            // Right half
                            arrowTex = arrows[1];
                            screenCoords = new Rect(a_cam.pixelWidth - arrowTex.width, a_cam.pixelHeight - projPlayer.y - arrowTex.height / 2,
                                                    arrowTex.width, arrowTex.height);
                        }
                    }
                    else
                    {
                        // Corners

                        if (projPlayer.x <= m_diagMargin)
                        {
                            // Top or bottom left corners

                            if (projPlayer.y >= halfScreenHeight)
                            {
                                // Top left corner
                                arrowTex = arrows[4];
                                screenCoords = new Rect(0, 0,
                                                        arrowTex.width, arrowTex.height);
                            }
                            else
                            {
                                // Bottom left corner
                                arrowTex = arrows[7];
                                screenCoords = new Rect(0, a_cam.pixelHeight - arrowTex.height,
                                                        arrowTex.width, arrowTex.height);
                            }
                        }
                        else if (projPlayer.x >= a_cam.pixelWidth - m_diagMargin)
                        {
                            // Top or bottom right corners

                            if (projPlayer.y >= halfScreenHeight)
                            {
                                // Top right corner
                                arrowTex = arrows[5];
                                screenCoords = new Rect(a_cam.pixelWidth - arrowTex.width, 0,
                                                        arrowTex.width, arrowTex.height);
                            }
                            else
                            {
                                // Bottom right corner
                                arrowTex = arrows[6];
                                screenCoords = new Rect(a_cam.pixelWidth - arrowTex.width, a_cam.pixelHeight - arrowTex.height,
                                                        arrowTex.width, arrowTex.height);
                            }
                        }
                    }

                    if (arrowTex != null)
                    {
                        GUI.DrawTexture(screenCoords, arrowTex);
                    }
                }
            }
        }
	}
}
