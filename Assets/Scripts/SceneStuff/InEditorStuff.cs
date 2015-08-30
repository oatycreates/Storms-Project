/**
 * File: InEditorStuff.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Editor-only scripts. Everything here is only for ease of access, and should only effect stuff in the editor.
 *              We should delete the Entire 'InEditor' branch of the airshipGameobject before master build.
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectStorms
{
    /// <summary>
    /// Everything here is only for ease of access, and should only effect stuff in the editor.
    /// We should delete the Entire 'InEditor' branch of the airshipGameobject before master build.
    /// </summary>
    public class InEditorStuff : MonoBehaviour
    {

        public Renderer myRenderer;
        public Renderer myOtherRenderer;
        private Color m_playerColor = Color.magenta;

        public GameObject airshipTopOfHierachy;
        public GameObject canvasChild;
        private Text m_canvasText;

        // Cached variables
        StateManager m_stateManager;

        void Start()
        {
            m_canvasText = canvasChild.GetComponentInChildren<Text>();
            m_stateManager = airshipTopOfHierachy.GetComponent<StateManager>();
            /*
            if (Application.isEditor == false)
            {
                canvasChild.SetActive(false);
            }*/
        }


        void Update()
        {
            // Set color
            if (myRenderer.enabled == true)
            {
                /*
                if (Application.isEditor == true)
                {
                    myRenderer.material.color = playerColor;	
                }
                */
                myRenderer.material.color = m_playerColor;

                string myTag = tag;
                if (myTag == "Player1_")
                {
                    m_playerColor = Color.magenta;
                }

                if (myTag == "Player2_")
                {
                    m_playerColor = Color.cyan;
                }

                if (myTag == "Player3_")
                {
                    m_playerColor = Color.green;
                }

                if (myTag == "Player4_")
                {
                    m_playerColor = Color.yellow;
                }

                //In case of a second render object
                if (myOtherRenderer != null)
                {
                    if (myOtherRenderer.enabled == true)
                    {
                        myOtherRenderer.material.color = m_playerColor;
                    }
                }
            }

            if (Application.isEditor)
            {
                // Explain game states
                m_canvasText.text = ("State: " + (m_stateManager.GetPlayerState()));
            }
            else
            {
                m_canvasText.text = "";
            }
        }
    } 
}
