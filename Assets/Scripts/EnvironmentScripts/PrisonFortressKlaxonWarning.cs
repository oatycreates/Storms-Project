/**
 * File: PrisonFortressKlaxonWarning.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script makes a klaxon sound as a warning, before the fortress jettisons prisoners.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// Existance state of the prison fortress.
    /// </summary>
    public enum EFortressStates
    {
        Dormant,
        Warning,
        Spawning
    }

    /// <summary>
    /// This script makes a klaxon sound as a warning, before the fortress jettisons prisoners.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class PrisonFortressKlaxonWarning : MonoBehaviour
    {
        public EFortressStates fortressState;

        public float timeBetweenPrisonerDrops = 160.0f;
        private float m_timer;

        public int numberOfTimesKlaxonShouldSound = 3;
        private int m_rememberStartValue;

        public float timePassengerSpawnFor = 15.0f;
        private float m_rememberPassengerTimerValue;


        private AudioSource m_mySource;

        public Light sternLight;
        public Light hullLight;

        public float maxLightIntensity = 4.0f;
        /// <summary>
        /// Should the lights be increasing or decreasing in intensity.
        /// </summary>
        private bool m_lightUp = false;

        public AudioClip klaxonSound;
        private bool m_warningCanPlay = true;
        private bool m_spawningCanPlay = true;

        public SpawnPassengers sternPassengerSpawn;
        public SpawnPassengers hullPassengerSpawn;

        private Color orange;

        void Start()
        {
            m_mySource = gameObject.GetComponent<AudioSource>();
            m_mySource.volume = 0.3f;
            sternLight.intensity = 0.0f;
            hullLight.intensity = 0.0f;

            m_timer = timeBetweenPrisonerDrops;
            m_rememberStartValue = numberOfTimesKlaxonShouldSound;
            m_rememberPassengerTimerValue = timePassengerSpawnFor;

            orange = new Color(1f, 0.75f, 0f);
        }

        void Update()
        {
            ClampLights();

            m_timer -= Time.deltaTime;

            if (m_timer < 0)
            {
                if (fortressState == EFortressStates.Dormant)
                {
                    fortressState = EFortressStates.Warning;
                }
            }


            // State Management
            if (fortressState == EFortressStates.Dormant)
            {
                m_lightUp = false;
                m_warningCanPlay = true;
                m_spawningCanPlay = true;
                sternPassengerSpawn.currentlySpawning = false;
                hullPassengerSpawn.currentlySpawning = false;
                timePassengerSpawnFor = m_rememberPassengerTimerValue;
            }


            if (fortressState == EFortressStates.Warning)
            {
                // Make sure the klaxon doesn't go on forever
                if (numberOfTimesKlaxonShouldSound > 0)
                {

                    if (!m_mySource.isPlaying)
                    {
                        // This is just to make sure that Warning IS NOT invoked more than once in the update.
                        if (m_warningCanPlay)
                        {
                            Invoke("Warning", 1.0f);
                            m_lightUp = false;
                            m_warningCanPlay = false;

                            numberOfTimesKlaxonShouldSound -= 1;
                        }
                    }
                }
                else
                    if (numberOfTimesKlaxonShouldSound <= 0)
                    {
                        // Change state
                        fortressState = EFortressStates.Spawning;
                    }
            }


            if (fortressState == EFortressStates.Spawning)
            {
                m_warningCanPlay = true;

                if (m_spawningCanPlay)
                {
                    Invoke("Spawning", 4.5f);
                    m_spawningCanPlay = false;
                }

                timePassengerSpawnFor -= Time.deltaTime;

                if (timePassengerSpawnFor < 0)
                {
                    fortressState = EFortressStates.Dormant;
                    m_timer = timeBetweenPrisonerDrops;
                }

                // Reset the number of times the klaxon should sound
                numberOfTimesKlaxonShouldSound = m_rememberStartValue;
            }
        }


        void ClampLights()
        {
            if (sternLight.intensity < 0.0f)
            {
                sternLight.intensity = 0.0f;
            }

            if (sternLight.intensity > maxLightIntensity)
            {
                sternLight.intensity = maxLightIntensity;
            }

            if (hullLight.intensity < 0.0f)
            {
                hullLight.intensity = 0.0f;
            }

            if (hullLight.intensity > maxLightIntensity)
            {
                hullLight.intensity = maxLightIntensity;
            }

            // Increase or decrease lights
            if (m_lightUp)
            {
                sternLight.intensity += 0.05f;
                hullLight.intensity += 0.05f;
            }
            else
                if (!m_lightUp)
                {
                    sternLight.intensity -= 0.5f;
                    hullLight.intensity -= 0.5f;
                }
        }

        /// <summary>
        /// Warning is called before Spawning.
        /// </summary>
        void Warning()
        {
            m_mySource.clip = klaxonSound;

            Color stateColour = orange; //now Orange
            sternLight.color = stateColour;
            hullLight.color = stateColour;

            UpdateSpawnerLaserColour(stateColour);

            m_lightUp = true;

            m_mySource.Play();

            m_warningCanPlay = true;
            sternPassengerSpawn.currentlySpawning = false;
            hullPassengerSpawn.currentlySpawning = false;
        }

        private void UpdateSpawnerLaserColour(Color a_baseColour)
        {
            Color laserColour = a_baseColour;
            if (sternPassengerSpawn.spawnHelperLaser != null)
            {
                laserColour.a = sternPassengerSpawn.GetSpawnLaserAlpha();
                sternPassengerSpawn.spawnHelperLaser.SetColors(laserColour, laserColour);
            }
            if (hullPassengerSpawn.spawnHelperLaser != null)
            {
                laserColour.a = hullPassengerSpawn.GetSpawnLaserAlpha();
                hullPassengerSpawn.spawnHelperLaser.SetColors(laserColour, laserColour);
            }
        }


        /// <summary>
        /// Spawning is called repeatedly for a bit, before returning to Warning.
        /// </summary>
        void Spawning()
        {
            Color stateColour = Color.red;
            sternLight.color = stateColour;
            hullLight.color = stateColour;
            m_lightUp = true;

            // Update spawn laser colour
            UpdateSpawnerLaserColour(stateColour);

            sternPassengerSpawn.currentlySpawning = true;
            hullPassengerSpawn.currentlySpawning = true;
        }
    } 
}
