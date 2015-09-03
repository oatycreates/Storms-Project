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

        private float m_yPos = 0;
        private float m_xPos = 0;
        private float m_zPos = 0;

        // Link to Cannons
        public GameObject[] cannons;

        // Fix cam to position
        public float camTurnSpeed = 3.0f;

        /// <summary>
        /// Minimum time between shots.
        /// </summary>
        public float shotCooldown = 2.51f;

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

        /// <summary>
        /// Time before the cannon can fire again.
        /// </summary>
        private float m_currShotCooldown = 0.0f;

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
        private Transform m_lookTarTrans = null;
        private Transform m_camProxyTrans = null;

        void Start()
        {
            // Cache variables
            m_referenceStateManager = GetComponent<StateManager>();
            m_shipTrans = transform;
            m_camRotTrans = rotateCam.transform;
            m_lookTarTrans = lookyHereTarget.transform;
            m_camProxyTrans = camProxyTarget.transform;
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
            // Count down the shot cool-down
            m_currShotCooldown -= Time.deltaTime;
        }

        public void PlayerInputs(float a_camVertical, float a_camHorizontal, float a_triggerAxis, bool a_faceDown, bool a_leftBumper, bool a_rightBumper, bool a_leftClick, bool a_rightClick)
        {
            // Zero input if not enabled
            if (!this.isActiveAndEnabled)
            {
                ResetCamRotation(true);
            }
            else
            {
                // Reset on left stick click
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

                // Move lookTarget around
                float internalCamYRotation = m_camRotTrans.localEulerAngles.y;

                //Debug.Log("Inp: " + m_totalHoriz + " " + m_totalVert + ", deg: " + m_tiltAroundX + " " + m_tiltAroundY + ", result: " + internalCamYRotation);

                if (internalCamYRotation <= 315 && internalCamYRotation > 225)
                {
                    /*//print ("Left");
                    //Move the target
                    m_yPos = Mathf.Lerp(m_yPos, targetHeightFactor, Time.deltaTime * smooth / 2);

                    //Move the cam
                    m_xPos = Mathf.Lerp(m_xPos, camPositionFactor, Time.deltaTime * smooth / 2);
                    m_zPos = Mathf.Lerp(m_zPos, camDistanceFactor, Time.deltaTime * smooth / 2);*/

                    // Allow CannonFire
                    if (a_faceDown)
                    {
                        Cannons(ECannonPos.Port);
                    }

                }
                else if (internalCamYRotation <= 135 && internalCamYRotation > 45)
                {
                    /*//print ("Right");

                    // Move the target
                    m_yPos = Mathf.Lerp(m_yPos, targetHeightFactor, Time.deltaTime * smooth / 2);


                    // Move the cam
                    m_xPos = Mathf.Lerp(m_xPos, -camPositionFactor, Time.deltaTime * smooth / 2);
                    m_zPos = Mathf.Lerp(m_zPos, camDistanceFactor, Time.deltaTime * smooth / 2);*/

                    // Allow CannonFire
                    if (a_faceDown)
                    {
                        Cannons(ECannonPos.Starboard);
                    }

                }
                else if (internalCamYRotation <= 225 && internalCamYRotation > 135)
                {
                    /*//print ("Back");
                    // Move the target
                    m_yPos = Mathf.Lerp(m_yPos, 0, Time.deltaTime * smooth / 2);


                    // Move the cam
                    m_xPos = Mathf.Lerp(m_xPos, 0, Time.deltaTime * smooth / 2);
                    m_zPos = Mathf.Lerp(m_zPos, 20, Time.deltaTime * smooth / 2);*/
                }
                else
                {
                    /*//print ("Forward");
                    // Move the target
                    m_yPos = Mathf.Lerp(m_yPos, 0, Time.deltaTime * smooth / 2);


                    // Move the cam
                    m_xPos = Mathf.Lerp(m_xPos, 0, Time.deltaTime * smooth / 2);
                    m_zPos = Mathf.Lerp(m_zPos, 20, Time.deltaTime * smooth / 2);*/

                    // Allow CannonFire
                    if (a_faceDown)
                    {
                        Cannons(ECannonPos.Forward);
                    }

                }

                //m_lookTarTrans.localPosition = new Vector3(m_lookTarTrans.localPosition.x, m_yPos, m_lookTarTrans.localPosition.z);
                //m_camProxyTrans.localPosition = new Vector3(m_xPos, m_camProxyTrans.localPosition.y, -m_zPos);

                //m_lookTarTrans.localPosition = new Vector3(m_lookTarTrans.localPosition.x, m_yPos, m_lookTarTrans.localPosition.z);
                //m_camProxyTrans.localPosition = new Vector3(m_xPos, m_camProxyTrans.localPosition.y, -m_zPos);
            }
        }

        void Cannons(ECannonPos a_angle)
        {
            if (m_currShotCooldown <= 0)
            {
                CannonFire script;

                // Put the cannon on cool-down
                m_currShotCooldown = shotCooldown;

                for (int i = 0; i < cannons.Length; i++)
                {
                    script = cannons[i].GetComponent<CannonFire>();

                    // Fire the cannons situated in the requested direction
                    if (a_angle == script.cannon)
                    {
                        script.Fire();
                    }
                }
            }
        }
    } 
}
