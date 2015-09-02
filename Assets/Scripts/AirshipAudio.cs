/**
 * File: AirshipAudio.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script controls the creaks and groans of the wood on the airship as it moves.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This script controls the creaks and groans of the wood on the airship as it moves.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class AirshipAudio : MonoBehaviour
    {
        private AudioSource m_mySource;
        public AudioClip[] woodenSounds;

        /// <summary>
        /// Percentage controller motor rumble to apply when the wooden creak sound plays.
        /// </summary>
        public float rumblePerc = 0.5f;

        /// <summary>
        /// Duration of the ship creak controller rumble.
        /// </summary>
        public float rumbleDurr = 0.1f;

        //private float timer;
        private bool m_movement = false;

        // Cached variables
        private float m_pitch   = 0.0f;
        private float m_yaw     = 0.0f;
        private float m_roll    = 0.0f;

        void Start()
        {
            m_mySource = gameObject.GetComponent<AudioSource>();
            m_mySource.volume = 0.20f;
            RandomMe();
        }

        void Update()
        {
            // If movement is True, play a sound.
            // If that sound finishes, and movement is Still True, play a new sound.
            if (m_movement)
            {
                // If source is Not Playing
                if (!m_mySource.isPlaying)
                {
                    RandomMe();
                    m_mySource.Play();

                    // Rumble the controller a bit
                    TriggerControllerRumble();
                }

            }
            else if (!m_movement)
            {
                m_mySource.Stop();
                RandomMe();
            }

        }

        public void AudioInputs(float a_pitch, float a_yaw, float a_roll)//, float a_speed)
        {
            m_pitch = a_pitch;
            m_yaw = a_yaw;
            m_roll = a_roll;

            if ((a_pitch != 0) || (a_yaw != 0) || (a_roll != 0))
            {
                m_movement = true;
            }
            else
            {
                m_movement = false;
            }
        }

        /// <summary>
        /// Calculate controller rumble.
        /// </summary>
        private void TriggerControllerRumble()
        {
            float leftMotor = 0;
            float rightMotor = 0;

            // Move both on pitch up and down
            float absPitch = Mathf.Abs(m_pitch);
            if (absPitch > 0.8f)
            {
                leftMotor += absPitch * rumblePerc;
                rightMotor += absPitch * rumblePerc;
            }

            // Yawing
            float absYaw = Mathf.Abs(m_yaw);
            if (m_yaw < -0.8f)
            {
                // Rumble left on left yaw
                leftMotor += absYaw * rumblePerc;
            }
            else if (m_yaw > 0.8f)
            {
                // Rumble right on right yaw
                rightMotor += absYaw * rumblePerc;
            }

            // Rolling
            float absRoll = Mathf.Abs(m_roll);
            if (m_roll < -0.8f)
            {
                // Rumble right on left roll
                rightMotor += absRoll * rumblePerc;
            }
            else if (m_roll > 0.8f)
            {
                // Rumble left on right roll
                leftMotor += absRoll * rumblePerc;
            }

            // Queue the rumble
            InputManager.SetControllerVibrate(tag, leftMotor, rightMotor, rumbleDurr);
        }

        private void RandomMe()
        {
            // Random clip from array
            m_mySource.clip = woodenSounds[Random.Range(0, woodenSounds.Length)];
            // Give it a random pitch
            m_mySource.pitch = Random.Range(0.75f, 1.5f);
        }
    } 
}
