/**
 * File: CustomRotate.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 08/10/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Makes the attached game object rotate in local space. - Used for effects in the instruction screens.
 **/

using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    public class CustomRotate : MonoBehaviour
    {
        void Update()
        {
            gameObject.transform.Rotate(Vector3.forward * -80 * Time.deltaTime, Space.Self);
        }
    }
}
