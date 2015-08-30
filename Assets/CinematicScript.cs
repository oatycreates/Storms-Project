/**
 * File: CinematicScript.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 28/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script manages a basic camera orbiting of the scene to show off level and gameplay features.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This script manages a basic camera orbiting of the scene to show off level and gameplay features.
    /// </summary>
    public class CinematicScript : MonoBehaviour
    {
        public float twirlSpeed = 20;

        public GameObject upDownTarget;
        private float upDown;

        // Cached variables
        private Transform m_trans = null;
        private Transform m_tarTrans = null;

        void Awake()
        {
            m_trans = transform;
            m_tarTrans = upDownTarget.transform;
        }

        void Start()
        {

        }

        void Update()
        {
            // Rotate in local space
            m_trans.Rotate(Vector3.up, twirlSpeed * Time.deltaTime, Space.Self);

            upDown = Mathf.PingPong(Time.time, -3);

            m_tarTrans.position = new Vector3(m_tarTrans.position.x, upDown, m_tarTrans.position.z);
        }
    } 
}
