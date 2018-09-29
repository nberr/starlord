using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/* 
 * @author: Ken Oshima
 */

public class ShopController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler {

   public GameObject poolGameObject;
   public GameObject turretType;
   public GameObject notEnoughMassText;
   public int turretCost = 50;
   private DynamicObjectPool dynamicPool;
   private GameObject turretToSpawn;
   private bool isDragged = false;
   private bool onImage = false;
   private readonly float MaxTimer = 1f;
   private float timer = 0; 

	// Use this for initialization
	void Start () { 
      dynamicPool = (DynamicObjectPool)poolGameObject.GetComponent(typeof(DynamicObjectPool));
      turretToSpawn = null;
   }
	
	// Update is called once per frame
	void Update () {
      if (onImage && PlayerController.instance.mass - turretCost < 0) {
         notEnoughMassText.SetActive (true);
         //Debug.Log ("Not enough: " + PlayerController.instance.mass);
      } else {
         notEnoughMassText.SetActive (false);
         //Debug.Log ("Enough: " + PlayerController.instance.mass);
      }
   }


   void FixedUpdate() {

      if (turretToSpawn != null && isDragged) {
         Vector2 target = MouseControl.GetWorldPositionOnPlane(Input.mousePosition, 0f);

         turretToSpawn.transform.position = target;
         turretToSpawn.GetComponent<CircleCollider2D> ().transform.position = target;
      }
   }

   public void OnPointerEnter(PointerEventData eventData)
   {
      onImage = true;
   }

   public void OnPointerExit(PointerEventData eventData)
   {
      onImage = false;
   }
   // Luke Smith, Kenny
   public void OnPointerUp(PointerEventData eventData) {
      // Calc the amount of turrents relative to mass of 
      // Place turret if touching player
      if (turretToSpawn != null && turretToSpawn.GetComponent<AutoTurretController>().isTouching && 
         TurretManager.instance.canPlaceTurret(turretToSpawn.gameObject)) {
         AutoTurretController newTurret = turretToSpawn.GetComponent<AutoTurretController> ();
         newTurret.AttachToPlayer();

         GameControl.instance.SetScoreText();
         GameControl.instance.SetMassText();

         isDragged = false;
         turretToSpawn = null;
      }

      // Otherwise, remove turret
      if (turretToSpawn != null && (!turretToSpawn.GetComponent<AutoTurretController>().isTouching ||
            !TurretManager.instance.canPlaceTurret(turretToSpawn.gameObject))) {
         RemoveTurret ();
      }
   }

   public void OnPointerDown(PointerEventData eventData) {
      if (PlayerController.instance.mass - turretCost < 0) {
         return;
      }

      if (onImage && !isDragged) {
         isDragged = true;
         GetTurret ();
      }
   }

   public void GetTurret() {
      turretToSpawn = dynamicPool.GetPooledObject(turretType);
      if (turretToSpawn != null) {
         turretToSpawn.SetActive(true);
         turretToSpawn.GetComponent<AutoTurretController> ().Reset();
      }
   }

   public void RemoveTurret() {
      if (turretToSpawn != null) {
         turretToSpawn.SetActive (false);
         isDragged = false;
         turretToSpawn = null;
      }
   }
}
