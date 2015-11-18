/**
 * File: FactionPreviewDisplay.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 9/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Updates the faction preview display UI element
 **/

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace ProjectStorms
{
    public class FactionPreviewDisplayUpdater : MonoBehaviour, IPointerEnterHandler
    {
        [Header("Render Textures")]
        public Texture renderTexture;

        [Header("References")]
        public RawImage m_rawImage;

        private Button m_button;

        public void Awake()
        {
            m_button = GetComponent<Button>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (m_button.interactable)
            {
                m_rawImage.texture = renderTexture;
            }
        }
        
        public void IsHighlighted( BaseEventData eventSystem)
        {
			if (m_button.interactable)
			{
				m_rawImage.texture = renderTexture;
			}
        }
    }
}
