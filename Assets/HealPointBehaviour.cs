/**
 * File: HealPointBehaviour.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 28/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Heals the player when they enter the healing trigger zone.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// Heals the player when they enter the healing trigger zone.
    /// </summary>
    public class HealPointBehaviour : MonoBehaviour
    {
        void Start()
        {

        }

        void Update()
        {

        }

        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player1_") || other.CompareTag("Player2_") || other.CompareTag("Player3_") || other.CompareTag("Player4_"))
            {
                ShipPartDestroy partScript = other.GetComponentInParent<ShipPartDestroy>();
                if (partScript != null)
                {
                    // Found in parent, repair
                    partScript.RepairAllParts();
                }
                else
                {
                    // Look to children for the part object
                    partScript = other.GetComponentInChildren<ShipPartDestroy>();
                    if (partScript != null)
                    {
                        partScript.RepairAllParts();
                    }
                }
            }
        }
    } 
}
