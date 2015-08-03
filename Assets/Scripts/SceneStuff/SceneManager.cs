using UnityEngine;
using System.Collections;

public class SceneManager : MonoBehaviour //This script changes the current scene/ level or room.
{

	void Update () 
	{
		if (Application.isEditor == true)
		{
			if (Input.GetKeyDown(KeyCode.M))
			{
				MenuScene();
			}
			
			if (Input.GetKeyDown(KeyCode.G))
			{
				GameScene();
			}
			
			if (Input.GetKeyDown(KeyCode.C))
			{
				CreditsScene();
			}	
			
			if (Input.GetKeyDown(KeyCode.Backspace))
			{
				Application.LoadLevel(Application.loadedLevelName);
			}
		}
		
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}
	
	public void MenuScene()
	{
		Application.LoadLevel("MenuScene");
	}
	
	public void GameScene()
	{
		Application.LoadLevel("GameScene");
	}
	
	public void CreditsScene()
	{
		Application.LoadLevel("CreditsScene");
	}
}
