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
	public ENumberOfPlayers e_numberOfBases;

	public Text winText;

	public PirateBaseIdentity pirateBase1;
	public PirateBaseIdentity pirateBase2;
	public PirateBaseIdentity pirateBase3;
	public PirateBaseIdentity pirateBase4;


	private int baseScore_1;
	private int baseScore_2;
	private int baseScore_3;
	private int baseScore_4;


	private Color winnerColour;
	private int winnerNumber;


	void Start()
	{
		winText.text = " ";
		winnerColour = Color.clear;
	}

	void Update()
	{
		
		//Lock the empty base values so that they Cannot 'win'
		if (e_numberOfBases == ENumberOfPlayers.One)
		{
				baseScore_2 = 50;

				baseScore_3 = 50;

				baseScore_4 = 50;
		}
		else
		if (e_numberOfBases == ENumberOfPlayers.Two)
		{
				baseScore_3 = 50;

				baseScore_4 = 50;
		}
		else
		if (e_numberOfBases == ENumberOfPlayers.Three)
		{

				baseScore_4 = 50;
		}
		else
		if (e_numberOfBases == ENumberOfPlayers.Four)
		{
		}


		//Check to see if any base score is less than 0

		if (pirateBase1.baseScore <= 0)
		{
			winnerColour = pirateBase1.baseColour;
			winnerNumber = pirateBase1.teamNumber;
			Win(winnerNumber, winnerColour); 
		}
		else
		if (pirateBase2.baseScore <= 0)
		{
			winnerColour = pirateBase2.baseColour;
			winnerNumber = pirateBase2.teamNumber;
			Win(winnerNumber, winnerColour); 
		}
		else
		if (pirateBase3.baseScore <= 0)
		{
			winnerColour = pirateBase3.baseColour;
			winnerNumber = pirateBase3.teamNumber;
			Win(winnerNumber, winnerColour); 
		}
		else
		if (pirateBase4.baseScore <= 0)
		{
			winnerColour = pirateBase4.baseColour;
			winnerNumber = pirateBase4.teamNumber;
			Win(winnerNumber, winnerColour); 
		}


	}
	
	void Win(float a_playerNumber, Color a_colour)
	{
		string winner = a_playerNumber.ToString();

		winText.text = ("Player " + winner + " Wins!");
		winText.color = winnerColour;

		//fadeOut.fadeEnd = true;
	}
}
