/**
 * File: Temp_CamPos_Behaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 16/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This changes the cam position while dropping off passengers.
 *              Like the Temp_TrapdoorScript, this script only needs to work for now...
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This changes the cam position while dropping off passengers.
    /// Like the Temp_TrapdoorScript, this script only needs to work for now...
    /// </summary>
    public class Temp_CamPos_Behaviour : MonoBehaviour
    {
        private Vector3 localCamStartPos;
        private Vector3 localCamStartEuler;

        public AirshipControlBehaviour controller;
        private bool buttonDown;

        public Vector3 targetPos = new Vector3(0, 20, 11);
        public Vector3 targetEuler = new Vector3(90, 0, 0);
        public float speed = 2.0f;

        // Cached variables
        private Transform m_trans = null;

        void Awake()
        {
            m_trans = transform;
        }

        void Start()
        {
            localCamStartPos = m_trans.localPosition; // Usually (0, 12, -25)
            localCamStartEuler = m_trans.localEulerAngles; // Usually (10, 0, 0)
        }

        void Update()
        {
            // From world space to local space
            //Vector3 localTargetPos = m_trans.TransformDirection(targetPos);
            //Vector3 localTargetEuler = m_trans.TransformDirection(targetEuler);

            if (controller != null)
            {
                buttonDown = controller.openHatch;
            }

            if (buttonDown)
            {
                m_trans.localPosition = Vector3.Lerp(m_trans.localPosition, targetPos, Time.deltaTime * speed);
                m_trans.localEulerAngles = Vector3.Lerp(m_trans.localEulerAngles, targetEuler, Time.deltaTime * speed);
            }
            else if (!buttonDown)
            {
                m_trans.localPosition = Vector3.Lerp(m_trans.localPosition, localCamStartPos, Time.deltaTime * speed);
                m_trans.localEulerAngles = Vector3.Lerp(m_trans.localEulerAngles, localCamStartEuler, Time.deltaTime * speed);
            }
        }
    } 
}
