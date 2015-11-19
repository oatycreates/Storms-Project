/**
 * File: Rotator.cs
 * Author: Andrew Barbour
 * Maintainer: Andrew Barbour
 * Created: 19/11/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Rotates an object
 **/

using UnityEngine;
using System.Collections;

namespace StormsProject
{
    public class Rotator : MonoBehaviour
    {
        public Vector3 rotationAxis;
        public float speed;

        Transform _transform;

        public void Awake()
        {
            _transform = transform;
        }

        void Start()
        {

        }

        void Update()
        {
            _transform.Rotate(rotationAxis * speed * Time.deltaTime);
        }
    }
}
