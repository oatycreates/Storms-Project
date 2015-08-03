using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InEditorStuff : MonoBehaviour 	//Everything here is only for ease of access, and should only effect stuff in the editor. // We should delete the Entire 'InEditor' branch of the airshipGameobject before master build.
{
	public Renderer myRenderer;
	private Color playerColor = Color.magenta;
	
	public GameObject airshipTopOfHierachy;
	public GameObject canvasChild;
	private Text canvasText;
	
	void Start()
	{
		canvasText = canvasChild.GetComponentInChildren<Text>();
		
		if (Application.isEditor == false)
		{
			canvasChild.SetActive(false);
		}
	}

	
	void Update () 
	{
	
	//set color
		if (myRenderer.enabled == true)
		{
			if (Application.isEditor == true)
			{
				myRenderer.material.color = playerColor;	
			}
		
			if (gameObject.tag == "Player1_")
			{
				playerColor = Color.magenta;
			}
			
			if (gameObject.tag == "Player2_")
			{
				playerColor = Color.cyan;
			}
			
			if (gameObject.tag == "Player3_")
			{
				playerColor = Color.green;
			}
			
			if (gameObject.tag == "Player4_")
			{
				playerColor = Color.yellow;
			}
		}
		
		//explain game states
		canvasText.text = ("State: " + (airshipTopOfHierachy.GetComponent<StateManager>().currentPlayerState));
	}
}
