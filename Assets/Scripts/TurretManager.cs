using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * @author: Luke Smith, Ken Oshima
 */

public class TurretManager : MonoBehaviour {

   public static TurretManager instance;

   public GameObject poolGameObject;
   public GameObject autoObject;
   public GameObject multiObject;
   public GameObject laserObject;
   private Dictionary<int, int> turretLimitLevels;
   public Dictionary<string, int> turretCostLevels;

   private DynamicObjectPool dynamicPool;

   void Awake() {
      if (instance == null)
         instance = this;
      else if (instance != this)
         Destroy(gameObject);
   }
   //Luke Smith
	// Use this for initialization
	void Start () {
      //pooledTurrets = new List<TurretController> ();
      dynamicPool = (DynamicObjectPool)poolGameObject.GetComponent(typeof(DynamicObjectPool));
      instance = this;
      turretLimitLevels = new Dictionary<int, int>();
      turretLimitLevels.Add(0, 1);
      turretLimitLevels.Add(100, 3);
      turretLimitLevels.Add(200, 5);
      turretLimitLevels.Add(300, 10);
      turretCostLevels = new Dictionary<string, int>();
      turretCostLevels.Add("AutoTurret", 25);
      turretCostLevels.Add("LaserTurret", 75);
      turretCostLevels.Add("MultiTurret", 50);
      turretCostLevels.Add("OrbitalTurret", 100);
   }

   // Update is called once per frame
   void Update () {
		
	}

   public void SpawnTurret(GameObject turret) {

   }
   // Luke Smith
   public bool canPlaceTurret(GameObject turretToAdd)
   {
      if (GameControl.instance.godmode)
      {
         return true;
      }

      int count = 0;
      // Count all of the turrets on the player
      if (GameObject.FindGameObjectWithTag("AutoTurret") != null)
         count += dynamicPool.ActiveCount(GameObject.FindGameObjectWithTag("AutoTurret"));
      if (GameObject.FindGameObjectWithTag("LaserTurret") != null)
         count += dynamicPool.ActiveCount(GameObject.FindGameObjectWithTag("LaserTurret"));
      if (GameObject.FindGameObjectWithTag("MultiTurret") != null)
         count += dynamicPool.ActiveCount(GameObject.FindGameObjectWithTag("MultiTurret"));
      if (GameObject.FindGameObjectWithTag("OrbitalTurret") != null)
         count += dynamicPool.ActiveCount(GameObject.FindGameObjectWithTag("OrbitalTurret"));
      // find limit associated with mass - massLoss
      int massLoss = turretCostLevels[turretToAdd.tag];
      int endMass = PlayerController.instance.mass - massLoss;
      int key = endMass - (endMass % 100);
      if (key > 300)
         key = 300;
      if (turretLimitLevels.ContainsKey(key) && endMass >= 0)
      {
         int value = turretLimitLevels[key];
         if (count <= value && PlayerController.instance.reduceMass(massLoss))
         {
            GameControl.instance.SetMassText();
            // can be added to the pool and already reduced mass
            return true;
         }
      }
      return false;
   }

   public void SpawnTurret(string tag) {
      GameObject turretToSpawn;
      switch (tag) {
      case "AutoTurret":
         turretToSpawn = autoObject;
         break;
      case "LaserTurret":
         turretToSpawn = laserObject;
         break;
      case "MultiShotTurret":
         turretToSpawn = multiObject;
         break;
      default:
         return;
      }

      GameObject tmp = dynamicPool.GetPooledObject(turretToSpawn);
      tmp.SetActive(true);
      tmp.GetComponent<AutoTurretController> ().Reset ();

   }
}
