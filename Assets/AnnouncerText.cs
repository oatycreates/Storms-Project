/**
* File: AnnouncerText.cs
* Author: RowanDonaldson
* Maintainers: Patrick Ferguson
* Created: 15/10/2015
* Copyright: (c) 2015 Team Storms, All Rights Reserved.
* Description: Behaviour for the Text/Screen Effects.
**/
		
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectStorms
{
    public class AnnouncerText : MonoBehaviour
    {

        public Text m_text;
        public float lifeTime = 1.0f;
        public float fadeMod = 1.5f;

        void Awake()
        {
            m_text = gameObject.GetComponent<Text>();
        }

        void OnEnable()
        {
            
        }

        void Update()
        {
            lifeTime -= (Time.deltaTime)/fadeMod;

            m_text.color = new Color(m_text.color.r, m_text.color.g, m_text.color.b, lifeTime);

            if (lifeTime < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
