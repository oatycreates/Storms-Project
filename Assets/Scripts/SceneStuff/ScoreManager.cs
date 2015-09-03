/**
 * File: ScoreManager.cs
 * Author: Rowan Donaldson
 * Maintainer: Patrick Ferguson
 * Created: 12/08/2015
 * Copyright: (c) 2015 Team Storms, All Rights Reserved.
 * Description: Manages scoring for each player/team in the game.
 **/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ProjectStorms
{

	public enum EGameType
	{
		FreeForAll,
		TeamGame
	}

    /// <summary>
    /// Manages scoring for each player in the game.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
       // public ENumberOfPlayers e_numberOfBases;
		public EGameType gameType;

        public int passengersToWin = 50;

        public Text winText;

        public PirateBaseIdentity pirateBase1;
        public PirateBaseIdentity pirateBase2;
        public PirateBaseIdentity pirateBase3;
        public PirateBaseIdentity pirateBase4;

		public PirateBaseIdentity teamBaseAlpha;
		public PirateBaseIdentity teamBaseOmega;

		public string alphaTeamName = "Alpha";
		public string omegaTeamName = "Omega";


        private Color m_winnerColour;
        private int m_winnerNumber;

        public FadeCamWhite fadeOutScene;

		public CinematicOutro optionalWinnerCam;

		//is game ending
		private bool gameOver = false;

        void Start()
        {
            winText.text = " ";
            m_winnerColour = Color.clear;

			if (gameType == EGameType.FreeForAll) 
			{
				pirateBase1.ResetPirateBase (passengersToWin);
				pirateBase2.ResetPirateBase (passengersToWin);
				pirateBase3.ResetPirateBase (passengersToWin);
				pirateBase4.ResetPirateBase (passengersToWin);
			}

			if (gameType == EGameType.TeamGame)
			{
				teamBaseAlpha.ResetPirateBase (passengersToWin);
				teamBaseOmega.ResetPirateBase (passengersToWin);


				//Set the colour of the bases
				teamBaseAlpha.teamGame = true;
				teamBaseOmega.teamGame = true;

				teamBaseAlpha.omegaBlack = false;
				teamBaseOmega.omegaBlack = true;
			}
        }


        void Update()
        {
           	if (gameType == EGameType.FreeForAll)
			{
				FourPlayerMatch();
			}
			else
			if (gameType == EGameType.TeamGame)
			{
				TeamMatch();
			}

			//Progress
			if (gameOver)
			{
				if (InputManager.GetAnyButtonDown ("Player1_") || InputManager.GetAnyButtonDown ("Player2_") || InputManager.GetAnyButtonDown ("Player3_") || InputManager.GetAnyButtonDown ("Player4_"))
				{
					FadeOut();
				}
			}
        }


		void TeamMatch()
		{
			//Alpha = team 1 = white
			//Omega = team 2 = black

			string winnerName = " ";

			if (teamBaseAlpha.baseScore <= 0)
			{
				winnerName = alphaTeamName;
				m_winnerColour = Color.white;
				TeamWin(winnerName, m_winnerColour);
			}
			else
			if (teamBaseOmega.baseScore <= 0)
			{
				winnerName = omegaTeamName;
				m_winnerColour = Color.black;
				TeamWin(winnerName, m_winnerColour);
			}

		}


		void FourPlayerMatch()
		{
			// Check to see if any base score is less than /equal to 0
			if (pirateBase1.baseScore <= 0)
			{
				m_winnerColour = pirateBase1.baseColour;
				m_winnerNumber = pirateBase1.teamNumber;
				PlayerWin(m_winnerNumber, m_winnerColour);
			}
			else if (pirateBase2.baseScore <= 0)
			{
				m_winnerColour = pirateBase2.baseColour;
				m_winnerNumber = pirateBase2.teamNumber;
				PlayerWin(m_winnerNumber, m_winnerColour);
			}
			else if (pirateBase3.baseScore <= 0)
			{
				m_winnerColour = pirateBase3.baseColour;
				m_winnerNumber = pirateBase3.teamNumber;
				PlayerWin(m_winnerNumber, m_winnerColour);
			}
			else if (pirateBase4.baseScore <= 0)
			{
				m_winnerColour = pirateBase4.baseColour;
				m_winnerNumber = pirateBase4.teamNumber;
				PlayerWin(m_winnerNumber, m_winnerColour);
			}
		}



		void TeamWin (string teamName, Color teamColour)
		{
			string teamWinner = teamName;

			//Set Text	
			//winText.text = ("Team " + teamWinner + " Wins!");
			winText.text = (teamWinner + " Wins!");

			//Set end game to true
			//Invoke ("GameOver", 5.0f);
			if (optionalWinnerCam != null)
			{
				optionalWinnerCam.gameObject.SetActive(true);
				//optionalWinnerCam.WinCam((teamWinner + "\nWins!"), teamColour);
				optionalWinnerCam.WinCam(winText.text, teamColour);
				
				
				winText.text = " ";

				//Fade out after 15 sec
				
				if (fadeOutScene != null)
				{
					Invoke("FadeOut", 15.0f);
				}
			}
			else
			{
				Debug.Log("No Cinematic Outro Prefab Attached");

				//Backup Manual Fadeout
				FadeOut();
			}
		}


        void PlayerWin(float a_playerNumber, Color a_colour)
        {
            string winner = a_playerNumber.ToString();

            //winText.text = ("Player " + winner + " Wins!");
            winText.text = ("Player " +winner + "\nWins!");
            winText.color = m_winnerColour;

			Invoke ("GameOver", 5.0f);



			//Play an Outro Cinematic if there is one present
			if (optionalWinnerCam != null)
			{
				optionalWinnerCam.gameObject.SetActive(true);
				optionalWinnerCam.WinCam(winText.text, m_winnerColour);


				winText.text = " ";

				if (fadeOutScene != null)
				{
					Invoke("FadeOut", 15.0f);
				}
			}
			else
			{
				Debug.Log("No outro attached");

				FadeOut();
			}
        }


		void GameOver()
		{
			//Toggle gameover
			gameOver = true;
		}


		void FadeOut()
		{
			if (fadeOutScene != null)
			{
				// check to see if the bool is not already true
				if (!fadeOutScene.fadeStart || !fadeOutScene.fadeEnd)
				{
					fadeOutScene.fadeEnd = true;
				}
			}
		}
    } 
}
