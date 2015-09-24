/**
 * File: ScoreIndicator.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 24/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Animates the score bar indicators of the minimap
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class ScoreIndicator : MonoBehaviour 
	{
        [Range(0.0f, 1.0f)]
        [Tooltip("Decimal percentage of the player's score, to win")]
        public float m_scorePercent     = 1.0f;
        [Tooltip("Speed in UV units per second")]
        public float m_animationSpeed  = 0.05f;

        // Used for setting the Y texture offset
        private float m_offsetValueY = 0.51f;

        // Cached variables
        private Material m_material;

        /// <summary>
        /// Should be a value within the range 0, 1
        /// </summary>
        public float scorePercent
        {
            set
            {
                m_scorePercent = value;
            }
        }

        /// <summary>
        /// UV offset in seconds, used for bar animation
        /// </summary>
        public float animationSpeed
        {
            set
            {
                m_animationSpeed = value;
            }
        }

        public void Awake()
        {
            m_material = GetComponent<Renderer>().sharedMaterial;
        }

		void Start() 
		{
            // Ensure starting texture offset is resonable
            m_material.SetTextureOffset("_MainTex", new Vector2(0.0f, m_offsetValueY));
		}
		
		void Update() 
		{
            Vector2 textureOffset = m_material.mainTextureOffset;
            Vector2 detailTexOffset = m_material.GetTextureOffset("_DetailAlbedoMap");

            if (textureOffset.x > 1.0f)
            {
                // Keep offset within limits
                textureOffset.x = 0.0f;
            }

            if (m_scorePercent >= 1.0f)
            {
                // Freeze animation
                textureOffset.x = 0.0f;
            }
            else
            {
                // Animate
                textureOffset.x     += m_animationSpeed * Time.deltaTime;
            }

            detailTexOffset.y += m_animationSpeed * Time.deltaTime;

            // Set Y offset - displays score percent
            textureOffset.y = 0.5f - (m_offsetValueY * m_scorePercent);

            m_material.SetTextureOffset("_MainTex", textureOffset);
            m_material.SetTextureOffset("_DetailAlbedoMap", detailTexOffset);
		}
	}
}
