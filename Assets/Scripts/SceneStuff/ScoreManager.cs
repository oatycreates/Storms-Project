/**
 * File: ScoreManager.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages scoring for each player in the game.
 **/

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

/// <summary>
/// Manages scoring for each player in the game.
/// </summary>
public class ScoreManager : MonoBehaviour 
{
	public ENumberOfPlayers e_numberOfBases;
	
	public int PassengersToWin = 10;

	public Text winText;

	public PirateBaseIdentity pirateBase1;
	public PirateBaseIdentity pirateBase2;
	public PirateBaseIdentity pirateBase3;
	public PirateBaseIdentity pirateBase4;

	private Color m_winnerColour;
	private int m_winnerNumber;
	
	public FadeCamWhite fadeOutScene;

	void Start()
	{
		winText.text = " ";
		m_winnerColour = Color.clear;

        pirateBase1.ResetPirateBase(PassengersToWin);
        pirateBase2.ResetPirateBase(PassengersToWin);
        pirateBase3.ResetPirateBase(PassengersToWin);
        pirateBase4.ResetPirateBase(PassengersToWin);
	}

	void Update()
	{
		// Check to see if any base score is less than /equal to 0
		if (pirateBase1.baseScore <= 0)
		{
			m_winnerColour = pirateBase1.baseColour;
			m_winnerNumber = pirateBase1.teamNumber;
			Win(m_winnerNumber, m_winnerColour); 
		}
		else if (pirateBase2.baseScore <= 0)
		{
			m_winnerColour = pirateBase2.baseColour;
			m_winnerNumber = pirateBase2.teamNumber;
			Win(m_winnerNumber, m_winnerColour); 
		}
		else if (pirateBase3.baseScore <= 0)
		{
			m_winnerColour = pirateBase3.baseColour;
			m_winnerNumber = pirateBase3.teamNumber;
			Win(m_winnerNumber, m_winnerColour); 
		}
		else if (pirateBase4.baseScore <= 0)
		{
			m_winnerColour = pirateBase4.baseColour;
			m_winnerNumber = pirateBase4.teamNumber;
			Win(m_winnerNumber, m_winnerColour); 
		}
	}
	
	void Win(float a_playerNumber, Color a_colour)
	{	
		string winner = a_playerNumber.ToString();

		winText.text = ("Player " + winner + " Wins!");
		winText.color = m_winnerColour;
		
		Debug.Log("Player " + winner + " Wins!");

		//fadeOut.fadeEnd = true;
		if (fadeOutScene != null)
		{
			// check to see if the bool is not already true
			if (!fadeOutScene.fadeStart || !fadeOutScene.fadeEnd)
			{
				fadeOutScene.fadeEnd = true;
			}
		}
		else
		{
			Debug.Log("No Scene to Fade between. See Screen Fader Script");
		}
	}
}
