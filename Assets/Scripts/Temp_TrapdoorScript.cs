/**
 * File: Temp_TrapdoorScript.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 28/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This temp script is used to trigger the trapdoor -- replace this with Animation when ready.
 *              Most of this script was copied off the 'HingeJointScript'.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This temp script is used to trigger the trapdoor -- replace this with Animation when ready.
    /// Most of this script was copied off the 'HingeJointScript'.
    /// </summary>
    public class Temp_TrapdoorScript : MonoBehaviour
    {
        private float rotateAmount;
        public float maxRotationInDegrees;
        private float speed = 3.0f;

        public AirshipControlBehaviour controller;
        // Check the input manager for inputs
        private bool buttonPressed = false;
        public bool flipDirection = false;

        private float turnValue;

        // Cached variables
        private Transform m_trans;

        void Awake()
        {
            m_trans = transform;
        }

        void Start()
        {
            rotateAmount = 0;
        }

        void FixedUpdate()
        {
            if (controller != null)
            {
                buttonPressed = controller.openHatch;
            }

            if (buttonPressed)
            {
                if (!flipDirection)
                {
                    rotateAmount = Mathf.Lerp(rotateAmount, maxRotationInDegrees, Time.deltaTime * speed);
                }
                else if (flipDirection)
                {
                    rotateAmount = Mathf.Lerp(rotateAmount, -maxRotationInDegrees, Time.deltaTime * speed);
                }
            }
            else if (!buttonPressed)
            {
                // Return to normal
                rotateAmount = Mathf.Lerp(rotateAmount, 0, Time.deltaTime * speed);
            }

            SetAxis();
        }

        void SetAxis()
        {
            Vector3 localRot = m_trans.localEulerAngles;
            m_trans.localRotation = Quaternion.Euler(localRot.x, localRot.y, rotateAmount);
        }

    } 
}
