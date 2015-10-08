/**
 * File: MenuSFX.cs
 * Author: Rowan Donaldson
 * Maintainer: Pat Ferguson
 * Created: 24/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Triggers SFX for menu navigation
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace ProjectStorms
{
    [RequireComponent(typeof(AudioSource))]
    public class MenuSFX : MonoBehaviour, ISelectHandler, IPointerDownHandler
    {
        private AudioSource m_myAudioSource;

        void Start()
        {
            m_myAudioSource = gameObject.GetComponent<AudioSource>();
        }

        public void OnSelect(BaseEventData eventData)
        {
            //print(this.gameObject.name + " Was Selected");

            if (!m_myAudioSource.isPlaying)
            {
                m_myAudioSource.pitch = 1.0f;
                m_myAudioSource.Play();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            print(this.gameObject.name + "Was Clicked.");

            if (!m_myAudioSource.isPlaying)
            {
                m_myAudioSource.pitch = 0.5f;
                m_myAudioSource.Play();
            }
        }
        
    }
}
