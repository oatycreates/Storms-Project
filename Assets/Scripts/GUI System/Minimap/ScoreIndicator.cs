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
        [Range(0.0f, 0.5f)]
        public float scorePercent = 0.5f;
        public float animateSpeed = 0.05f;

        Material m_material;

        public void Awake()
        {
            m_material = GetComponent<Renderer>().sharedMaterial;
        }

		void Start() 
		{
			
		}
		
		void Update() 
		{
            Vector2 textureOffset = m_material.mainTextureOffset;

            if (textureOffset.x > 1.0f)
            {
                textureOffset.x = 0.0f;
            }

            m_material.SetTextureOffset("_MainTex", new Vector2(textureOffset.x + (animateSpeed * Time.deltaTime), scorePercent));
		}
	}
}
