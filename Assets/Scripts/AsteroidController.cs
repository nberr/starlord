using UnityEngine;
/*
 * @author: Luke Smith, Joshua King
 * @methods: OnEnable, OnTriggerExit2D, Update
 * @since: 10.1.17 
 */
public class AsteroidController : MonoBehaviour {
   private Rigidbody2D rb2d;

   public Vector2 originPoint = Vector2.zero;
   public float maxRange = 40f;
   public float randomOffset;
   public int speed;
	public float pullForce = 3f;

   void OnEnable()
   {
      rb2d = GetComponent<Rigidbody2D>();
      Vector2 target = MouseControl.GetWorldPositionOnPlane(new Vector2(0, 0), 0f);
      Vector2 current = transform.position;
      Vector2 vectorToOrigin = Vector2.MoveTowards(-current, target, 3 * Time.deltaTime) * speed;
      vectorToOrigin.x += Random.Range(25, randomOffset);
      vectorToOrigin.y += Random.Range(25, randomOffset);

      rb2d.velocity = Vector2.zero;
      rb2d.AddForce(vectorToOrigin);
   }

   //Joshua King
   void OnTriggerExit2D(Collider2D other)
   {
      if (other.gameObject.CompareTag("Beam") && other.gameObject.GetComponent<Renderer>().enabled == true)
      {
         rb2d.velocity -= new Vector2(2f, 2f);
         if (rb2d.velocity.x< 0f && rb2d.velocity.y< 0f)
         {
            rb2d.velocity = rb2d.velocity.normalized;//Vector2.zero;
         }
      }
   }

   void Update()
   {
      if (Vector2.Distance(originPoint, transform.position) >= maxRange)
      {
         gameObject.SetActive(false);
      }
   }

   //Joshua King
	void OnTriggerStay2D (Collider2D other){
	   if (other.gameObject.CompareTag ("Beam") && other.gameObject.GetComponent<Renderer> ().enabled == true) {
			Vector2 target = MouseControl.GetWorldPositionOnPlane(new Vector2(0, 0), 0f);
			Vector2 current = transform.position;
			Vector2 vectorToOrigin = Vector2.MoveTowards(-current, target, 3 * Time.deltaTime) * pullForce;
			rb2d.AddForce(vectorToOrigin);
		}
	}
}
