/**
 * File: Cam_DollyForward.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 18/09/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: This script makes the camera shake.
 **/
using UnityEngine;
using System.Collections;

public class ScreenShake : MonoBehaviour 
{
    //Transform holding the current location of the camera.
    private Transform myTransform;

    //Timer used for determining how long the camera should continue shaking for.
    private float shake = 0.0f;
    //The amount that the camera should shake.
    public float shakeAmount = 0.5f;

	void Start () 
    {
        myTransform = gameObject.GetComponent<Transform>() as Transform;

        InvokeRepeating("CamShake", 0, 1.0f);
	}
	
	void Update () 
    {
        Vector3 startPos = myTransform.localPosition;

        shake -= Time.deltaTime;

        if (shake > 0)
        {
            myTransform.localPosition = startPos + Random.insideUnitSphere * shakeAmount;
        }
        else
        {
            myTransform.localScale = startPos;
        }
	}

    public void CamShake()
    {
        shake = 0.4f;
    }
}
