using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Joshua King

public class TractorBeam : MonoBehaviour {
	private Rigidbody2D rb2d;
   private Renderer sprite;

	public float thrust = 50f;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D> ();
      sprite = GetComponent<Renderer> ();
		sprite.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 target = MouseControl.GetWorldPositionOnPlane(Input.mousePosition, 0f);
		Vector2 turret = transform.position;
		Vector2 direction = new Vector2(target.x - turret.x, target.y - turret.y);
		direction.Normalize();
		transform.up = direction;


		if (Input.GetMouseButtonDown (1)) {
         sprite.enabled = true;
         GetComponent<AudioSource> ().Play ();
         GetComponent<AudioSource> ().loop = true;
		} 
		else if (Input.GetMouseButtonUp (1)){
         sprite.enabled = false;
         GetComponent<AudioSource> ().loop = false;
		}

	}

}
