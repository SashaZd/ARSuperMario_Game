using UnityEngine;
using System;

// Makes the player grow in size and gain an extra hit.
public class Mushroom : Item {
	
	// Use this for initialization.
	void Start () {
		PathUtil.FindClosestPath (transform.position, GetComponent<PathMovement> ());
	}

	// Powers up the player upon collision.
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Player") {
			collision.gameObject.GetComponent<Player> ().HitMushroom ();
			Destroy (gameObject);
		}
	}
}
