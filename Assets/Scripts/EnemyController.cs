using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * @author Nicholas Berriochoa
 * @since 9.26.17
 * 
 * v1 - basic mover for enemy (may rename or move code somewhere else)
 * 
 */
public class EnemyController : Destructable {
   private Rigidbody2D rb2d;

   private Vector3 saveScale;
   
   void Awake() {
      saveScale = transform.localScale;
   }
   
   void Start() {
      base.Start();

      rb2d = GetComponent<Rigidbody2D>();
   }
	
	void FixedUpdate () {

	}
   // Luke Smith
	public virtual void Update() {
      transform.up = (-transform.position).normalized;
	}

   //Joshua King
   void OnTriggerStay2D (Collider2D other){
      if (other.gameObject.CompareTag ("Beam") && other.gameObject.GetComponent<Renderer> ().enabled == true) {
         Vector2 target = MouseControl.GetWorldPositionOnPlane(new Vector2(0, 0), 0f);
         Vector2 current = transform.position;
         Vector2 vectorAwayFromOrigin = Vector2.MoveTowards(current, -target, 3 * Time.deltaTime) * 0.05f;
         rb2d.AddForce(vectorAwayFromOrigin);
      }
   }
   
   public void Reset() {
      base.Reset();
      transform.localScale = saveScale;
   }
}
