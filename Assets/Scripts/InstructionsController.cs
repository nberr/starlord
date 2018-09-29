using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//John Bradbury
public class InstructionsController : MonoBehaviour {

   public GameUIController uiController;
   public GameObject targetObject;
   
   // Use this for initialization
   void Start () {
   }
   
   // Update is called once per frame
   void Update () {
      
   }
   
   void OnEnable() {
      Time.timeScale = 0.0f;
   }
   
   public void closeButton() {
      uiController.TogglePause(false);
      Time.timeScale = 1.0f;
      targetObject.SetActive(false);
   }
}
