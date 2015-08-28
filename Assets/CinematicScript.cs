using UnityEngine;
using System.Collections;

public class CinematicScript : MonoBehaviour 
{
	public float twirlSpeed = 20;

	public GameObject upDownTarget;
	private float upDown;

	//Need to make it work in local space

	void Update () 
	{
		gameObject.transform.Rotate (Vector3.up * twirlSpeed * Time.deltaTime);

		upDown = Mathf.PingPong (Time.time, -3);

		upDownTarget.transform.position = new Vector3 (upDownTarget.transform.position.x, upDown, upDownTarget.transform.position.z);

	}
}
