/**
 * File: PassengerTray.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Attempts to keep the passengers in the ship by passing any forces applied to the ship onto the passengers.
 **/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace ProjectStorms
{
    /// <summary>
    /// Attempts to keep the passengers in the ship by passing any forces applied to the ship onto the passengers.
    /// </summary>
    public class PassengerTray : MonoBehaviour
    {
        /// <summary>
        /// List of  game object tags to try to keep in the tray.
        /// </summary>
        public string[] trayPassengerTags = { "Passengers" };

        /// <summary>
        /// Mass to add to the ship per prisoner in the body.
        /// </summary>
        public float prisonerMassAdd = 0.1f;

        /// <summary>
        /// Mass to add to the ship as a result of part mass changes.
        /// </summary>
        public float shipPartMassAdd = 0.0f;

        /// <summary>
        /// How hard to explode the ship's contents away.
        /// </summary>
        public float explosionForce = 10.0f;

        /// <summary>
        /// How big to make the ship's explosion.
        /// </summary>
        public float explosionRadius = 3.0f;

        /// <summary>
        /// Where to centre the explosion from, this should be a transform relative to the ship prefab.
        /// </summary>
        public Transform explosionCentreTrans;

        /// <summary>
        /// Cumulative ship acceleration for the tick.
        /// </summary>
        private Vector3 m_currShipAccel = Vector3.zero;

        /// <summary>
        /// Velocity of the ship last tick.
        /// </summary>
        private Vector3 m_lastShipVel = Vector3.zero;

        /// <summary>
        /// Set to true when the players ship actually starts moving.
        /// </summary>
        private bool m_hasStarted = false;

        /// <summary>
        /// Start mass of the ship. 
        /// </summary>
        private float m_shipStartMass = 0;

        /// <summary>
        /// List of objects in the tray that match the passenger tag type.
        /// </summary>
        private List<GameObject> m_trayContents = new List<GameObject>();

        // Cached variables
        private Rigidbody m_shipRb;

        void Awake()
        {
            // Zero variables
            m_currShipAccel = Vector3.zero;
            m_lastShipVel = Vector3.zero;
            shipPartMassAdd = 0.0f;

            // Cache variables
            m_shipRb = gameObject.GetComponentInParent<Rigidbody>();
            m_shipStartMass = 0.0f;
        }

        /// <summary>
        /// Use this for initialisation.
        /// </summary>
        void Start()
        {
            if (explosionCentreTrans == null)
            {
                Debug.LogError("The explosion centre transform is not set!");
            }
        }

        /// <summary>
        /// Update is called once per frame.
        /// </summary>
        void Update()
        {

        }

        /// <summary>
        /// Called once per physics tick.
        /// </summary>
        void FixedUpdate()
        {
            Vector3 currShipVel = m_shipRb.velocity;

            // Only start applying velocity forces when the player starts moving, this avoids passengers in the first tick being launched
            if (!m_hasStarted && currShipVel.magnitude > 0)
            {
                m_hasStarted = true;
            }

            if (m_shipStartMass == 0)
            {
                m_shipStartMass = m_shipRb.mass;
            }
            else
            {
                //Debug.Log(m_shipRb.mass + " " + m_shipStartMass);

                // Set the mass
                m_shipRb.mass = m_shipStartMass + shipPartMassAdd + m_trayContents.Count * prisonerMassAdd;
            }

            if (m_hasStarted)
            {
                // Calculate ship velocity over the past tick. a = (v - u) / t
                m_currShipAccel = (m_shipRb.velocity - m_lastShipVel) / Time.deltaTime;

                // Store ship velocity for the next tick
                m_lastShipVel = m_shipRb.velocity;
            }

            m_trayContents.Clear();
        }

        /// <summary>
        /// Called each physics tick that other objects are colliding with this trigger.
        /// </summary>
        /// <param name="a_other"></param>
        void OnTriggerStay(Collider a_other)
        {
            if (IsTrayObject(a_other.tag))
            {
                // Apply the cumulative ship force for the tick to this object
                Rigidbody rb = a_other.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    // Add force
                    rb.AddForce(m_currShipAccel, ForceMode.Acceleration);

                    // Cumulate mass
                    m_trayContents.Add(rb.gameObject);
                }
            }
        }

        /// <summary>
        /// Causes the tray to explode its contents outwards.
        /// </summary>
        public void ExplodeTray()
        {
            // Explode the ship tray
            foreach (GameObject go in m_trayContents)
            {
                go.GetComponent<Rigidbody>().AddExplosionForce(explosionForce, explosionCentreTrans.position, explosionRadius);
            }
        }

        /// <summary>
        /// Returns whether the input tag is for an object that should be kept in the tray.
        /// </summary>
        /// <param name="a_otherTag">Tag of the other game object.</param>
        /// <returns>True if it is a tray object, false if not.</returns>
        private bool IsTrayObject(string a_otherTag)
        {
            bool outIsTrayObj = false;

            for (uint i = 0; i < trayPassengerTags.Length; ++i)
            {
                if (trayPassengerTags[i].CompareTo(a_otherTag) == 0)
                {
                    outIsTrayObj = true;
                    break;
                }
            }

            return outIsTrayObj;
        }
    } 
}
