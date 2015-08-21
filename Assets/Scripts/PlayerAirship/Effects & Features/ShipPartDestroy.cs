/**
 * File: ShipPartDestroy.cs
 * Author: Patrick Ferguson
 * Created: 20/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the destruction of player ship parts, and parent mass change.
 **/

using UnityEngine;
using System.Collections;

/// <summary>
/// Manages the destruction of player ship parts, and parent mass change.
/// </summary>
public class ShipPartDestroy : MonoBehaviour
{
    /// <summary>
    /// What the part is, affects how the ship's handling will be hampered.
    /// </summary>
    public enum EShipPartType
    {
        INVALID,
        LEFT_BALLOON,
        RIGHT_BALLOON,
        MAST,
        RUDDER, 
    }

    /// <summary>
    /// A collection object to contain part information.
    /// </summary>
    [System.Serializable]
    public class ShipPart
    {
        /// <summary>
        /// Handle to the parent of all of the part's colliders, this part will be disabled.
        /// </summary>
        public GameObject partObject = null;
        /// <summary>
        /// What the part is, affects how the ship's handling will be hampered.
        /// </summary>
        public EShipPartType partType = EShipPartType.INVALID;
        /// <summary>
        /// If a collision relative velocity is larger than this value, the part will break.
        /// </summary>
        public float breakVelocity = 100;
        /// <summary>
        /// Amount to subtract from the ship's mass when this part gets destroyed.
        /// </summary>
        public float partMass = 5;

        // Cached variables, these don't have to be updated by hand
        public float cached_breakVelocitySqr = 0;
    }
    /// <summary>
    /// The parts of this ship that have children colliders and can be destroyed.
    /// </summary>
    public ShipPart[] destructableParts;

    // Cached variables
    private Rigidbody m_shipRB = null;
    private float m_breakVelSqr = 0;

    void Awake()
    {
        m_shipRB = GetComponent<Rigidbody>();
    }

	/// <summary>
    /// Use this for initialisation.
	/// </summary>
	void Start()
    {
	
	}
	
	/// <summary>
    /// Update is called once per frame.
	/// </summary>
	void Update()
    {
	
	}

    /// <summary>
    /// Called when a collision begins to objects.
    /// </summary>
    /// <param name="a_colInfo">Collision information.</param>
    void OnCollisionEnter(Collision a_colInfo)
    {
        EvaluatePartCollision(a_colInfo);
    }

    private void EvaluatePartCollision(Collision a_colInfo)
    {
        Collider[] childColliders;
        foreach (ContactPoint contact in a_colInfo.contacts)
        {
            Debug.Log("Colliding with: " + contact.thisCollider.transform.parent.name + ", other: " + contact.otherCollider.transform.parent.name);
            foreach (ShipPart part in destructableParts)
            {
                childColliders = part.partObject.GetComponentsInChildren<Collider>();
                foreach (Collider col in childColliders)
                {
                    // Check if the collision actually involves the part
                    if (contact.thisCollider == col)
                    {
                        Debug.Log("Colliding with: " + col + ", part : " + part.partObject.name);
                        // Cache part squared velocity
                        if (Mathf.Approximately(part.cached_breakVelocitySqr, 0))
                        {
                            part.cached_breakVelocitySqr = part.breakVelocity * part.breakVelocity;
                        }

                        Debug.Log(a_colInfo.relativeVelocity.magnitude);

                        // Break the part if the collision is fast enough
                        if (!IsPartDestroyed(part) && a_colInfo.relativeVelocity.sqrMagnitude >= part.cached_breakVelocitySqr)
                        {
                            BreakPart(part);
                        }

                        // Collision has evaluated, return back to OnCollisionEnter
                        return;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Returns whether the part has been destroyed.
    /// </summary>
    /// <param name="a_part">The part to test</param>
    /// <returns>True if destroyed, false if not.</returns>
    public bool IsPartDestroyed(ShipPart a_part)
    {
        return !a_part.partObject.activeSelf;
    }

    /// <summary>
    /// Re-enables the part, returns mass and thus restores control to the ship.
    /// </summary>
    /// <param name="a_part">The part to test</param>
    public void RepairPart(ShipPart a_part)
    {
        // TODO Build part repair system

        if (!IsPartDestroyed(a_part))
        {
            Debug.Log("Repair! " + name);

            // Make the mass change to the parent
            m_shipRB.mass += a_part.partMass;

            // Re-enable the part
            a_part.partObject.SetActive(true);
        }
    }

    /// <summary>
    /// Breaks the part, takes mass and hampers control to the ship.
    /// </summary>
    /// <param name="a_part">The part to test</param>
    private void BreakPart(ShipPart a_part)
    {
        Debug.Log("Break! " + name);

        // Make the mass change to the parent
        m_shipRB.mass -= a_part.partMass;

        // Disable the part
        a_part.partObject.SetActive(false);
    }
}
