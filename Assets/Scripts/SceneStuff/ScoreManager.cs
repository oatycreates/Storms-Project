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

namespace ProjectStorms
{
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

		public CinematicOutro optionalWinnerCam;

		//is game ending
		private bool gameOver = false;

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

			//Progress
			if (gameOver)
			{
				if (InputManager.GetAnyButtonDown ("Player1_") || InputManager.GetAnyButtonDown ("Player2_") || InputManager.GetAnyButtonDown ("Player3_") || InputManager.GetAnyButtonDown ("Player4_"))
				{
					FadeOut();
				}
			}
        }

        void Win(float a_playerNumber, Color a_colour)
        {
            string winner = a_playerNumber.ToString();

            //winText.text = ("Player " + winner + " Wins!");
            winText.text = ("Player " +winner + "\nWinnARRRRR!");
            winText.color = m_winnerColour;

            Debug.Log("Player " + winner + " Wins!");

			Invoke ("GameOver", 5.0f);

			//Play an Outro Cinematic if there is one present
			if (optionalWinnerCam != null)
			{
				optionalWinnerCam.gameObject.SetActive(true);
				optionalWinnerCam.WinCam(("Player " +winner + "\nWins!"), m_winnerColour);

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
