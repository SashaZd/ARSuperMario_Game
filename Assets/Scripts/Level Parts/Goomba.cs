﻿using UnityEngine;
using System.Collections;

// An enemy that will kill the player on contact.
// Landing on it from above will kill it instead.
public class Goomba : Enemy {

	// Fires when the goomba hits the player.
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Player") {
			// Check if the player is high enough to "Goomba stomp" the enemy.
			float playerStompHeight = collision.collider.transform.position.y - collision.collider.bounds.extents.y;
			float enemyHeadHeight = transform.position.y + GetComponent<Collider> ().bounds.extents.y * 0.95f;
			if (playerStompHeight > enemyHeadHeight) {
				collision.gameObject.GetComponent<Player> ().StompEnemy ();
				KillEntity ();
			} else {
				// If not, the player dies and the level is reset.
				collision.gameObject.GetComponent<Player> ().TakeDamage ();
			}
		}
	}
}
