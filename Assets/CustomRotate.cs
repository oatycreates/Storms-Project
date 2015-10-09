using UnityEngine;
using System.Collections;

public class CustomRotate : MonoBehaviour 
{
	void Update () 
    {
        gameObject.transform.Rotate(Vector3.forward * -80* Time.deltaTime, Space.Self);
	}
}
