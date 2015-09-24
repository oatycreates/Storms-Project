/**
 * File: Cam_DollyForward.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 20/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script makes the camera 'Dolly' forward or backwards in Local Space (translate forward or back). This is a quick fix to the 'Zoom behind ship issue'.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This script makes the camera 'Dolly' forward or backwards in Local Space (translate forward or back). This is a quick fix to the 'Zoom behind ship issue'.
    /// </summary>
    public class Cam_DollyForward : MonoBehaviour
    {
        // Get movement
        public StateManager airshipStateManager;
        public AirshipControlBehaviour myController;
        public AirshipSuicideBehaviour mySuicideController;

        private float forwardSpeed;

        private float myLocalZ;
        private float myStartZ;

        public float camLerpSpeed = 25.0f;

        public float distanceOne = -25.0f;

        public float distanceTwo = -10.0f;

        private Vector3 m_colForcePos = Vector3.zero;

        // Cached variables
        private Transform m_trans = null;
        private Vector3 m_cachedLocalPos = Vector3.zero;

        void Awake()
        {
            m_trans = transform;
            m_cachedLocalPos = m_trans.localPosition;
        }

        void Start()
        {
            myStartZ = m_trans.localPosition.z;
        }


        void Update()
        {
            myLocalZ = m_trans.localPosition.z;

            // Check which state Im in
            EPlayerState currState = airshipStateManager.GetPlayerState();
            if (currState == EPlayerState.Control)
            {
                forwardSpeed = myController.throttle;
            }
            else if (currState == EPlayerState.Suicide)
            {
                // Make the camera move back a bit
                forwardSpeed = -0.5f;
            }
            else
            {
                forwardSpeed = 0;
            }

            if (forwardSpeed < 0)
            {
                SlideForward();
            }
            else if (forwardSpeed == 0)
            {
                ReturnToNormal();
            }
            else if (forwardSpeed > 0)
            {
                SlideBack();
            }

            if (m_colForcePos != Vector3.zero)
            {
                /*m_trans.localPosition = Vector3.zero;*/
                m_trans.position = m_colForcePos;
            }
            else
            {
                m_trans.localPosition = new Vector3(m_cachedLocalPos.x, m_cachedLocalPos.y, myLocalZ);
            }
        }

        void SlideForward()
        {
            myLocalZ = Mathf.Lerp(myLocalZ, distanceOne, Time.deltaTime * camLerpSpeed);
        }

        void ReturnToNormal()
        {
            myLocalZ = Mathf.Lerp(myLocalZ, myStartZ, Time.deltaTime * camLerpSpeed);
        }

        void SlideBack()
        {
            myLocalZ = Mathf.Lerp(myLocalZ, distanceTwo, Time.deltaTime * camLerpSpeed / 2);
        }

        public void SetCollisionFollowDist(Vector3 a_nearPos)
        {
            m_colForcePos = a_nearPos;
        }

        public Vector3 GetOriginalPosition()
        {
            return m_trans.parent.position + m_trans.parent.transform.rotation * m_cachedLocalPos;
        }
    } 
}
