using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Joshua King

//Unused but functional
public class ShieldPickup : MonoBehaviour {

	public GameObject Shield;

	public float thrust = 1f;

	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			gameObject.SetActive (false);
			GameObject newShield = Instantiate (Shield, new Vector3 (0, 0, 1), Quaternion.identity);
			newShield.transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			int count = 100;
			while (count > 0) {
				newShield.transform.localScale += new Vector3 (1f, 1f, 1f) * (Time.deltaTime);
				count--;
			}
			count = 100;
		}
	}

	void OnTriggerStay2D (Collider2D other){
		if (other.gameObject.CompareTag ("Beam") && other.gameObject.GetComponent<Renderer> ().enabled == true) {
			Vector2 target = new Vector2 (0, 0);
			Vector2 current = transform.position;
			Vector2 vectorToOrigin = Vector2.MoveTowards (-current, target, 3 * Time.deltaTime) * thrust;
			rb2d.AddForce (vectorToOrigin);
		}
	}
}
