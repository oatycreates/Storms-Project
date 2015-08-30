/**
 * File: CannonBallBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 14/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This has all the behaviour for the cannonballs.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This has all the behaviour for the cannonballs.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(SphereCollider))]
    public class CannonBallBehaviour : MonoBehaviour
    {
        /// <summary>
        /// Physical collider for the cannon ball.
        /// </summary>
        public Collider cannonBallCollider;

        /// <summary>
        /// For ignoring collisions with the owner's player ship & own base.
        /// </summary>
        public Collider selfCollisionTrigger;

        /// <summary>
        /// How long the cannonballs should live for.
        /// </summary>
        public float cannonBallLifetime = 5.0f;

        /// <summary>
        /// Scale of the cannonball at the end of its lifetime.
        /// </summary>
        public Vector3 endScale = new Vector3(12, 12, 12);

        /// <summary>
        /// How long the cannonball has left in seconds before it expires.
        /// </summary>
        private float m_timer = 0.0f;

        /// <summary>
        /// Whether the collider has been disabled.
        /// </summary>
        private bool m_disabledCollider = false;

        /// <summary>
        /// Time the cannon-ball last self-triggered.
        /// </summary>
        private float m_lastSelfTriggerTime = 0.0f;

        // Cached variables
        private Transform m_trans = null;
        private Vector3 m_startScale = Vector3.one;

        void Awake()
        {
            m_trans = transform;
            m_startScale = m_trans.localScale;
        }

        void OnEnable()
        {
            // Begin the cannonball's life
            m_timer = cannonBallLifetime;

            // Revert the cannonball scale
            m_trans.localScale = m_startScale;

            // Make the cannonball collider start disabled to prevent collision with the player's own ship
            SetCannBallColEnabled(false);
        }

        void Start()
        {

        }

        void Update()
        {
            m_timer -= Time.deltaTime;

            // Scale the cannonball
            float lifeProg = Mathf.Min((cannonBallLifetime - m_timer) / cannonBallLifetime, 1.0f);
            m_trans.localScale = Vector3.Lerp(m_startScale, endScale, lifeProg);

            // For the cannonball expiring
            if (m_timer <= 0)
            {
                gameObject.SetActive(false);
            }
        }

        void FixedUpdate()
        {
            m_lastSelfTriggerTime -= Time.deltaTime;
            if (m_disabledCollider && m_lastSelfTriggerTime <= 0)
            {
                // Re-enable the collider
                SetCannBallColEnabled(true);
            }
        }

        void OnTriggerEnter(Collider a_other)
        {
            // Disable collider to prevent self-collision
            if (gameObject.CompareTag(a_other.tag))
            {
                SetCannBallColEnabled(false);
            }
        }

        void OnTriggerStay(Collider a_other)
        {
            // Disable collider to prevent self-collision
            if (gameObject.CompareTag(a_other.tag))
            {
                SetCannBallColEnabled(false);
            }
        }

        /// <summary>
        /// Sets whether the cannonball collider is active.
        /// </summary>
        /// <param name="a_colEnable">True if active, false if not.</param>
        private void SetCannBallColEnabled(bool a_colEnable)
        {
            cannonBallCollider.enabled = a_colEnable;
            m_disabledCollider = !a_colEnable;

            // Make the collider take time to re-enable
            if (!a_colEnable)
            {
                m_lastSelfTriggerTime = 0.25f;
            }
        }

        /*
        private AudioSource mySource;
        public AudioClip explosionNoise;

        private float timeOut = 5.0f;
        private float rememberTimeOut;
	
        private SphereCollider myCollider;
	
        void Awake()
        {
            mySource = gameObject.GetComponent<AudioSource>();
            myCollider = gameObject.GetComponent<SphereCollider>();
		
            mySource.clip = explosionNoise;
        }
	
        void Start()
        {
            rememberTimeOut = timeOut;
		
        }
	

        void OnEnable () 
        {
            timeOut = rememberTimeOut;	
		
            myCollider.isTrigger = true;
		
            // Turn the trigger back on once the cannonball has spawned.
            Invoke("TriggerOff", 1.0f);
        }
	
	
        void Update () 
        {
            timeOut -= Time.deltaTime;
		
            if (timeOut < 0)
            {
                gameObject.SetActive(false);
            }
		
            // Or if the cannonball has somehow fallen too far
            if (gameObject.transform.position.y < -2000)
            {
                gameObject.SetActive(false);
            }
        }
	
        void TriggerOff()
        {
            myCollider.isTrigger = false;
        }
	
	
        void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.tag != gameObject.tag)
            {	
                //Sound Stuff!!!
                if (explosionNoise != null)
                {
                    mySource.Play();
                }
                else
                {
                    Debug.Log("No sound on Cannonball!");
                }
			
                //Leave an impact
                if (other.gameObject.GetComponent<Rigidbody>() != null)
                {
                    other.gameObject.GetComponent<Rigidbody>().AddExplosionForce(5, gameObject.transform.position, 5.0f);
                }
			
			
                //Wipe me out!
                myCollider.isTrigger = true;
                gameObject.SetActive(false);
			
            }
        }
        */
    } 
}
