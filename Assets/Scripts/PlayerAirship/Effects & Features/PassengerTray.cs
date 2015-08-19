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

/// <summary>
/// Attempts to keep the passengers in the ship by passing any forces applied to the ship onto the passengers.
/// </summary>
public class PassengerTray : MonoBehaviour
{
    /// <summary>
    /// List of  game object tags to try to keep in the tray.
    /// </summary>
    public string[] trayPassengerTags = {"Passengers"};

    /// <summary>
    /// Mass to add to the ship per prisoner in the body.
    /// </summary>
    public float prisonerMassAdd = 0.1f;

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
    /// Number of things in the tray that match the passenger tags.
    /// </summary>
    private int m_numInTray = 0;

    // Cached variables
    private Rigidbody m_shipRb;

	/// <summary>
    /// Use this for initialisation.
	/// </summary>
	void Start()
    {
        // Zero variables
        m_currShipAccel = Vector3.zero;
        m_lastShipVel = Vector3.zero;

        // Cache variables
        m_shipRb = gameObject.GetComponentInParent<Rigidbody>();
        m_shipStartMass = 0;
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
            m_shipRb.mass = m_shipStartMass + m_numInTray * prisonerMassAdd;
            m_numInTray = 0;
        }

        if (m_hasStarted)
        {
            // Calculate ship velocity over the past tick. a = (v - u) / t
            m_currShipAccel = (m_shipRb.velocity - m_lastShipVel) / Time.deltaTime;

            // Store ship velocity for the next tick
            m_lastShipVel = m_shipRb.velocity;
        }
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
                ++m_numInTray;
            }
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
