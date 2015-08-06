using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum NextScene{Menu, Game, Credits, Splash}

//Fade a scene camers TO or FROM white
public class FadeCamWhite : MonoBehaviour 
{
// Most of this was inspired by the Unity Example Tutorials

	public bool fadeStart = true;
	public bool fadeEnd = false;
	
	public float fadeSpeed = 1.5f;
	
	public Image whitePanel;
	private Color panelStartColor;
	
	//Switching Scenes
	public NextScene fadeToThisScene;
	private SceneManager sceneManagerScript;


	void Awake()
	{
		sceneManagerScript = gameObject.GetComponentInParent<SceneManager>();
	}
	
	void Start()
	{
		panelStartColor = whitePanel.color;
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
		
		//Debug.Log(whitePanel.color.a);
	}
	
	
	void FadeUp()
	{
		whitePanel.color = Color.Lerp(whitePanel.color, Color.clear, fadeSpeed * Time.deltaTime);
	}
	
	void FadeDown()
	{
		whitePanel.color = Color.Lerp(whitePanel.color, panelStartColor, fadeSpeed * Time.deltaTime);
	}
	
 	void StartScene()
 	{
 		FadeUp();
 		
 		//Find a margin between clear and "Very near clear" - from Unity examples
 		if (whitePanel.color.a <= 0.05f)	
 		{	
 			whitePanel.color = Color.clear;
 			whitePanel.gameObject.SetActive(false);
 			fadeStart = false;
 		}
 	}
 	
 	void EndScene()
 	{
 		//make sure the panel is enabled
 		whitePanel.gameObject.SetActive(true);
 	
 		FadeDown();
 		
 		if (whitePanel.color.a >= 0.95f)
 		{
 			//Go to next scene
 			Scene();
 		}
 	}
 	
 	void Scene()
 	{
 	  	if (fadeToThisScene == NextScene.Menu)
 	  	{
 	  		sceneManagerScript.MenuScene();
 	  	}
 	  	else
		if (fadeToThisScene == NextScene.Game)
		{
			sceneManagerScript.GameScene();	
		}
		else
		if (fadeToThisScene == NextScene.Credits)
		{
			sceneManagerScript.CreditsScene();
		}
		else
		if (fadeToThisScene == NextScene.Splash)
		{
			sceneManagerScript.SplashScreen();
		}
 	}
}
