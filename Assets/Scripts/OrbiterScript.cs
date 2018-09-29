using UnityEngine;

/*
 * @author Luke Smith
 * @since 10.25.17
 * 
 */
public class OrbiterScript : EnemyController
{
   private GameObject attracter; // Set this to the gameobject that this gameobject will be attracted to 
   public float gravityConstant; // Affects strength of gravity
   
   private Rigidbody2D rb;
   public int radius = 25;
   private bool orbit = false;
   private float attracterMass;
   private int maxAngle = 10;
   public float reloadTime = .1f; //seconds
   private float timeSinceFiring = .1f; //seconds
   public GameObject bullet;
   public float speed = 25.0f;
   private bool inBeam = false;

   // Use this for initialization
   void Start()
   {
      base.Start();
      this.OnEnable(); 
   }

   public override void Update(){}
   // Luke Smith
   // FixedUpdate is called once per physics update
   void FixedUpdate()
   {
      if (!orbit)
         transform.up = -Vector2.MoveTowards(transform.position, attracter.transform.position, 1);
      if (Vector2.Distance(attracter.transform.position, transform.position) <= radius && !orbit)
      {
         Vector2 target = new Vector2(0, 0);
         Vector2 direction = new Vector2(target.x - transform.position.x, target.y - transform.position.y);
         direction.Normalize();

         Vector2 cross = new Vector2(direction.y, -direction.x);
         rb.velocity = (direction * 7) + (cross * (4 + PlayerController.instance.mass/30));
         orbit = true;
      }
      if (orbit)
      {
         //https://answers.unity.com/questions/1393567/how-can-i-make-game-object-to-orbit-another-using-2.html
         float distance = Vector2.Distance(this.transform.position, attracter.transform.position); // Distance between us and attracter
         Vector2 unrotatedForce = (Vector2.right * gravityConstant * attracterMass) / Mathf.Pow(distance, 2); // Magnitude of force due to gravity

         // Now we have to rotate that force so it's pointing towards the attracter
         Vector2 posDifference = attracter.transform.position - this.transform.position; // Difference in position
         float angleDifference = Mathf.Atan2(posDifference.y, posDifference.x); // Now, difference in angle

         // Now we use some trig to rotate the force vector from pointing right to pointing at the attracting object
         Vector2 rotatedForce = new Vector2(unrotatedForce.x * Mathf.Cos(angleDifference) - unrotatedForce.y * Mathf.Sin(angleDifference),
                 unrotatedForce.x * Mathf.Sin(angleDifference) + unrotatedForce.y * Mathf.Cos(angleDifference));

         // And now we simply add the force to our rigidbody
         rb.AddForce(rotatedForce);
         
         transform.up = rb.velocity.normalized;
      }
      if (orbit && IsLookingAtObject(transform, attracter.transform.position, 70 + (PlayerController.instance.mass/10)))
      {
         FireBullet(new Vector2(0, 0));
         if (timeSinceFiring < reloadTime)
         {
            timeSinceFiring += Time.deltaTime;
         }
      }

      //Joshua King
      if (inBeam) {
         Vector2 target = MouseControl.GetWorldPositionOnPlane (new Vector2 (0, 0), 0f);
         Vector2 current = transform.position;
         Vector2 vectorToOrigin = Vector2.MoveTowards (3f * -current, 3f * target, 3 * Time.deltaTime);
         rb.AddForce (vectorToOrigin);
      }
   }
   //Luke Smith
   void OnEnable()
   {
      attracter = GameObject.FindGameObjectWithTag("Player");
      rb = this.GetComponent<Rigidbody2D>();
      attracterMass = 50 + (PlayerController.instance.mass);//attracter.GetComponent<Rigidbody2D>().mass;
      orbit = false;
      radius = 25 + (PlayerController.instance.mass / 150);
   }
   // Joshua King
   void OnTriggerEnter2D(Collider2D other)
   {
      // If an enemy collides with the player, the player loses score
      if (other.gameObject.CompareTag ("Player")) {
         this.gameObject.SetActive (false);
      } else if (other.gameObject.CompareTag ("Beam") && other.gameObject.GetComponent<Renderer> ().enabled == true) {
         inBeam = true;
      } else {
         inBeam = false;
      }
   }
   // Luke Smith
   //https://answers.unity.com/questions/503934/chow-to-check-if-an-object-is-facing-another.html
   bool IsLookingAtObject(Transform looker, Vector3 targetPos, float FOVAngle)
   {
      // FOVAngle has to be less than 180
      float checkAngle = Mathf.Min(FOVAngle, 359.9999f) / 2; // divide by 2 isn't necessary, just a bit easier to understand when looking at the angles.

      float dot = Vector3.Dot(looker.up, (targetPos - looker.position).normalized); // credit to fafase for this

      float viewAngle = (1 - dot) * 90; // convert the dot product value into a 180 degree representation (or *180 if you don't divide by 2 earlier)

      if (viewAngle <= checkAngle)
         return true;
      else
         return false;
   }
   // Luke Smith
   protected void FireBullet(Vector2 direction)
   {
      if (timeSinceFiring >= reloadTime)
      {
         timeSinceFiring = 0f;
         Vector2 newDir = -Vector2.MoveTowards(transform.position, direction, Time.deltaTime);

         GameObject clone;
         Rigidbody2D cloneRb2d;
         //Get instance of Bullet
         clone = dynamicPool.GetPooledObject(bullet);
         //if (clone == null) continue; //for graceful error
         clone.SetActive(true);
         cloneRb2d = clone.GetComponent<Rigidbody2D>();

         cloneRb2d.transform.position = transform.position;
         cloneRb2d.velocity = Vector2.zero;
         cloneRb2d.transform.up = (-transform.position).normalized;
         cloneRb2d.transform.gameObject.GetComponent<BulletController>().isEnemyBullet = true;
         // Send bullet on its errand of destruction
         cloneRb2d.AddForce(newDir * speed);
      }
   }
}