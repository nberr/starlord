using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CenterTurretController : TurretController {

   public GameObject reloadIcon;

   public float initDmgMult;
   public float initReloadMult;
   public int initNumMult;

   // Use this for initialization
   new void Start () {
      base.Start();
      initMaxRange = maxRange;
      maxRange = maxRange * PlayerController.instance.transform.localScale.x;
      initDmgMult = collisionDamage;
      initReloadMult = reloadTime;
      initNumMult = numBullets;
   }
   
   // Update is called once per frame
   new void Update () {
      base.Update();
      
      Vector2 target = MouseControl.GetWorldPositionOnPlane(Input.mousePosition, 0f);
      Vector2 turret = transform.position;
      Vector2 direction = new Vector2(target.x - turret.x, target.y - turret.y);
      direction.Normalize();
      
      //make turret point towards mouse
      transform.up = direction;
      
      // Check if player clicks mouse in game
      if (Input.GetMouseButtonDown(0))
      {
         //if (EventSystem.current.IsPointerOverGameObject() == false)
         FireBullet(direction);
         //GetComponent<AudioSource> ().Play ();
      }
      
      if (timeSinceFiring >= reloadTime) {
         reloadIcon.SetActive(false);
      } else {
         reloadIcon.SetActive(true);
      }

      //float newMaxRange = initMaxRange * PlayerController.instance.transform.localScale.x;
      maxRange = Mathf.Max(initMaxRange * PlayerController.instance.transform.localScale.x, minRange);
      arcScale = Mathf.Max(initArcScale, initArcScale * PlayerController.instance.transform.localScale.x);
      firingArc.transform.localScale = new Vector3(arcScale, arcScale, 1);
      bullet.gameObject.GetComponent<BulletController>().maxRange = Mathf.Max(initMaxRange + PlayerController.instance.transform.localScale.x, minRange);
   }
}
