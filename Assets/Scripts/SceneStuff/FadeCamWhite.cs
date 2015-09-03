/**
 * File: FadeCamWhite.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages the cinematic fade-through-white for scene transitions.
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectStorms
{
    public enum ENextScene
    {
		Empty,
        Menu,
        Game,
        Credits,
        Splash,
        LoopLevel,
        TestScene,
		CoOpTeamScene
    }

    /// <summary>
    /// Fade a scene camers TO or FROM white
    /// </summary>
    public class FadeCamWhite : MonoBehaviour
    {
        // Most of this was inspired by the Unity Example Tutorials
        public bool fadeStart = true;
        public bool fadeEnd = false;

        public float fadeSpeed = 1.5f;

        public Image whitePanel;
        private Color m_panelStartColor;

        /// <summary>
        /// Switching Scenes.
        /// </summary>
        public ENextScene fadeToThisScene;
        private SceneManager m_sceneManagerScript;


        void Awake()
        {
            m_sceneManagerScript = gameObject.GetComponentInParent<SceneManager>();
        }

        void Start()
        {
            m_panelStartColor = whitePanel.color;
        }

        void Update()
        {
            if (fadeStart)
            {
                StartScene();
            }

            if (fadeEnd)
            {
                EndScene();
            }
        }

        void FadeUp()
        {
            whitePanel.color = Color.Lerp(whitePanel.color, Color.clear, fadeSpeed * Time.deltaTime);
        }

        void FadeDown()
        {
            whitePanel.color = Color.Lerp(whitePanel.color, m_panelStartColor, fadeSpeed * Time.deltaTime);
        }

        void StartScene()
        {
            FadeUp();

            // Find a margin between clear and "Very near clear" - from Unity examples
            if (whitePanel.color.a <= 0.05f)
            {
                whitePanel.color = Color.clear;
                whitePanel.gameObject.SetActive(false);
                fadeStart = false;
            }
        }

        void EndScene()
        {
            // Make sure the panel is enabled
            whitePanel.gameObject.SetActive(true);

            FadeDown();

            if (whitePanel.color.a >= 0.95f)
            {
                // Go to next scene
                Scene();
            }
        }

        void Scene()
        {
			if (fadeToThisScene == ENextScene.Empty)
			{
				Debug.Log("Stay Here");
			}
			else
	            if (fadeToThisScene == ENextScene.Menu)
	            {
	                m_sceneManagerScript.MenuScene();
	            }
	            else
	                if (fadeToThisScene == ENextScene.Game)
	                {
	                    m_sceneManagerScript.GameScene();
	                }
	                else
	                    if (fadeToThisScene == ENextScene.Credits)
	                    {
	                        m_sceneManagerScript.CreditsScene();
	                    }
	                    else
	                        if (fadeToThisScene == ENextScene.Splash)
	                        {
	                            m_sceneManagerScript.SplashScreen();
	                        }
	                        else
	                            if (fadeToThisScene == ENextScene.LoopLevel)
	                            {
	                                m_sceneManagerScript.LoopCurrentLevel();
	                            }
	                            else
	                                if (fadeToThisScene == ENextScene.TestScene)
	                                {
	                                    m_sceneManagerScript.TestScene();
	                                }
									else
										if (fadeToThisScene == ENextScene.CoOpTeamScene)
										{
											m_sceneManagerScript.CoOpTeamMatch();
										}
        }
    } 
}
