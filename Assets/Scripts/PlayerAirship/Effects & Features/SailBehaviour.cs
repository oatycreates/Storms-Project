using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    public class SailBehaviour : MonoBehaviour
    {
        public float speed = 10.0f;

        float m_startScaleY;

        float m_rawLerp     = 0.0f;
        float m_lerpTime    = 0.0f;

        Transform m_transform;
        AirshipControlBehaviour m_airpshipControl;

        public void Awake()
        {
            m_airpshipControl = GetComponentInParent<AirshipControlBehaviour>();
            m_transform = transform;
        }

        void Start()
        {
            m_startScaleY = m_transform.localScale.y;
        }

        void Update()
        {
            Vector3 currentScale = m_transform.localScale;
            float lerpTarget;

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
                m_lerpTime  = 0.0f;
                m_rawLerp   = 0.0f;
            }
        }
    }
}
