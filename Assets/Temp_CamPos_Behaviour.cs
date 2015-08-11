using UnityEngine;
using System.Collections;
//Like the Temp_TrapdoorScript, this script only needs to work for now...

//This changes the cam pos while dropping off passengers.
public class Temp_CamPos_Behaviour : MonoBehaviour 
{

	private Vector3 localCamStartPos;
	private Vector3 localCamStartEuler;
	
	public AirshipControlBehaviour controller;
	private bool buttonDown;
	
	public Vector3 targetPos = new Vector3(0, 20, 11);
	public Vector3 targetEuler = new Vector3(90, 0, 0);
	public float speed = 2.0f;

	void Start () 
	{
		localCamStartPos = gameObject.transform.localPosition; // usually (0, 12, -25)
		localCamStartEuler = gameObject.transform.localEulerAngles; // usually (10, 0, 0)
	}
	
	void Update () 
	{
		//from world space to local space
		//Vector3 localTargetPos = gameObject.transform.TransformDirection(targetPos);
		//Vector3 localTargetEuler = gameObject.transform.TransformDirection(targetEuler);
	
		if (controller != null)
		{
			buttonDown = controller.openHatch;
		}
		
		if (buttonDown)
		{
			gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, targetPos, Time.deltaTime * speed);
			gameObject.transform.localEulerAngles = Vector3.Lerp(gameObject.transform.localEulerAngles, targetEuler, Time.deltaTime * speed);
		}
		else
		if (!buttonDown)
		{
			gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition, localCamStartPos, Time.deltaTime * speed);
			gameObject.transform.localEulerAngles = Vector3.Lerp(gameObject.transform.localEulerAngles, localCamStartEuler, Time.deltaTime * speed);
		
		}
	}
}
