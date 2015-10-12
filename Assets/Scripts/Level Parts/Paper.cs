using UnityEngine;
using System.Collections;

// An enemy that will kill the player on contact.
// Landing on it from above will kill it instead.
public class Paper : Enemy {

	// Update is called once per frame.
	void Update () {
		transform.Rotate (new Vector3 (0f, 8f, 0f));
	}

	// Fires when the goomba hits the player.
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Player") {
			// Check if the player is high enough to flatten the enemy.
			float playerStompHeight = collision.collider.transform.position.y - collision.collider.bounds.extents.y / 2;
			float enemyHeadHeight = transform.position.y + GetComponent<Collider> ().bounds.extents.y * 0.45f;
			if (playerStompHeight > enemyHeadHeight || collision.collider.transform.GetComponent<Rigidbody> ().velocity.y < -0.01f) {
				collision.gameObject.GetComponent<Player> ().StompEnemy ();
				KillEntity ();
			} else {
				// If not, the player dies and the level is reset.
				collision.gameObject.GetComponent<Player> ().TakeDamage ();
			}
		}
	}
}
