/**
* File: GIFAnimator.cs
	* Author: RowanDonaldson
		* Maintainers: Patrick Ferguson
		* Created: 04/10/2015
		* Copyright: (c) 2015 Team Storms, All Rights Reserved.
		* Description: Flip through textures taken from GIF sprites.
		**/
		
using UnityEngine;
using System.Collections;

namespace ProjectStorms
{
    public class GIFAnimator : MonoBehaviour
    {
        public Texture[] frames;
        //public float framesPerSec = 10;
        public float changeFrameInterval = 0.33f;
        private Renderer rend;


        void Start()
        {
            rend = gameObject.GetComponent<Renderer>();
        }

        void Update()
        {
           //float index = (Time.time * framesPerSec) % frames.Length;

            //Derive code from Unity API examples - Main Texture page
            if (frames.Length == 0)
                return;

            int index = Mathf.FloorToInt(Time.time / changeFrameInterval);

            index = index % frames.Length;

           rend.material.mainTexture = frames[index];
            
        }
    }
}
