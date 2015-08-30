/**
 * File: CinematicScript.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 28/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script manages a basic camera orbiting of the scene to show off level and gameplay features.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    /// <summary>
    /// This script manages a basic camera orbiting of the scene to show off level and gameplay features.
    /// </summary>
    public class CinematicScript : MonoBehaviour
    {
        public float twirlSpeed = 20;

        public GameObject upDownTarget;
        private float upDown;

        // Need to make it work in local space

        void Start()
        {

        }

        void Update()
        {
            gameObject.transform.Rotate(Vector3.up * twirlSpeed * Time.deltaTime);

            upDown = Mathf.PingPong(Time.time, -3);

            upDownTarget.transform.position = new Vector3(upDownTarget.transform.position.x, upDown, upDownTarget.transform.position.z);
        }
    } 
}
