/**
 * File: RotateCam.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 20/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script takes input from the input manager, and passes the movement into an empty game object with an attached camera.
 *              Most of this script was derived from the Unity Example for transform.rotate.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This script takes input from the input manager, and passes the movement into an empty game object with an attached camera.
    /// Most of this script was derived from the Unity Example for transform.rotate.
    /// </summary>
    public class RotateCam : MonoBehaviour
    {
        private StateManager m_referenceStateManager;

        // The rotate cam is the centre GameObject - not the Camera itself.
        public GameObject rotateCam;

        public bool invertUpDown = false;
        public bool invertLeftRight = false;

        public float horizontalTiltAnglePerc = 0.95f;
        public float verticalTiltAnglePerc = 0.95f;
        public float smooth = 2.0f;

        private float m_tiltAroundY;
        private float m_tiltAroundX;

        // Move the target object
        public GameObject lookyHereTarget;
        public float targetHeightFactor = 5.0f;

        // Move the camera directly
        public GameObject camProxyTarget;
        public float camPositionFactor = 2.0f;
        public float camDistanceFactor = 15.0f;

        public CannonFire frontCannon = null;

        // Link to Cannons
        public GameObject[] cannons;

        // Fix cam to position
        public float camTurnSpeed = 3.0f;

        /// <summary>
        /// Time to wait before beginning to interp the camera back when moving,
        /// </summary>
        public float movingCamResetTime = 1.5f;
        /// <summary>
        /// Lerp speed for resetting the camera
        /// </summary>
        public float camResetMoveSpeed = 3.0f;
        /// <summary>
        /// For resetting the camera when moving a few seconds after the last look.
        /// </summary>
        private float m_lastCamLookTime = 0;

        // Total camera rotation values
        private float m_totalVert = 0;
        private float m_totalHoriz = 0;

        /// <summary>
        /// Whether the view was flipped last tick.
        /// </summary>
        private bool m_flippedViewLast = false;

        // Cached variables
        private Transform m_shipTrans = null;
        private Transform m_camRotTrans = null;

        //Connect to announcer text
        public UI_Controller announcerController;

        //Bool a_lastSelect
        private bool a_lastSelect = false;

        void Start()
        {
            // Cache variables
            m_referenceStateManager = GetComponent<StateManager>();
            m_shipTrans = transform;
            m_camRotTrans = rotateCam.transform;

            announcerController = GameObject.FindObjectOfType<UI_Controller>();
        }

        public void ResetCamRotation(bool a_snap)
        {
            if (a_snap)
            {
                m_totalHoriz = 0;
                m_totalVert = 0;
            }
            else
            {
                // Interp the look back to neutral
                m_totalHoriz = Mathf.Lerp(m_totalHoriz, 0, camResetMoveSpeed * Time.deltaTime);
                m_totalVert = Mathf.Lerp(m_totalVert, 0, camResetMoveSpeed * Time.deltaTime);

                // Snap the last leg
                if (Mathf.Abs(m_totalHoriz) < 0.01f)
                {
                    m_totalHoriz = 0;
                }
                if (Mathf.Abs(m_totalVert) < 0.01f)
                {
                    m_totalVert = 0;
                }
            }
        }

        void Update()
        {

        }

        public void PlayerInputs(float a_camVertical, float a_camHorizontal, float a_triggerAxis, bool a_faceDown, bool a_leftBumper, bool a_rightBumper, bool a_leftClick, bool a_rightClick, bool a_select)
        {
            // Zero input if not enabled
            if (!this.isActiveAndEnabled)
            {
                ResetCamRotation(true);
            }
            else
            {
                // Reset camera facing
                if (a_leftClick)
                {
                    ResetCamRotation(true);
                }

                // Reset on accelerate only a few seconds after the last input
                m_lastCamLookTime -= Time.deltaTime;
                if (a_triggerAxis > 0 && m_lastCamLookTime < 0)
                {
                    // Check for direct cam input first
                    ResetCamRotation(false);
                }

                // Record last camera movement time for when to reset the looking
                if (!Mathf.Approximately(a_camHorizontal, 0) || !Mathf.Approximately(a_camVertical, 0))
                {
                    m_lastCamLookTime = movingCamResetTime;
                }

                // Sample look input
                m_totalVert += a_camVertical * camTurnSpeed * Time.deltaTime;
                m_totalHoriz += a_camHorizontal * camTurnSpeed * Time.deltaTime;

                // Clamp the total rotation values
                m_totalHoriz = Mathf.Clamp(m_totalHoriz, -1.0f, 1.0f);
                m_totalVert = Mathf.Clamp(m_totalVert, -1.0f, 1.0f);

                // Map input to desired tilt angle
                m_tiltAroundX = m_totalVert * 90.0f * horizontalTiltAnglePerc;
                m_tiltAroundY = m_totalHoriz * 90.0f * verticalTiltAnglePerc;

                if (invertUpDown)
                {
                    m_tiltAroundX = -m_tiltAroundX;
                }

                if (invertLeftRight)
                {
                    m_tiltAroundY = -m_tiltAroundY;
                }

                bool shouldFlip = a_rightClick;

                // Construct the local target rotation
                Vector3 playerRot = m_shipTrans.rotation.eulerAngles;
                Quaternion target = Quaternion.Euler(playerRot.x + m_tiltAroundX, playerRot.y +  m_tiltAroundY, 0);

                EPlayerState currState = m_referenceStateManager.GetPlayerState();
                if (currState == EPlayerState.Control || currState == EPlayerState.Suicide)
                {
                    // If the view was flipped last tick, just snap
                    if (m_flippedViewLast && !shouldFlip)
                    {
                        m_camRotTrans.rotation = target;
                    }

                    // Smooth the camera's rotation out on control and suicide
                    m_camRotTrans.rotation = Quaternion.Slerp(m_camRotTrans.rotation, target, Time.deltaTime * smooth);
                }

                // Look behind self on right stick click
                m_flippedViewLast = false;
                if (shouldFlip)
                {
                    m_camRotTrans.localRotation = Quaternion.Euler(0, -180, 0);
                    m_flippedViewLast = true;
                }           



                //Invert cam on select button press
                if (!a_select && a_lastSelect) 
                {
                    

                    invertUpDown = !invertUpDown;
                    //a_select = !a_select;

                    if (announcerController != null)
                    {
                        string me = gameObject.GetComponent<FactionIndentifier>().factionName;

                        if (invertUpDown)
                        {
                            announcerController.InvertYCam(me);
                        }
                        else
                        if (!invertUpDown)
                        {
                            announcerController.NormalYCam(me);
                        }
                    }
                   
                }

                a_lastSelect = a_select; 
            }
        }

    } 
}