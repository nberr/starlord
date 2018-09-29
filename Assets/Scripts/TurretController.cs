using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/*
 * @author: Luke Smith, Ken Oshima
 * @since: 9.26.17
 * 
 * This class is the parent class to all Types of turrets.
 */
 
public class TurretController : Destructable {
   public GameObject player;
   public GameObject bullet;
   
   public float speed = 100.0f;
   public float maxRange = 10f;
   public float minRange = 0f;
   public float reloadTime = 2.0f; //seconds
   public int numBullets = 1;
   public float spread = 0.0f; //inaccuracy

   protected CircleCollider2D collider;
   protected float initMaxRange;
   protected float initArcScale;
   protected float arcScale;
   protected GameObject firingArc;

   protected float timeSinceFiring = 2.0f; //seconds


   private bool selected = false;

   // Use this for initialization
   public virtual void Start () {
      base.Start();
      
      collider = GetComponent<CircleCollider2D> ();
      firingArc = transform.Find("TurretFiringArc").gameObject;

      bullet.gameObject.GetComponent<BulletController>().maxRange = maxRange;
      bullet.gameObject.GetComponent<BulletController>().minRange = minRange;

      speed = bullet.gameObject.GetComponent<BulletController>().speed;
      
      arcScale = (maxRange * 2f) / firingArc.GetComponent<SpriteRenderer>().bounds.size.x;
      initArcScale = arcScale;

      firingArc.transform.localScale = new Vector3(arcScale, arcScale, 1);

      isInvincible = true;
   }

   // Update is called once per frame
   public virtual void Update () {
      if (timeSinceFiring < reloadTime) {
         timeSinceFiring += Time.deltaTime;
      }
   }
      
   // Luke Smith, John Bradbury
   protected void FireBullet(Vector2 direction) {
      if (timeSinceFiring >= reloadTime) {
         timeSinceFiring = 0f;
         transform.up = direction;
         
         float theta = -spread / 2;
         float incTheta = numBullets > 1 ? spread / (numBullets - 1) : 0f;
         if (numBullets == 1) {
            theta = 0f;
         }
         
         GameObject clone;
         Rigidbody2D cloneRb2d;
         BulletController bulletC;
         for (int i = 0; i < numBullets; ++i) {
            //Get instance of Bullet
            clone = dynamicPool.GetPooledObject(bullet);
            //if (clone == null) continue; //for graceful error
            clone.SetActive(true);

            cloneRb2d = clone.GetComponent<Rigidbody2D>();

            //clone.GetComponent<BulletController> ().originPoint = transform.position;
            //cloneRb2d.transform.position = transform.position;
            clone.GetComponent<BulletController>().maxRange = maxRange;

            bulletC = clone.GetComponent<BulletController>();
            
            bulletC.ResetBullet(transform.position, transform.up, theta, false);
            cloneRb2d.transform.position = transform.position;
            cloneRb2d.velocity = Vector2.zero;
            cloneRb2d.transform.up = transform.up;
            cloneRb2d.transform.Rotate(0, 0, theta);
            //cloneRb2d.velocity = Vector2.zero;
            // Send bullet on its errand of destruction
            cloneRb2d.AddForce(clone.transform.up * speed);
            GetComponent<AudioSource> ().Play ();
            theta += incTheta;
         }
      }
   }

   void OnTriggerEnter2D(Collider2D other) {
      if (other.gameObject.CompareTag("Enemy") && !isInvincible) {
         Destructable otherScript = null;
         GameObject parent = other.gameObject;
         while (true) {
            if (parent.GetComponent<Destructable>()) {
               otherScript = parent.GetComponent<Destructable>();
               break;
            }
            
            parent = parent.transform.parent.gameObject;
         }
         
         otherScript.InflictDamage(collisionDamage);
         InflictDamage(otherScript.GetCollisionDamage());
      }
      
      if (other.gameObject.CompareTag ("Asteroid")) {
         if (this is AutoTurretController && !(this is OrbitingTurretController) && ((AutoTurretController)this).isAttached) {
            PlayerController.instance.GetComponent<PlayerController>().addMass (10);

            other.gameObject.SetActive (false);
         }
      }
   }
}
