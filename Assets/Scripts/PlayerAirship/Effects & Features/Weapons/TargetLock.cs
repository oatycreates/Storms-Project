/**
 * File: TargetLock.cs
 * Author: 
 * Maintainers: 
 * Created: 
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: 
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class TargetLock : MonoBehaviour
    {
        /// <summary>
        /// Dot product lock range, this is equal to the sine of the angle you want. E.g. 0.707 for 45*.
        /// </summary>
        public float frontCannonLockAngle = 0.707f;

        /// <summary>
        /// How long to wait before re-evaluating the target locking.
        /// </summary>
        public float lockRepeatCooldown = 0.25f;

        /// <summary>
        /// Maximum distance to lock targets in.
        /// </summary>
        public float maximumTarDist = 1000.0f;

        /// <summary>
        /// Where the player is looking at.
        /// </summary>
        public Transform lookTarget = null;

        /// <summary>
        /// Points the front cannon towards the target.
        /// </summary>
        public Transform frontCannon = null;

        /// <summary>
        /// Handle to all of the player objects in the scene.
        /// </summary>
        public GameObject[] playerObjs = null;

        /// <summary>
        /// Current front target for the ship.
        /// </summary>
        private Transform m_currFrontTarget = null;

        // Cached variables
        private Transform m_trans = null;
        private Transform m_tarTrans = null;
        private CannonFire m_cannon = null;
        private Quaternion m_startCannonRot = Quaternion.identity;

        void Awake()
        {
            m_trans = transform;
            m_tarTrans = lookTarget;
            if (frontCannon != null)
            {
                m_startCannonRot = frontCannon.localRotation;
            }
            m_cannon = frontCannon.GetComponent<CannonFire>();

            // Re-target the front cannon every few seconds
            InvokeRepeating("FindLockTarget", lockRepeatCooldown, lockRepeatCooldown);
        }

		void Start() 
		{
		
		}
		
		void Update() 
		{
            // Render over the target
            if (frontCannon != null)
            {
                if (m_currFrontTarget != null)
                {
                    if (m_cannon != null)
                    {
                        m_cannon.lookAtTarget = m_currFrontTarget;
                    }
                    Debug.DrawLine(frontCannon.position, m_currFrontTarget.position, Color.red);
                }
                else
                {
                    // Reset to look where the ship is
                    frontCannon.localRotation = m_startCannonRot;
                    if (m_cannon != null)
                    {
                        m_cannon.lookAtTarget = null;
                    }
                }
            }
		}

        public Transform GetTarget()
        {
            return m_currFrontTarget;
        }

        private void FindLockTarget()
        {
            if (isActiveAndEnabled)
            {
                // Find all players in the scene
                string playerName = gameObject.name;
                float closestToLook = -1.0f, closestTarDist = 99999.0f;
                Transform closestTar = null, tempTrans = null;
                Vector3 myPos = m_trans.position;
                Vector3 lookDir = (m_tarTrans != null) ? (m_tarTrans.position - myPos).normalized : m_trans.forward;
                float currLookDist = 0, currDist = 0;
                Vector3 tempPos = Vector3.zero, offsetVec = Vector3.zero, offsetVecNorm = Vector3.zero;
                foreach (GameObject obj in playerObjs)
                {
                    if (obj != null)
                    {
                        tempTrans = obj.transform;
                        tempPos = tempTrans.position;
                        offsetVec = tempPos - myPos;
                        currDist = offsetVec.magnitude;
                        // If target is in range
                        if (currDist <= maximumTarDist)
                        {
                            offsetVecNorm = offsetVec.normalized;

                            // Check if closer to camera first and in view cone
                            currLookDist = Vector3.Dot(offsetVecNorm, lookDir);
                            if (Vector3.Dot(offsetVecNorm, m_trans.forward) >= frontCannonLockAngle && currLookDist >= closestToLook)
                            {
                                // Check for targets closer to the player ship
                                if (currDist <= closestTarDist)
                                {
                                    // Potential target found
                                    closestToLook = currLookDist;
                                    closestTarDist = currDist;
                                    closestTar = tempTrans;
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.Log("Null player object found on the TargetLock script of " + playerName + ", make sure all are set as needed!");
                    }
                }

                // Assign the target
                if (closestTar != null)
                {
                    //Debug.Log("Targeted! " + playerTag + ", tar: " + closestTar.tag);
                    m_currFrontTarget = closestTar;
                }
                else
                {
                    //Debug.Log("Cleared target! " + playerTag);
                    m_currFrontTarget = null;
                }
            }
        }
	}
}
