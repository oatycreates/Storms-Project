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

		/// <summary>
		/// Screenshake	function variables
		/// </summary>
		//Timer used for determining how long the camera should continue shaking for.
		private float shakeTime = 0.0f;
		//The amount that the camera should shake.
		private float shakeAmount = 1.0f;


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
			if (Input.GetKeyDown(KeyCode.Space))
			{
				ShakeCam(0.7f, 1.5f);
			}

			shakeTime -= Time.deltaTime;

			//This has been updated - If cam IS following and screenshake is NOT in effect, then make the camera move to the right position.
            if (camFollowPlayer)
            {
				if (shakeTime < 0)
				{
                	FollowCam();
				}
				else
				{
					Shaking();
				}
            }
            else if (!camFollowPlayer)
            {
                WatchCam();
            }
        }

		/// <summary>
		/// This triggers the camera shake.		
		/// </summary>
		public void ShakeCam(float shakeDuration, float shakeStrength)
		{
			//Recommend shakeDuration = 0.6f and shakeStrength = 1.5f;

			//Set shake time
			shakeTime = shakeDuration;
			shakeAmount = shakeStrength;
			
			//This changes the values in the update function.
		}

        public void FollowCam()
        {
            if (camPosTarget != null)
            {
                // TODO Fix lerping
                m_trans.position = camPosTarget.position;
            }

            if (camLookTarget != null)
            {
                m_trans.rotation = camRotator.rotation;
            }
        }


        public void SuicideCam()
        {
            m_trans.rotation = Quaternion.LookRotation(camLookTarget.position - m_trans.position);
        }

        public void WatchCam()
        {
            if (camLookTarget != null)
            {
                m_trans.rotation = Quaternion.LookRotation(camLookTarget.position - m_trans.position);
            }
        }

        /// <summary>
        /// Reset the camera back for the roulette state.
        /// </summary>
        public void RouletteCam()
        {
            m_trans.position = m_myStartPos;
            m_trans.rotation = m_myStartRot;
        }

		/// <summary>
		/// This determines the shaking behaviour.
		/// </summary>
		private void Shaking()
		{
			if (camPosTarget != null)
			{
				Vector3 tempPos = camPosTarget.position;
				m_trans.position = tempPos + Random.insideUnitSphere * shakeAmount;

				//Look at target.
				m_trans.rotation = camRotator.rotation;
			}

			//Make my controller vibrate while camera shakes
			InputManager.SetControllerVibrate (gameObject.tag, 1, 1, shakeTime);
		}
	} 
}
