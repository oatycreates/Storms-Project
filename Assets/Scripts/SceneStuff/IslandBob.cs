/**
 * File: IslandBob.cs
 * Author: Patrick Ferguson
 * Maintainers: 
 * Created: 10/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Aesthetic island bobbing animation.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class IslandBob : MonoBehaviour 
	{
        /// <summary>
        /// How large to make the movement up and down.
        /// </summary>
        public float bobAmplitude = 10.0f;

        /// <summary>
        /// Speed at which to bob.
        /// </summary>
        public float bobSpeed = 50.0f;

        private float m_timer = 0;

        private float m_direction = 1;

        // Cached variables
        private Vector3 m_startPos = Vector3.zero;
        private Transform m_trans = null;

        void Awake()
        {
            m_trans = transform;
            m_startPos = m_trans.position;
        }

		void Start()
        {
            //timer = Random.value * 500;
            m_direction = Random.value >= 0.5f ? 1 : -1;
            bobAmplitude += ((Random.value - 0.5f) * 2) * bobAmplitude * 0.5f;
		}
		
		void Update() 
		{
            m_timer += Time.deltaTime;

		    // Bob
            Vector3 goalPos = Vector3.up * bobAmplitude * Mathf.Sin(bobSpeed * m_timer * m_direction);
            m_trans.position = m_startPos + goalPos;
		}
	}
}
