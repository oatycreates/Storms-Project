/**
 * File: SailBehaviour.cs
 * Author: Andrew Barbour
 * Maintainer: Andrew Barbour
 * Created: 3/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the behaviour of the airship's sail
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    public class SailBehaviour : MonoBehaviour
    {
        public float speed = 10.0f;

        float m_startScaleY;                        // Store origin scale of sail

        float m_rawLerp     = 0.0f;                 // Raw lerp value
        float m_lerpTime    = 0.0f;                 // Used within lerp as the time value

        // Cached variables
        Transform m_transform;                      // This gameObject's transform
        AirshipControlBehaviour m_airpshipControl;  // Primary airship controller

        public void Awake()
        {
            m_airpshipControl   = GetComponentInParent<AirshipControlBehaviour>();
            m_transform         = transform;
        }

        void Start()
        {
            // Store starting Y scale
            m_startScaleY = m_transform.localScale.y;
        }

        void Update()
        {
            Vector3 currentScale = m_transform.localScale;
            float lerpTarget;

            // Select lerp target
            if (m_airpshipControl.isReversing)
            {
                lerpTarget = 0.0f;
            }
            else
            {
                lerpTarget = m_startScaleY;
            }

            // Update scale
            currentScale.y = Mathf.Lerp(currentScale.y, lerpTarget, m_lerpTime);
            m_transform.localScale = currentScale;

            // Update lerp values
            m_rawLerp += Time.deltaTime * speed;
            m_lerpTime = Mathf.Min(m_rawLerp, 1.0f);

            if (m_lerpTime == 1.0f)
            {
                // Reset lerp
                m_lerpTime  = 0.0f;
                m_rawLerp   = 0.0f;
            }
        }
    }
}
