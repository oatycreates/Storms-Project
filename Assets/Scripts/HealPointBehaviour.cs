/**
 * File: HealPointBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 28/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Heals the player when they enter the healing trigger zone.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// Heals the player when they enter the healing trigger zone.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class HealPointBehaviour : MonoBehaviour
    {
        public ParticleSystem m_childParticles;

        private AudioSource m_mySource;
       // private AudioSource[] m_childSources; 

        public AudioSource m_childStatic;
        public AudioClip[] repairSounds;

        void Start()
        {
            m_mySource = gameObject.GetComponent<AudioSource>();
            m_mySource.volume = 3;

            //start with no particles
            m_childParticles.Stop();
        }

        void RandomClip()
        {
            m_mySource.clip = repairSounds[Random.Range(0, repairSounds.Length)];
            RandomPitch();
            m_mySource.Play();
            print(m_mySource.clip.name);
        }

        void RandomPitch()
        {
            m_mySource.pitch = Random.Range(0.85f, 1.15f);
        }
      
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player1_") || other.CompareTag("Player2_") || other.CompareTag("Player3_") || other.CompareTag("Player4_"))
            {
                ShipPartDestroy partScript = other.GetComponentInParent<ShipPartDestroy>();
                if (partScript != null)
                {
                    // Found in parent, repair
                    partScript.RepairAllParts();
                }
                else
                {
                    // Look to children for the part object
                    partScript = other.GetComponentInChildren<ShipPartDestroy>();
                    if (partScript != null)
                    {
                        partScript.RepairAllParts();
                    }
                }

            }
        }

        void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player1_") || other.CompareTag("Player2_") || other.CompareTag("Player3_") || other.CompareTag("Player4_"))
            {
                //If an airship is in the trigger zone, play a sound
                if (!m_mySource.isPlaying)
                {
                    RandomClip();
                }

                //Make the static noise only play if a player is in the trigger zone.
                if (!m_childStatic.isPlaying)
                {
                    m_childStatic.Play();
                }

                //
                //Trigger particles
                if (!m_childParticles.isPlaying)
                {
                    m_childParticles.Play();
                }
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player1_") || other.CompareTag("Player2_") || other.CompareTag("Player3_") || other.CompareTag("Player4_"))
            {
                if (m_childStatic.isPlaying)
                {
                    m_childStatic.Stop();
                }

                if (m_childParticles.isPlaying)
                {
                    m_childParticles.Stop();
                }
            }
        }
    } 
}
