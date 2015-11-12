/**
 * File: MasterCamera.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the setup of each scene camera. Stretches the camera if there are different numbers of players.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    public enum ECamerasInScene
    {
        One,
        Two,
        Three,
        Four
    };

    /// <summary>
    /// A very important script. This sets the camera's screen width/height depending on the number of players.
    /// For now, this has to be set manually.
    /// </summary>
    public class MasterCamera : MonoBehaviour
    {
        public ECamerasInScene currentCamera;

        public Camera cam1;
        public Camera cam2;
        public Camera cam3;
        public Camera cam4;

        private bool m_isInitialised = false;
        

        void Awake()
        {
			
        }

        void Update()
        {
            if (!m_isInitialised)
            {
                Debug.LogWarning("InitialiseMasterCamera() has not been run!");
            }
        }

        /// <summary>
        /// Should be called after all player cameras have been initialised.
        /// </summary>
        public void InitialiseMasterCamera()
        {
            m_isInitialised = true;

            if (currentCamera == ECamerasInScene.One)
            {
                cam1.enabled = true;
                if (cam2 != null)
                {
                    cam2.enabled = false;
                }
                if (cam3 != null)
                {
                    cam3.enabled = false;
                }
                if (cam4 != null)
                {
                    cam4.enabled = false;
                }

                cam1.rect = new Rect(0, 0, 1, 1);
            }

            if (currentCamera == ECamerasInScene.Two)
            {
                cam1.enabled = true;
                cam2.enabled = true;
                if (cam3 != null)
                {
                    cam3.enabled = false;
                }
                if (cam4 != null)
                {
                    cam4.enabled = false;
                }

                // One on top
                cam1.rect = new Rect(0f, 0.5f, 1f, 0.5f);
                cam2.rect = new Rect(0, 0.0f, 1f, 0.5f);

            }

            if (currentCamera == ECamerasInScene.Three)
            {
                cam1.enabled = true;
                cam2.enabled = true;
                cam3.enabled = true;
                if (cam4 != null)
                {
                    cam4.enabled = false;
                }

                // Cam two top right
                cam1.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                cam2.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                //cam3.rect = new Rect(0, 0, 1f, 0.5f);
                cam3.rect = new Rect(0.25f, 0f, 0.5f, 0.5f);

            }

            if (currentCamera == ECamerasInScene.Four)
            {
                cam1.enabled = true;
                cam2.enabled = true;
                cam3.enabled = true;
                cam4.enabled = true;

                cam1.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                cam2.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
                cam3.rect = new Rect(0f, 0f, 0.5f, 0.5f);
                cam4.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
            }
        }
    } 
}
