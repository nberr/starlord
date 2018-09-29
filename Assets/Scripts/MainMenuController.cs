using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//John Bradbury
public class MainMenuController : MonoBehaviour {

   private bool statsOpen = false;
   public GameObject statsPanel;
   public Text statsHighscore;
   public Text statsKilled;
   public Text statsNumGames;
   
   // Use this for initialization
   void Start () {
      GameControl.ReadStats();
   }
   
   // Update is called once per frame
   void Update () {
      statsHighscore.text = "Highscore: " + GameControl.Stat_HighScore.ToString();
      statsKilled.text = "Best # Enemies Killed: " + GameControl.Stat_EnemiesKilled.ToString();
      statsNumGames.text = "# Games Played: " + GameControl.Stat_NumGamesPlayed.ToString();
   }
   
   public void LoadStage(string target) {
      if (!statsOpen) {
         Time.timeScale = 1f;
         SceneManager.LoadScene(target);
      }
   }
   
   public void OpenStats() {
      statsOpen = true;
      statsPanel.SetActive(true);
   }
   
   public void CloseStats() {
      statsOpen = false;
      statsPanel.SetActive(false);
   }
   
   public void Exit() {
      if (!statsOpen) {
         Application.Quit();
      }
   }
}
