using UnityEngine;
using System;

// Makes the player grow in size and gain an extra hit.
public class Mushroom : Item {

	// The movement speed of the mushroom.
	public float moveSpeed = 0.005f;
	// The pathing system for the mushroom.
	PathMovement movement;
	
	// Use this for initialization.
	void Start () {
		movement = GetComponent<PathMovement> ();
		PathUtil.FindClosestPath (transform.position, movement);
	}

	// Makes the mushroom start moving after emerging from a question block.
	public override void EmergeFromBlock() {
		movement.moveSpeed = moveSpeed;
	}

	// Powers up the player upon collision.
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Player") {
			collision.gameObject.GetComponent<Player> ().HitMushroom ();
			Destroy (gameObject);
		}
	}
}
