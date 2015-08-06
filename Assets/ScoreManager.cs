using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum ENumberOfPlayers 
{
	One, 
	Two, 
	Three, 
	Four
}

// This script keeps track of all the player's scores
public class ScoreManager : MonoBehaviour 
{
	public ENumberOfPlayers numberOfBases;
	
	public DetectFallingPassenger triggerZone_1;
	public DetectFallingPassenger triggerZone_2;
	public DetectFallingPassenger triggerZone_3;
	public DetectFallingPassenger triggerZone_4;
	
	[HideInInspector]
	public int score1;
	[HideInInspector]
	public int score2;
	[HideInInspector]
	public int score3;
	[HideInInspector]
	public int score4;

	public Text WinText;
	public FadeCamWhite fadeOut;	
	
	void Start()
	{
		WinText.text = "";
		
		//Set default values
		if (triggerZone_1 == null)
		{
			score1 = 50;
		}
		
		if (triggerZone_2 == null)
		{
			score2 = 50;
		}
		
		if (triggerZone_3 == null)
		{
			score3 = 50;
		}
		
		if (triggerZone_4 == null)
		{
			score4 = 50;
		}
	}
		
		
	void Update () 
	{
		if (numberOfBases == ENumberOfPlayers.One)
		{
			score1 = triggerZone_1.myScore;
			
		}
		
		if (numberOfBases == ENumberOfPlayers.Two)
		{
			score1 = triggerZone_1.myScore;
			score2 = triggerZone_2.myScore;
		}
		
		if (numberOfBases == ENumberOfPlayers.Three)
		{
			score1 = triggerZone_1.myScore;
			score2 = triggerZone_2.myScore;
			score3 = triggerZone_3.myScore;
		}
		
		if (numberOfBases == ENumberOfPlayers.Four)
		{
			score1 = triggerZone_1.myScore;
			score2 = triggerZone_2.myScore;
			score3 = triggerZone_3.myScore;
			score4 = triggerZone_4.myScore;
		}
		
		
		//Set win condition
		if (score1 <= 0)
		{
			Win(1);
		}
		else
		if (score2 <= 0)
		{
			Win(2);
		}
		else
		if (score3 <= 0)
		{
			Win(3);
		}
		else
		if (score4 <= 0)
		{
			Win(4);
		}
	}
	
	void Win(float player)
	{
		//fadeOut.fadeEnd = true;
		WinText.text = ("Player " + player + " Wins!");
	}
}
