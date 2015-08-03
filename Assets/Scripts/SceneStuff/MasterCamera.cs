using UnityEngine;
using System.Collections;

public enum CamerasInScene{One,Two,Three,Four}

public class MasterCamera : MonoBehaviour 		// A very important script. This sets the camera's screen width/height depending on the number of players.
{												//For now, this has to be set manually.
	public CamerasInScene currentCamera;
	
	public Camera cam1;
	public Camera cam2;
	public Camera cam3;
	public Camera cam4;
	

	void Update () 
	{
		if (currentCamera == CamerasInScene.One)
		{
			cam1.enabled = true;
			cam2.enabled = false;
			cam3.enabled = false;
			cam4.enabled = false;
			
			cam1.rect = new Rect(0,0,1,1);
			
			
		}	
		
		if (currentCamera == CamerasInScene.Two)
		{
			cam1.enabled = true;
			cam2.enabled = true;
			cam3.enabled = false;
			cam4.enabled = false;
			
			cam1.rect = new Rect(0f,0.5f,1f,0.5f);	//One on top
			cam2.rect = new Rect(0,0.0f,1f,0.5f);
			
		}	
		
		if (currentCamera == CamerasInScene.Three)
		{
			cam1.enabled = true;
			cam2.enabled = true;
			cam3.enabled = true;
			cam4.enabled = false;
			
			cam1.rect = new Rect(0,0.5f, 0.5f,0.5f);
			cam2.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);	//Cam two top right
			cam3.rect = new Rect(0, 0, 1f, 0.5f);
			
		}	
		
		if (currentCamera == CamerasInScene.Four)
		{
			cam1.enabled = true;
			cam2.enabled = true;
			cam3.enabled = true;
			cam4.enabled = true;
			
			cam1.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
			cam2.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);
			cam3.rect = new Rect(0f, 0f, 0.5f, 0.5f);
			cam4.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
		}	
	}
	
}
