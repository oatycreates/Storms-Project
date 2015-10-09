/**
 * File: FactionModelPreview.cs
 * Author: Andrew Barbour
 * Maintainers: Andrew Barbour
 * Created: 9/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Updates the faction model previews
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    public class FactionModelPreview : MonoBehaviour
    {
        public float rotateSpeed;

        [Header("Model References")]
        public Transform navyModel;
        public Transform piratesModel;
        public Transform tinkerersModel;
        public Transform vikingsModel;

        public void Awake()
        {
            
        }

        void Start()
        {

        }

        void Update()
        {
            // Rotate models
            RotateTransform(navyModel);
            RotateTransform(piratesModel);
            RotateTransform(tinkerersModel);
            RotateTransform(vikingsModel);
        }

        void RotateTransform(Transform a_transform)
        {
            // Rotate transform using given speed, within local space
            a_transform.Rotate(0.0f, rotateSpeed * Time.deltaTime, 0.0f, Space.Self);
        }
    }
}
