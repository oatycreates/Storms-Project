/**
 * File: TagChildren.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 6/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Simple script to pass GameObject tags onto the children of this object.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This script will pass itself through EVERY child object of the Airship. This can be useful for collisions, inputs and cam references. 
    /// I.e. The Cannonball collisions will check for the object's tag. If all objects, have the same tag as parent, then there is no confusion.
    /// </summary>
    public class TagChildren : MonoBehaviour
    {
        void Start()
        {
            string myTag = gameObject.tag;
            ApplyTags_R(transform, myTag);
        }

        private void ApplyTags_R(Transform a_trans, string a_tag)
        {
            // Make the child tag == my tag
            a_trans.gameObject.tag = a_tag;
            foreach (Transform child in a_trans)
            {
                // Recurse
                ApplyTags_R(child, a_tag);
            }
        }
    } 
}
