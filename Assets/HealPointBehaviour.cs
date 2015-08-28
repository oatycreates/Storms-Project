using UnityEngine;
using System.Collections;

public class HealPointBehaviour : MonoBehaviour 
{

	void OnTriggerEnter(Collider other)
	{
		if ((other.gameObject.tag == "Player1_")||(other.gameObject.tag == "Player2_")||(other.gameObject.tag == "Player3_")||(other.gameObject.tag == "Player4_"))
		{
			if(other.transform.root.gameObject.GetComponent<ShipPartDestroy>() != null)
			{
				other.transform.root.gameObject.GetComponent<ShipPartDestroy>().RepairAllParts();
			}
			else
			{
				other.transform.root.gameObject.GetComponentInChildren<ShipPartDestroy>().RepairAllParts();
			}

		}
	}
}
