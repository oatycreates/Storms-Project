/**
 * File: CamLookDirection.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 20/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script makes the airship camera rotate to look at an invisible target, regardless of the camera's actual position.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This script makes the airship camera rotate to look at an invisible target, regardless of the camera's actual position.
    /// </summary>
    public class CamLookDirection : MonoBehaviour
    {
        public GameObject lookTarget;

        private float distanceToTarget;
        private Ray myRay;

        // Cached variables
        private Transform m_trans = null;
        private Transform m_tarTrans = null;

        void Awake()
        {
            m_trans = transform;
            m_tarTrans = lookTarget.transform;
        }

        void Start()
        {

        }

        void Update()
        {
            m_trans.LookAt(m_tarTrans.position);

            DebugMe();
        }


        void DebugMe()
        {
            // Raycast
            distanceToTarget = Vector3.Distance(m_tarTrans.position, m_trans.position);

            Vector3 relativeForward = m_trans.TransformDirection(Vector3.forward);

            myRay = new Ray(m_trans.position, relativeForward);

            Debug.DrawRay(myRay.origin, myRay.direction * distanceToTarget, Color.green);
        }
    } 
}
