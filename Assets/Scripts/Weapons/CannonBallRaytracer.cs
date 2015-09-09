/**
 * File: CannonBallRaytracer.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 9/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Controls the behaviour of the cannon balls,
 * and raytraces onto any ships which enter the trigger volume
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
	public class CannonBallRaytracer : MonoBehaviour 
	{
        // Cached variables
        GameObject thisGameObject;      // This game object
        CapsuleCollider m_collider;     // Capsule trigger

        [Tooltip("Maximum size the trigger volume will expand to")]
        public float maxSize        = 50.0f;
        [Tooltip("Speed at which the trigger volume will expand")]
        public float expandSpeed    = 10.0f;

        public void Awake()
        {
            thisGameObject  = this.gameObject;
            m_collider      = GetComponent<CapsuleCollider>();
        }
    
        private void Start() 
		{
		    
		}
		
		private void Update() 
		{
            // Disable this game object when reached maximum size
            if (m_collider.height >= maxSize)
            {
                thisGameObject.SetActive(false);
            }

            ExpandTrigger();
		}

        private void ExpandTrigger()
        {
            // Expand height
            m_collider.height += expandSpeed * Time.deltaTime;

            // Set new collider centre
            Vector3 centre  = m_collider.center;
            centre.z = (m_collider.height / 2.0f) - m_collider.radius;

            m_collider.center = centre;
        }

        private void HitObject(GameObject a_gameObject)
        {
            // TODO: Alert other object they have been hit

            thisGameObject.SetActive(false);
        }

        public void OnEnable()
        {
            m_collider.center = Vector3.zero;
            m_collider.radius = 0.5f;
            m_collider.height = 1.0f;
        }

        public void OnTriggerEnter(Collider a_other)
        {
            string tag = a_other.tag;

            if (tag == "Player1_" ||
                tag == "Player2_" ||
                tag == "Player3_" ||
                tag == "Player4_")
            {
                // Hit player object
                HitObject(a_other.gameObject);
            }

            // Hit other object
            thisGameObject.SetActive(false);
        }
	}
}
