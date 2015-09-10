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
using System.Collections.Generic;

namespace ProjectStorms
{
	public class CannonBallRaytracer : MonoBehaviour 
	{
        [Tooltip("Maximum size the trigger volume will expand to")]
        public float maxSize = 50.0f;
        [Tooltip("Speed at which the trigger volume will expand")]
        public float expandSpeed = 10.0f;
        [Tooltip("How much force will be applied on colliding objects")]
        public float collisionForce = 200.0f;
        
        [Tooltip("Layers which cannon balls will collide with")]
        public LayerMask collisionLayerMask;

        [HideInInspector]
        float m_oldCollider_height;
        [HideInInspector]
        float m_oldCollider_radius;

        // Cached variables
        [HideInInspector]
        GameObject m_thisGameObject;    // This game object
        [HideInInspector]
        CapsuleCollider m_collider;     // Capsule trigger
        [HideInInspector]
        Transform m_transform;          // This game object's transform

        public float totalLifeTime
        {
            get
            {
                return maxSize / expandSpeed;
            }
        }

        public void Awake()
        {
            // Cache variables
            m_thisGameObject    = this.gameObject;
            m_collider          = GetComponent<CapsuleCollider>();
            m_transform         = transform;

            // Save old settings of collider
            m_oldCollider_radius = m_collider.radius;
            m_oldCollider_height = m_collider.height;

            // Ensure no divisions by zero occur
            if (m_oldCollider_height == 0.0f)
            {
                Debug.LogError("Collider height cannot be 0!");
                Debug.Break();
            }
        }
		
		private void Update() 
		{
            // Disable this game object when reached maximum size
            if (m_collider.height >= maxSize)
            {
                m_thisGameObject.SetActive(false);
            }

            ExpandTrigger();
		}

        /// <summary>
        /// Expands the trigger over a period of time
        /// </summary>
        private void ExpandTrigger()
        {
            // Expand height
            m_collider.height += expandSpeed * Time.deltaTime;

            // Set new collider centre
            Vector3 centre  = m_collider.center;
            centre.z = (m_collider.height / 2.0f) - m_collider.radius;

            m_collider.center = centre;
        }

        /// <summary>
        /// Attempts to hit an object by raycast onto it first, then
        /// destroying that ship part (if ship part) and applies a force
        /// in the direction of this game object's forward vector
        /// </summary>
        /// <param name="a_gameObject">Which game object to alert of collisions</param>
        private void HitObject(GameObject a_gameObject)
        {
            RaycastHit raycastHit;
            if (!Physics.Raycast(m_transform.position, m_transform.forward, 
                    out raycastHit, m_collider.height, collisionLayerMask))
            {
                // Didn't collide with object
                return;
            }

            // Get player rigid body
            Rigidbody objectRigidBody = a_gameObject.GetComponentInParent<Rigidbody>();
            if (objectRigidBody == null)
            {
                Debug.Log("Can't find rigid body on collided object");
            }

            // Apply colliding force to player
            objectRigidBody.AddForce(m_transform.forward * collisionForce, ForceMode.Impulse);
            Debug.Log("Cannon ball collided with: " + objectRigidBody.name + " (tag: " + objectRigidBody.tag + ")");

            // Attempt to destroy player part
            ShipPartDestroy partDestroyer = a_gameObject.GetComponentInParent<ShipPartDestroy>();
            if (partDestroyer == null)
            {
                // Not destructable player part
                return;
            }

            // Destroy player part
            partDestroyer.EvaluatePartCollision(a_gameObject.GetComponent<Collider>(), collisionForce * 2.0f);
        }

        public void OnEnable()
        {
            // Reset collider
            m_collider.center = Vector3.zero;
            m_collider.radius = m_oldCollider_radius;
            m_collider.height = m_oldCollider_height;
        }

        public void OnTriggerEnter(Collider a_other)
        {
            string tag = a_other.tag;

            if (tag == this.tag)
            {
                // Don't hit original shooter
                return;
            }

            if (tag == "Player1_"   ||
                tag == "Player2_"   ||
                tag == "Player3_"   ||
                tag == "Player4_")
            {
                // Hit player object
                HitObject(a_other.gameObject);
            }

            // Hit other object
            m_thisGameObject.SetActive(false);
        }
	}
}
