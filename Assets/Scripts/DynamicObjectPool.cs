using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
/*
 * @author Luke Smith
 * @date 10.9.17
 */
public class DynamicObjectPool : MonoBehaviour
{
   private List<List<GameObject>> pool;

   public void Start()
   {
      pool = new List<List<GameObject>>();
   }
   // Luke Smith
   public List<GameObject> GetPoolList(GameObject listObjectType)
   {
      for (int index = 0; index < pool.Count; index++)
      {
         if (listObjectType.CompareTag(pool[index][0].tag))
         {
            return pool[index];
         }
      }
      return null;
   }
   // Luke Smith
   public GameObject GetPooledObject(GameObject objectToGet)
   {
      int listIndex = -1;
      for(int index = 0; index < pool.Count; index++)
      {
         if (objectToGet.CompareTag(pool[index][0].tag))
         {
            listIndex = index;
         }
      }
      if (listIndex == -1)
      {
         pool.Add(new List<GameObject>());
         listIndex = pool.Count - 1;
      }
      for (int i = 0; i < pool[listIndex].Count; i++)
      {
         if (!pool[listIndex][i].activeInHierarchy)
         {
            return pool[listIndex][i];
         }
      }
      GameObject obj = (GameObject)Instantiate(objectToGet, transform);
      obj.SetActive(true);
      pool[listIndex].Add(obj);
      //reset obj
      obj.transform.position = Vector3.zero;
      
      return obj;
   }

   //Nicholas Berriochoa
   //used to check if any enemies are in the scene
   public int ActiveCount(GameObject objectToGet) {
      List<GameObject> tmp = GetPoolList (objectToGet);
      if (tmp == null) return 0;
      int count = 0;
      for (int i = 0; i < tmp.Count; i++) {
         if (tmp [i].activeSelf) {
            count++;
         }
      }
      return count;
   }

   //Nicholas Berriochoa
   //clears all enemies of a certain type
   public void ClearEnemies(GameObject enemy) {
      List<GameObject> tmp = GetPoolList (enemy);

      if (tmp == null)
         return;

      for (int i = 0; i < tmp.Count; i++) {
         tmp [i].gameObject.SetActive (false);
      }
   }
}
