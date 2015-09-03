/**
 * File: AirshipCamBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the camera's following of each player.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// Basic lerp follow airship. Don't change the cam height/width/pixel position here - do that in the master cam controller.
    /// However, we can tell the camera where to go and what position to take here.
    /// </summary>
    public class AirshipCamBehaviour : MonoBehaviour
    {
        [HideInInspector]
        public bool camFollowPlayer = true;

        public Transform camPosTarget = null;
        public Transform camRotator = null;
        public Transform camLookTarget = null;

        /// <summary>
        /// Used to slerp look direction.
        /// </summary>
        public float camLookSmooth = 2.0f;

        /// <summary>
        /// Used to lerp cam pos.
        /// </summary>
        public float camPosSmooth = 2.0f;

        /// <summary>
        /// Keep a reference to the start position, so we can reset to the roulette position.
        /// </summary>
        private Vector3 m_myStartPos;
        private Quaternion m_myStartRot;

        // Cached variables
        Transform m_trans;

        void Awake()
        {
            m_trans = transform;
        }

        void Start()
        {
            GameObject camHolder = GameObject.Find("CamHolder");
            if (camHolder == null)
            {
                camHolder = new GameObject();
                camHolder.name = "CamHolder";
            }

            // Detach from parent on start!
            m_trans.SetParent(camHolder.transform, true);

            m_myStartPos = m_trans.position;
            m_myStartRot = m_trans.rotation;

            m_trans.localPosition = Vector3.zero;
        }

        void Update()
        {
            if (camFollowPlayer)
            {
                FollowCam();
            }
            else if (!camFollowPlayer)
            {
                WatchCam();
            }
        }

        public void FollowCam()
        {
            if (camPosTarget != null)
            {
                // TODO Fix lerping
                //m_trans.position = Vector3.Lerp(m_trans.position, camPosTarget.position, Time.deltaTime * camPosSmooth);
                m_trans.position = camPosTarget.position;
            }

            if (camLookTarget != null)
            {
                //Quaternion tar = Quaternion.LookRotation(camLookTarget.position - m_trans.position);
                //m_trans.rotation = Quaternion.Slerp(m_trans.localRotation, tar, Time.deltaTime * camLookSmooth);
                //m_trans.rotation = Quaternion.LookRotation(camLookTarget.position - m_trans.position);
                m_trans.rotation = camRotator.rotation;
            }

        }


        public void SuicideCam()
        {
            //Quaternion tar = Quaternion.LookRotation(camLookTarget.position - m_trans.position);
            //m_trans.rotation = Quaternion.Slerp(m_trans.localRotation, tar, Time.deltaTime * camLookSmooth);
            m_trans.rotation = Quaternion.LookRotation(camLookTarget.position - m_trans.position);
        }

        public void WatchCam()
        {
            if (camLookTarget != null)
            {
                //Quaternion tar = Quaternion.LookRotation(camLookTarget.position - m_trans.position);
                //m_trans.rotation = Quaternion.Slerp(m_trans.localRotation, tar, Time.deltaTime * camLookSmooth);
                m_trans.rotation = Quaternion.LookRotation(camLookTarget.position - m_trans.position);
            }
        }

        /// <summary>
        /// Reset the camera back for the roulette state.
        /// </summary>
        public void RouletteCam()
        {
            //m_trans.parent = rememberMyParent.transform;
            //m_trans.LookAt(camPosTarget.transform.position);

            m_trans.position = m_myStartPos;
            m_trans.rotation = m_myStartRot;
        }
    } 
}
