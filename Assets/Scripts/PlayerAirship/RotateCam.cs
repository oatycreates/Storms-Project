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

        public float horizontalTiltAngle = 360.0f;
        public float verticalTiltAngle = 90.0f;
        public float smooth = 2.0f;
        public float deadZoneFactor = 0.25f;

        private float m_tiltAroundY;
        private float m_tiltAroundX;

        // Move the target object

        public GameObject lookyHereTarget;
        public float targetHeightFactor = 5.0f;
        private float yPos = 0;

        // Move the camera directly
        public GameObject camProxyTarget;
        private float m_xPos;
        public float camPositionFactor = 2.0f;
        private float m_zPos;
        public float camDistanceFactor = 15.0f;

        // Link to Cannons
        public GameObject[] cannons;

        // Fix cam to position
        public float camTurnMultiplier = 1.0f;
        public float totalVert = 0;
        public float totalHori = 0;

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
                totalHori = 0;
                totalVert = 0;
            }
            else
            {
                // Interp the look back to neutral
                totalHori = Mathf.Lerp(totalHori, 0, camResetMoveSpeed * Time.deltaTime);
                totalVert = Mathf.Lerp(totalVert, 0, camResetMoveSpeed * Time.deltaTime);

                // Snap the last leg
                if (Mathf.Abs(totalHori) < 0.01f)
                {
                    totalHori = 0;
                }
                if (Mathf.Abs(totalVert) < 0.01f)
                {
                    totalVert = 0;
                }
            }
        }

        void Update()
        {
            // Clamp the totalVert values
            totalVert = Mathf.Clamp(totalVert, -1.25f, 1.25f);
            totalHori = Mathf.Clamp(totalHori, -1.85f, 1.85f);

            // Count down the shot cool-down
            m_currShotCooldown -= Time.deltaTime;
        }

        public void PlayerInputs(float a_camVertical, float a_camHorizontal, float a_triggerAxis, bool a_faceDown, bool a_leftBumper, bool a_rightBumper, bool a_leftClick, bool a_rightClick)
        {
            if (this.isActiveAndEnabled)
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

                // Lock up/down
                if (a_camVertical > 0)
                {
                    totalVert -= 0.01f * camTurnMultiplier;
                }

                if (a_camVertical < 0)
                {
                    totalVert += 0.01f * camTurnMultiplier;
                }

                // Lock left/right
                if (a_camHorizontal > 0)
                {
                    totalHori += 0.01f * camTurnMultiplier;
                }

                if (a_camHorizontal < 0)
                {
                    totalHori -= 0.01f * camTurnMultiplier;
                }

                // Record last camera movement time for when to reset the looking
                if (!Mathf.Approximately(a_camHorizontal, 0) || !Mathf.Approximately(a_camVertical, 0))
                {
                    m_lastCamLookTime = movingCamResetTime;
                }

                m_tiltAroundX = totalVert * verticalTiltAngle * deadZoneFactor;
                m_tiltAroundY = totalHori * verticalTiltAngle * deadZoneFactor;

                //tiltAroundY = camHorizontal * horizontalTiltAngle * deadZoneFactor;
                //tiltAroundX = camVertical * verticalTiltAngle * deadZoneFactor;

                if (invertUpDown)
                {
                    m_tiltAroundX *= -1;
                }

                if (invertLeftRight)
                {

                    m_tiltAroundY *= -1;
                }

                Quaternion target = Quaternion.Euler(m_tiltAroundX, m_tiltAroundY, 0);

                EPlayerState currState = m_referenceStateManager.GetPlayerState();
                if (currState == EPlayerState.Control || currState == EPlayerState.Suicide)
                {
                    // Smooth the camera's rotation out on control and suicide
                    m_camRotTrans.localRotation = Quaternion.Slerp(m_camRotTrans.localRotation, target, Time.deltaTime * smooth);
                }

                // Look behind self on right stick click
                if (a_rightClick)
                {
                    m_camRotTrans.localRotation = Quaternion.Euler(0, -180, 0);//Quaternion.LookRotation(-m_shipTrans.forward);
                }

                // Move lookTarget around
                float internalCamYRotation = m_camRotTrans.localEulerAngles.y;
                //Debug.Log(internalCamYRotation);


                if (internalCamYRotation <= 315 && internalCamYRotation > 225)
                {
                    //print ("Left");
                    //Move the target
                    yPos = Mathf.Lerp(yPos, targetHeightFactor, Time.deltaTime * smooth / 2);

                    //Move the cam
                    m_xPos = Mathf.Lerp(m_xPos, camPositionFactor, Time.deltaTime * smooth / 2);
                    m_zPos = Mathf.Lerp(m_zPos, camDistanceFactor, Time.deltaTime * smooth / 2);

                    // Allow CannonFire
                    if (a_faceDown)
                    {
                        Cannons(ECannonPos.Port);
                    }

                }
                else if (internalCamYRotation <= 135 && internalCamYRotation > 45)
                {
                    //print ("Right");

                    // Move the target
                    yPos = Mathf.Lerp(yPos, targetHeightFactor, Time.deltaTime * smooth / 2);


                    // Move the cam
                    m_xPos = Mathf.Lerp(m_xPos, -camPositionFactor, Time.deltaTime * smooth / 2);
                    m_zPos = Mathf.Lerp(m_zPos, camDistanceFactor, Time.deltaTime * smooth / 2);

                    // Allow CannonFire
                    if (a_faceDown)
                    {
                        Cannons(ECannonPos.Starboard);
                    }

                }
                else if (internalCamYRotation <= 225 && internalCamYRotation > 135)
                {
                    //print ("Back");
                    // Move the target
                    yPos = Mathf.Lerp(yPos, 0, Time.deltaTime * smooth / 2);


                    // Move the cam
                    m_xPos = Mathf.Lerp(m_xPos, 0, Time.deltaTime * smooth / 2);
                    m_zPos = Mathf.Lerp(m_zPos, 20, Time.deltaTime * smooth / 2);
                }
                else
                {
                    //print ("Forward");
                    // Move the target
                    yPos = Mathf.Lerp(yPos, 0, Time.deltaTime * smooth / 2);


                    // Move the cam
                    m_xPos = Mathf.Lerp(m_xPos, 0, Time.deltaTime * smooth / 2);
                    m_zPos = Mathf.Lerp(m_zPos, 20, Time.deltaTime * smooth / 2);

                    // Allow CannonFire
                    if (a_faceDown)
                    {
                        Cannons(ECannonPos.Forward);
                    }

                }

                m_lookTarTrans.localPosition = new Vector3(m_lookTarTrans.localPosition.x, yPos, m_lookTarTrans.localPosition.z);
                m_camProxyTrans.localPosition = new Vector3(m_xPos, m_camProxyTrans.localPosition.y, -m_zPos);
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
