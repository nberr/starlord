using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//John Bradbury
public class TeleportingEnemy : EnemyController {
   
   public GameObject solidImage;
   public GameObject ghostImage;
   public GameObject forceField; //when not teleporting
   public GameObject teleportEffect; //when teleporting
   
   public GameObject spawnedObject;
   public float spawnRate;
   public float teleportRate;
   public float teleportCountdown; //how long does it take to teleport
   public float closeToRange;
   
   private float nextSpawn = 0.0f;
   private float nextTeleport = 0.0f;
   private Vector2 telePos;
   private float currentTeleCountdown = 0.0f;
   // Use this for initialization
   void Start () {
      base.Start();
      
      //teleport into battle
      nextTeleport = teleportRate;
      Teleport();
   }
   
   // Update is called once per frame
   void Update () {
      //teleport to some random location in range
      
      if (!Teleporting()) {
         if (!Teleport()) {
            LaunchEnemy();
         }
      }
   }
   
   //Choose a destination, start teleportation countdown and ghosting
   bool Teleport() {
      if (nextTeleport >= teleportRate) {
         float randX, randY;
         
         float theta = Random.Range(0f, 360f);
         float r = closeToRange + PlayerController.instance.getPlayerRadius();
         float radians = theta * Mathf.Deg2Rad; 
         telePos = new Vector2(r * Mathf.Sin(radians), r * Mathf.Cos(radians));
         
         //place ghost there
         ghostImage.transform.position = telePos;
         ghostImage.transform.up = (Vector3.zero - ghostImage.transform.position).normalized;
         ghostImage.SetActive(true);
         
         nextTeleport = 0.0f;
         currentTeleCountdown = teleportCountdown;
         
         //remove the force field
         isInvincible = false;
         forceField.SetActive(false);
         teleportEffect.SetActive(true);
         return true;
      }
      nextTeleport += Time.deltaTime;
      return false;
   }
   
   bool Teleporting() {
      if (currentTeleCountdown > 0.0f) {
         currentTeleCountdown -= Time.deltaTime;
         if (currentTeleCountdown <= 0.0f) {
            currentTeleCountdown = 0.0f;
            //finish teleporting
            transform.position = telePos;
            solidImage.transform.up = (Vector3.zero - transform.position).normalized;
            ghostImage.SetActive(false);
            
            //set up the force field (no damage)
            isInvincible = true;
            forceField.SetActive(true);
            teleportEffect.SetActive(false);
         }
         return true;
      }
      return false;
   }
   
   void LaunchEnemy() {
      if (Time.time > nextSpawn) {
         nextSpawn = Time.time + spawnRate;
         
         Vector2 spawnPos = new Vector2(transform.position.x, transform.position.y);
         
         //spawn enemy
         GameObject tmp = dynamicPool.GetPooledObject(spawnedObject);
         tmp.SetActive(true);
         tmp.transform.position = spawnPos;
         Vector2 direction = Vector2.zero - spawnPos;
         tmp.GetComponent<EnemyController>().Reset();
         tmp.GetComponent<Rigidbody2D>().AddForce(direction.normalized * 300f);
      }
   }
}
