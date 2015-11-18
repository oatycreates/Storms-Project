using UnityEngine;
using System.Collections;

public class CogSpriteRotate : MonoBehaviour 
{
	public float rotateSpeed = 1;

	void Update()
	{
		gameObject.transform.Rotate( Vector3.forward * rotateSpeed, Space.Self);	
	}
}
