using UnityEngine;
using System.Collections;

// Responds to keypresses to control the character.
public class Player : MonoBehaviour {

	// Controls the player's movement along the ribbon path.
	PathMovement pathMovement;
	// The rigid body controlling the object's physics.
	Rigidbody body;

	// Ticks after the player has reached the goal.
	int goalTick;
	// The number of ticks to wait before resetting the level after winning.
	const int MAXGOALTICKS = 60;

	// Use this for initialization.
	void Start () {
		pathMovement = GetComponent<PathMovement> ();
		body = GetComponent<Rigidbody> ();
	}

	// Update is called once per frame.
	void Update () {
		if (goalTick == 0) {
			// Get player input.
			bool forward = Input.GetKey ("right") || Input.GetKey ("d");
			bool backward = Input.GetKey ("left") || Input.GetKey ("a");
			bool jump = Input.GetKey ("space") || Input.GetKey ("up") || Input.GetKey ("w");
			bool reset = Input.GetKey ("r");

			if (forward ^ backward) {
				pathMovement.MoveAlongPath (forward);
			}

			if (jump) {
				pathMovement.jump ();
			}

			if (reset) {
				LevelManager.GetInstance ().ResetLevel ();
			}
		} else {
			// Wait for the win animation before resetting the level.
			if (goalTick > MAXGOALTICKS) {
				goalTick = 0;
				body.useGravity = true;
				LevelManager.GetInstance ().ResetLevel ();
			} else {
				goalTick++;
			}
		}
	}

	// Triggers events when colliding with certain objects.
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Finish") {
			OnGoalHit ();
		}
	}

	// Plays an animation upon reaching the goal.
	public void OnGoalHit () {
		body.useGravity = false;
		body.velocity = Vector3.up * pathMovement.jumpSpeed;
		goalTick = 1;
	}
}
