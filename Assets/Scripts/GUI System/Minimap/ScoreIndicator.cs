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
        [Header("Texture References")]
        public Texture2D normalAlbedo;
        public Texture2D flippedAlbedo;

        public Texture2D navyTexture;
        public Texture2D pirateTexture;
        public Texture2D tinkererTexture;
        public Texture2D vikingTexture;

        [Header("Configuration")]
        public Faction faction = Faction.NONE;

        [Range(0.0f, 1.0f)]
        [Tooltip("Decimal percentage of the player's score, to win")]
        public float m_scorePercent     = 1.0f;
        [Tooltip("Speed in UV units per second")]
        public float m_animationSpeed  = 0.05f;

        public bool m_antiClockwiseAnimation = true;

        // Used for setting the Y texture offset
        private float m_offsetValueY = 0.5f;

        // Cached variables
        private Renderer m_renderer;
        private Texture2D m_emptyTexture;

        /// <summary>
        /// Should be a value within the range 0, 1
        /// </summary>
        public float scorePercent
        {
            get
            {
                return m_scorePercent;
            }

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
            // Save reference to renderer
            m_renderer = GetComponent<Renderer>();

            // Store current material texture, for when faction is set to NONE
            m_emptyTexture = (Texture2D)m_renderer.material.GetTexture("_MainTex");
        }

		void Start() 
		{
            // Ensure starting texture offset is resonable
            m_renderer.material.SetTextureOffset("_MainTex", new Vector2(0.0f, m_offsetValueY));
		}
		
		void Update() 
		{
            SetTextures();

            Vector2 textureOffset   = m_renderer.material.mainTextureOffset;
            Vector2 detailTexOffset = m_renderer.material.GetTextureOffset("_DetailAlbedoMap");

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
                if (!m_antiClockwiseAnimation)
                {
                    textureOffset.x -= m_animationSpeed * Time.deltaTime;
                }
                else
                {
                    textureOffset.x += m_animationSpeed * Time.deltaTime;
                }
            }

            if (!m_antiClockwiseAnimation)
            {
                detailTexOffset.y -= m_animationSpeed * Time.deltaTime;
            }
            else
            {
                detailTexOffset.y += m_animationSpeed * Time.deltaTime;
            }

            // Set Y offset - display's score percent
            if (m_antiClockwiseAnimation)
            {
                textureOffset.y = -m_offsetValueY + (m_offsetValueY * m_scorePercent) + 0.01f;
            }
            else
            {
                textureOffset.y = (m_offsetValueY * m_scorePercent);
                textureOffset.y *= -1.0f;
            }

            m_renderer.material.SetTextureOffset("_MainTex", textureOffset);
            m_renderer.material.SetTextureOffset("_DetailAlbedoMap", detailTexOffset);
		}

        void SetTextures()
        {
            if (m_antiClockwiseAnimation)
            {
                m_renderer.material.SetTexture("_MainTex", flippedAlbedo);
            }
            else
            {
                m_renderer.material.SetTexture("_MainTex", normalAlbedo);
            }

            switch (faction)
            {
                case Faction.NAVY:
                    m_renderer.material.SetTexture("_DetailAlbedoMap", navyTexture);
                    break;

                case Faction.PIRATES:
                    m_renderer.material.SetTexture("_DetailAlbedoMap", pirateTexture);
                    break;

                case Faction.TINKERERS:
                    m_renderer.material.SetTexture("_DetailAlbedoMap", tinkererTexture);
                    break;

                case Faction.VIKINGS:
                    m_renderer.material.SetTexture("_DetailAlbedoMap", vikingTexture);
                    break;

                case Faction.NONE:
                    m_renderer.material.SetTexture("_MainTex", m_emptyTexture);
                    break;
            }
        }
	}
}