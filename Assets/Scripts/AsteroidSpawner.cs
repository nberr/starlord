using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author: Luke Smith
 * @since: 10.1.17
 * 
 * This class controls random generation of astroids in game to increase player size and hp
 */
public class AsteroidSpawner : MonoBehaviour
{
   
   public GameObject poolGameObject;
   private DynamicObjectPool dynamicPool;
   public GameObject asteroidObject;
   public int count = 0;
   public int poolSize = 1;
   public float radius = 30f;

   //spawn every n seconds
   public float spawnRate = 2f;
   private float nextSpawn = 0.0f;

   void Start()
   {
      dynamicPool = (DynamicObjectPool)poolGameObject.GetComponent(typeof(DynamicObjectPool));
   }

   void Update()
   {
      //time to spawn an enemy
      if (Time.time > nextSpawn && !GameControl.instance.gameIsPaused)
      {
         float randX, randY;
         Vector2 spawnPos;

         nextSpawn = Time.time + spawnRate;

         //get random x and y within a certain range
         float theta = Random.Range(0f, 360f);
         float r = radius; //rand later
         float radians = theta * Mathf.Deg2Rad;
         spawnPos = new Vector2(r * Mathf.Sin(radians), r * Mathf.Cos(radians));

         GameObject tmp = dynamicPool.GetPooledObject(asteroidObject);
         tmp.transform.position = spawnPos;
         tmp.SetActive(true);
      }
   }
}
