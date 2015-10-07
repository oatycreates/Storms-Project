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
        /// Timer used for determining how long the camera should continue shaking for.
        /// </summary>
		private float m_shakeTime = 0.0f;
		/// <summary>
        /// The amount that the camera should shake.
		/// </summary>
		private float m_shakeAmount = 1.0f;

        /// <summary>
        /// Transform of the camera child.
        /// </summary>
        private Transform m_camTrans = null;
        private Cam_DollyForward m_camDolly = null;


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

            m_camDolly = GetComponentInChildren<Cam_DollyForward>();
            m_camTrans = m_camDolly.transform;

            // Detach from parent on start!
            m_trans.SetParent(camHolder.transform, true);

            m_myStartPos = m_trans.position;
            m_myStartRot = m_trans.rotation;

            m_trans.localPosition = Vector3.zero;
        }

        void Update()
        {
#if UNITY_EDITOR
			if (Input.GetKeyDown(KeyCode.Space))
			{
                ShakeCam(1.5f, 0.7f);
			}
#endif

			m_shakeTime -= Time.deltaTime;

			// This has been updated - If cam IS following and screenshake is NOT in effect, then make the camera move to the right position.
            if (camFollowPlayer)
            {
				if (m_shakeTime < 0)
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
        public void ShakeCam(float a_shakeStrength, float a_shakeDuration)
		{
			// Recommend shakeDuration = 0.6f and shakeStrength = 1.5f;

            // Only override shake if stronger or longer
            if (a_shakeDuration >= m_shakeTime || a_shakeStrength >= m_shakeAmount)
            {
                // Set shake time
                m_shakeTime = a_shakeDuration;
                m_shakeAmount = a_shakeStrength;
            }
			
			// This changes the values in the update function.
		}

        public void FollowCam()
        {
            if (camPosTarget != null)
            {
                // TODO Fix lerping
                m_trans.position = camPosTarget.position;

                // Move camera to take into account world obstacles
                Vector3 lookOffset = m_camDolly.GetOriginalPosition() - camLookTarget.position;
                RaycastHit[] rayHits = Physics.RaycastAll(camLookTarget.position, lookOffset, lookOffset.magnitude * 1.1f, Physics.DefaultRaycastLayers);
                string myTag = gameObject.tag;
                float nearDist = 999999.0f;
                Vector3 nearPos = Vector3.zero;
                for (int i = 0; i < rayHits.Length; ++i)
                {
                    // Find obstacles that aren't owned by this player
                    if (rayHits[i].distance <= nearDist && !rayHits[i].collider.isTrigger && 
                        !rayHits[i].collider.gameObject.tag.Contains("Player") && rayHits[i].collider.gameObject.tag.CompareTo("Passengers") != 0)
                    {
                        //Debug.Log("Hit " + rayHits[i].collider.gameObject.name + ", tag: " + rayHits[i].collider.gameObject.tag + ", dist: " + rayHits[i].distance);
                        nearDist = rayHits[i].distance;
                        nearPos = rayHits[i].point;
                    }
                }
                //Debug.DrawRay(camLookTarget.position, lookOffset, Color.yellow);

                // Found nearest camera object, snap to
                m_camDolly.SetCollisionFollowDist(nearPos);
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
				m_trans.position = tempPos + Random.insideUnitSphere * m_shakeAmount;

				// Look at target
				m_trans.rotation = camRotator.rotation;
			}
		}
	} 
}
