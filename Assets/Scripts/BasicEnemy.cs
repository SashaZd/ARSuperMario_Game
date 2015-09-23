using UnityEngine;
using System.Collections;

// An enemy that will kill the player on contact.
public class BasicEnemy : MonoBehaviour {
	
	// Controls the enemy's movement along the ribbon path.
	PathMovement pathMovement;
	// Whether the enemy is moving forwards.
	bool forward = true;

	// Use this for initialization.
	void Start () {
		pathMovement = GetComponent<PathMovement> ();
	}
	
	// Update is called once per frame.
	void Update () {
		if (!pathMovement.MoveAlongPath (forward)) {
			forward = !forward;
		}
		if (PathUtil.OnFloor (gameObject)) {
			KillEntity ();
		}
	}

	// Resets the position and direction of the enemy.
	public void Reset () {
		gameObject.SetActive (true);
		forward = true;
		pathMovement.ResetPosition ();
	}

	// Fires when the enemy hits the player.
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Player") {
			// Check if the player is high enough to "Goomba stomp" the enemy.
			float playerStompHeight = collision.collider.transform.position.y - collision.collider.bounds.extents.y / 2;
			float enemyHeadHeight = transform.position.y + GetComponent<Collider> ().bounds.extents.y * 0.45f;
			if (playerStompHeight > enemyHeadHeight) {
				collision.gameObject.GetComponent<Player> ().StompEnemy ();
				KillEntity ();
			} else {
				// If not, the player dies and the level is reset.
				collision.gameObject.GetComponent<Player> ().HitEnemy ();
			}
		}
	}

	// Kills the enemy.
	void KillEntity () {
		gameObject.SetActive (false);
	}
}
