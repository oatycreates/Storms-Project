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

		[HideInInspector]
		public static bool teamGame = false;

        public int passengersToWin = 50;


        public PirateBaseIdentity pirateBase1;
        private int previousBase1Score;
        private string base1Name = " ";
		
        public PirateBaseIdentity pirateBase2;
        private int previousBase2Score;
        private string base2Name = " ";
		
        public PirateBaseIdentity pirateBase3;
        private int previousBase3Score;
        private string base3Name = " ";
        
        public PirateBaseIdentity pirateBase4;
        private int previousBase4Score;
        private string base4Name = " ";
        

		public PirateBaseIdentity teamBaseAlpha;
		private int previousAlphaScore;
		private string baseAlphaName = " ";
		public PirateBaseIdentity teamBaseOmega;
		private int previousOmegaScore;
		private string baseOmegaName = " ";
		
		
		//Set the team names
		/*
		public string alphaTeamName = "Alpha";
		public Color alphaTeamColour = Color.red;
		public string omegaTeamName = "Omega";
		public Color omegaTeamColour = Color.black;
		*/


		
		public Text winText;
        private Color m_winnerColour;
        private int m_winnerNumber;

        public FadeCamWhite fadeOutScene;

		public CinematicOutro optionalWinnerCam;
		//is game ending
		private bool gameOver = false;

        //check each base identity
        private string player1tag;
        private string player2tag;
        private string player3tag;
        private string player4tag;
        
        //Send messages to Announcer Effects
        /*
        public AnnouncerEffects announcerEffects;
        */
        
        public UI_Controller scoreText;
        private bool startDelayOver = false;
        
        private bool halfWayCheck1 = false;
        private bool halfWayCheck2 = false;
        private bool halfWayCheck3 = false;
        private bool halfWayCheck4 = false;
        

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

				teamGame = false;
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

				//make the reference
				teamGame = true;

			}
			
			Invoke("FinishStartDelay", 1);
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
			
			//check previous Scores
			if (gameType == EGameType.FreeForAll) 
			{
				previousBase1Score = pirateBase1.baseScore;
				previousBase2Score = pirateBase2.baseScore;
				previousBase3Score = pirateBase3.baseScore;
				previousBase4Score = pirateBase4.baseScore;
			}
			else
			if (gameType == EGameType.TeamGame)
			{
				previousBase1Score = teamBaseAlpha.baseScore;
				previousBase2Score = teamBaseOmega.baseScore;
			}
			
			
			//get names
			base1Name = pirateBase1.GetComponent<FactionIndentifier>().name;
			base2Name = pirateBase2.GetComponent<FactionIndentifier>().name;
			base3Name = pirateBase3.GetComponent<FactionIndentifier>().name;
			base4Name = pirateBase4.GetComponent<FactionIndentifier>().name;
			baseAlphaName = teamBaseAlpha.GetComponent<FactionIndentifier>().name;
			baseOmegaName = teamBaseOmega.GetComponent<FactionIndentifier>().name;
		
        }
        	
        void FinishStartDelay()
        {
        	startDelayOver = true;
        }


		void TeamMatch()
		{
			string winnerName = " ";
			
			if (startDelayOver)
			{
				//Check to see if any base has less score than last update
				if (previousAlphaScore != teamBaseAlpha.baseScore)
				{
					Score(baseAlphaName);
				}
				
				if (previousOmegaScore != teamBaseOmega.baseScore)
				{
					Score(baseOmegaName);
				}
			}

			/*
			if (teamBaseAlpha.baseScore <= 0)
			{
				
				winnerName = baseAlphaName; //alphaTeamName;
				m_winnerColour = alphaTeamColour;
				TeamWin(winnerName, m_winnerColour);
				
			}
			else
			if (teamBaseOmega.baseScore <= 0)
			{
				
				winnerName = omegaTeamName;
				m_winnerColour = omegaTeamColour;
				TeamWin(winnerName, m_winnerColour);
				
			}
			*/
		}


		void FourPlayerMatch()
		{
			if (startDelayOver)
			{
				//Check to see if any base has less score than last update
				if (previousBase1Score != pirateBase1.baseScore)
				{
					//Get faction identifier
					Score ( pirateBase1.GetComponent<FactionIndentifier>().factionName);
					
				}
				
				if (previousBase2Score != pirateBase2.baseScore)
				{
					//Score(base2Name);
					Score ( pirateBase2.GetComponent<FactionIndentifier>().factionName);
				}
				
				if (previousBase3Score != pirateBase3.baseScore)
				{
					//Score(base3Name);
					Score ( pirateBase3.GetComponent<FactionIndentifier>().factionName);
				}
				
				if (previousBase4Score != pirateBase4.baseScore)
				{
					//Score(base4Name);
					Score ( pirateBase4.GetComponent<FactionIndentifier>().factionName);
				}
			}
			
			
			
			// Check to see if any bases are less than Halfway 
			if (pirateBase1.baseScore < (passengersToWin/2))
			{
				if (!halfWayCheck1)
				{
					HalfWay ( pirateBase1.GetComponent<FactionIndentifier>().factionName);
					halfWayCheck1 = true;
				}
			}	
			
			if (pirateBase2.baseScore < (passengersToWin/2))
			{
				if (!halfWayCheck2)
				{
					HalfWay ( pirateBase2.GetComponent<FactionIndentifier>().factionName);
					halfWayCheck2 = true;
				}
			}	
			
			if (pirateBase3.baseScore < (passengersToWin/2))
			{
				if (!halfWayCheck3)
				{
					HalfWay ( pirateBase3.GetComponent<FactionIndentifier>().factionName);
					halfWayCheck3 = true;
				}
			}	
			
			if (pirateBase4.baseScore < (passengersToWin/2))
			{
				if (!halfWayCheck4)
				{
					HalfWay ( pirateBase4.GetComponent<FactionIndentifier>().factionName);
					halfWayCheck4 = true;
				}
			}	
			
			
			
		
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

			Invoke ("GameOver", 2.0f);



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
		
		public void Score(string teamName)	//Base Numbers 1-4 for Free4All Match, 	Base Numbers 5 & 6 for Team Match
		{
			//Debug.Log(teamName + "  Scores!");
			
			if (teamName == "NONAME")
			{
				Debug.Log(teamName);
			}
			else
			if (teamName == null)
			{
				Debug.Log("no team name");
			}
			else
			if (teamName == "PIRATES")
			{
				scoreText.Score(teamName);
			}
			else
			if (teamName == "NAVY")
			{
				scoreText.Score(teamName);
			}
			else
			if (teamName == "TINKERERS")
			{
				scoreText.Score(teamName);
			}
			else
			if (teamName == "VIKINGS")
			{
				scoreText.Score(teamName);
			}
		}
		
		public void HalfWay(string teamName)
		{
			if (teamName == "NONAME")
			{
				Debug.Log(teamName);
			}
			else
			if (teamName == null)
			{
				Debug.Log("no team name");
			}
			else
			if (teamName == "PIRATES")
			{
				scoreText.HalfWay(teamName);
			}
			else
			if (teamName == "NAVY")
			{
				scoreText.HalfWay(teamName);
			}
			else
			if (teamName == "TINKERERS")
			{
				scoreText.HalfWay(teamName);
			}
			else
			if (teamName == "VIKINGS")
			{
				scoreText.HalfWay(teamName);
			}
		}
    } 
}
