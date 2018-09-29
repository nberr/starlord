using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * @author: John Bradbury, Ken Oshima
 * 
 */

public class GameUIController : MonoBehaviour {
   
   public const string OUR_TEXT_COLOR = "#ff8f8fff";
   public const string ENEMY_TEXT_COLOR = "#ff0000ff";
   
   public GameObject upgradeButton;
   public GameObject shopButton;
   public GameObject pauseButton;

   public GameObject waveStuff;
   public GameObject scoreText;
   public GameObject massText;
   
   public GameObject gameOverPopup;
   public Text statScore;
   public Text statKilled;

   public GameObject pauseMenu;
   public GameObject shopMenu;
   public GameObject upgradeMenu;
   
   public GameObject thoughtText;
   public float thoughtTextSpeed = 0.1f; //# seconds until next character
   private float timeUntilNextThoughtChar = 0.0f;
   public GameObject introText;
   public SpriteRenderer backgroundImg;
   public SpriteRenderer playerImg;
   public GameObject popupListObject;
   
   public GameObject beamObject;
   public GameObject centerTurretObject;
   
   private List<List<string>> thoughtQueue = new List<List<string>>();
   private float sleepTime = 0.0f;
   
   //John Bradbury
   // Use this for initialization
   void Start () {
      //disable everything, burn it all. Even if it's dead already, kill it again
      upgradeButton.SetActive(false);
      pauseMenu.SetActive(false);
      shopMenu.SetActive(false);
      upgradeMenu.SetActive(false);
      shopButton.SetActive(false);
      waveStuff.SetActive(false);
      scoreText.SetActive(false);
      massText.SetActive(false);
      pauseButton.SetActive(false);
      
      Color tmp = backgroundImg.color;
      tmp.a = 1.0f;
      backgroundImg.color = tmp;
      
      tmp = playerImg.color;
      tmp.a = 0.0f;
      playerImg.color = tmp;
   }
   
   // Update is called once per frame
   void Update () {
      if (Input.GetMouseButtonDown(0)) {
         DequeueThought(0.0f, true); //finish current thought. If it is already finished, then go to next thought if any
      } else {
         DequeueThought(Time.deltaTime, false);
      }
         
      if (EnemySpawner.instance.spawnMode && shopMenu.activeSelf) {
         // If a turret is being dragged, remove it
         foreach (ShopController s in shopMenu.GetComponentsInChildren<ShopController>())
            s.RemoveTurret ();
         
         shopMenu.SetActive (false);
      }
   }
   
   public void ExitGame() {
      GameControl.ExitGame();
      SceneManager.LoadScene("main_menu");
   }
   
   public void TogglePauseMenu(bool open) {
      pauseMenu.SetActive(open);
      shopMenu.SetActive (false);
      upgradeMenu.SetActive (false);

      Time.timeScale = open ? 0 : 1F;
      GameControl.instance.togglePauseGame(open);
   }

   // Ken Oshima
   public void ToggleShopMenu(bool open) {
      if (!EnemySpawner.instance.spawnMode && !pauseMenu.activeInHierarchy) {
         shopMenu.SetActive (open);
         upgradeMenu.SetActive (false);
      }
      //Time.timeScale = open ? 0 : 1F;
      //GameControl.instance.togglePauseGame(open);
   }

   // Ken Oshima
   public void ToggleUpgradeMenu(bool open) {
      if (pauseMenu.activeInHierarchy)
         return;

      upgradeMenu.SetActive (open);
      shopMenu.SetActive (false);

      //Time.timeScale = open ? 0 : 1F;
      //GameControl.instance.togglePauseGame(open);
   }

   //John Bradbury
   //cutscene stuff (intro text and word text)
   //add the thought to a queue, then slowly dequeue based on time and user input
   public void WriteThought(string speakerName, string msg, string hex_color, bool isIntro) {
      //prefix, total msg, current msg
      thoughtQueue.Add(new List<string>(new string [] {"<color=" + hex_color + ">" + speakerName + (speakerName.Length > 0 ? ": " : ""), msg, "", isIntro ? "intro" : "thought"}));
   }
   
   //John Bradbury
   void DequeueThought(float deltaTime, bool completeMsg) {
      if (thoughtQueue.Count == 0 || Time.timeScale == 0.0f) return;
      if (sleepTime > 0.0f) {
         sleepTime -= deltaTime;
         if (sleepTime > 0.0f) return;
         sleepTime = 0.0f;
      }
      //Debug.Log(thoughtQueue[0][0] + " / " + thoughtQueue[0][1] + " / " + thoughtQueue[0][2] + " / " + thoughtQueue[0][3]);
      if (thoughtQueue[0][3] == "intro" || thoughtQueue[0][3] == "thought") {
         
         if (completeMsg) {
			//if (isIntro) introText.SetActive(true);
			
            if (thoughtQueue[0][1] == thoughtQueue[0][2]) {
               thoughtQueue.RemoveAt(0); //remove the first msg (all done)
               thoughtText.GetComponent<Text>().text = "";
               introText.SetActive(false);//GetComponent<TextMesh>().text = "";
               return;
            } else {
               thoughtQueue[0][2] = thoughtQueue[0][1]; //display whole msg
            }
         } else {
				if (thoughtQueue[0][3] == "intro") introText.SetActive(true);

            if (thoughtQueue[0][1] == thoughtQueue[0][2]) return; //already displayed
            if (timeUntilNextThoughtChar <= 0.0) {
               timeUntilNextThoughtChar = thoughtTextSpeed;
               thoughtQueue[0][2] += thoughtQueue[0][1][thoughtQueue[0][2].Length];
            } else {
               timeUntilNextThoughtChar -= deltaTime;
            }
         }
         
         if (thoughtQueue.Count == 0) {
            thoughtText.GetComponent<Text>().text = "";
            //introText.SetActive(false);//GetComponent<TextMesh>().text = "";
            return;
         };
         if (thoughtQueue[0][3] == "intro") {
            thoughtText.GetComponent<Text>().text = "";
            introText.GetComponent<TextMesh>().text = thoughtQueue[0][0] + thoughtQueue[0][2] + "</color>";
         } else if (thoughtQueue[0][3] == "thought") {
            introText.SetActive(false);//GetComponent<TextMesh>().text = "";
            thoughtText.GetComponent<Text>().text = thoughtQueue[0][0] + thoughtQueue[0][2] + "</color>";
         }
      } else if (thoughtQueue[0][3] == "pause") {
         GameControl.instance.togglePauseGame(thoughtQueue[0][0] == "true");
         thoughtQueue.RemoveAt(0);
      } else if (thoughtQueue[0][3] == "fade") {
         SpriteRenderer targetImg = backgroundImg;
         if (thoughtQueue[0][0] == "player") {
            targetImg = playerImg;
            
            Color tmp = targetImg.color;
            tmp.a += deltaTime;
            if (tmp.a >= 1.0f) {
               tmp.a = 1.0f;
               thoughtQueue.RemoveAt(0);
            }
            targetImg.color = tmp;
         } else if (thoughtQueue[0][0] == "background") {
            Color tmp = targetImg.color;
            tmp.a -= deltaTime;
            if (tmp.a <= 0.0f) {
               tmp.a = 0.0f;
               thoughtQueue.RemoveAt(0);
            }
            targetImg.color = tmp;
         } else {
            //just reenable the ui
            shopButton.SetActive(true);
            pauseButton.SetActive(true);
            upgradeButton.SetActive (true);

            waveStuff.SetActive(true);
            scoreText.SetActive(true);

            massText.SetActive(true);

            thoughtQueue.RemoveAt(0);
         }
      } else if (thoughtQueue[0][3] == "popup") {
         GameObject popup = popupListObject.transform.Find(thoughtQueue[0][0]).gameObject;
         if (popup) {
            popup.SetActive(true);
            TogglePause(true); //the popup must send back TogglePause(false)
            thoughtQueue.RemoveAt(0);
         } else {
            Debug.Log("no popup: " + thoughtQueue[0][0]);
         }
      } else if (thoughtQueue[0][3] == "enable") {
         if (thoughtQueue[0][0] == "beam") {
            beamObject.SetActive(true);
         } else if (thoughtQueue[0][0] == "center_turret") {
            centerTurretObject.SetActive(true);
         }
         thoughtQueue.RemoveAt(0);
      } else if (thoughtQueue[0][3] == "sleep") {
         sleepTime = float.Parse(thoughtQueue[0][0]);
         thoughtQueue.RemoveAt(0);
      }
   }
   
   //John Bradbury
   public void TogglePause(bool enable) {
      thoughtQueue.Add(new List<string>(new string [] {enable ? "true" : "false", "", "", "pause"}));
   }
   
   //John Bradbury
   public void FadeIn(string target) {
      thoughtQueue.Add(new List<string>(new string [] {target, "", "", "fade"}));
   }
   
   //John Bradbury
   public void ShowPopup(string target) {
      thoughtQueue.Add(new List<string>(new string [] {target, "", "", "popup"}));
   }
   
   //John Bradbury
   public void EnableObject(string target) {
      thoughtQueue.Add(new List<string>(new string [] {target, "", "", "enable"}));
   }
   
   //John Bradbury
   public void Sleep(float time) {
      thoughtQueue.Add(new List<string>(new string [] {time.ToString(), "", "", "sleep"}));
   }
   
   //John Bradbury
   public void DisplayGameOver(int finalScore, int enemiesKilled) {
      gameOverPopup.SetActive(true);
      statScore.text = "Final Score: " + finalScore.ToString();
      statKilled.text = "Enemies Killed: " + enemiesKilled.ToString();
   }
}
