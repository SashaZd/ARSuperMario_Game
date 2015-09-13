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
			bool forward = Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D);
			bool backward = Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A);
			bool jump = Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W);
			bool reset = Input.GetKey (KeyCode.R);

			if (forward ^ backward) {
				pathMovement.MoveAlongPath (forward);
			}

			if (jump) {
				pathMovement.jump ();
			}

			if (reset || PathUtil.OnFloor (gameObject)) {
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

	// Causes the player to bounce after stomping on an enemy.
	public void stompEnemy () {
		Vector3 setVelocity = new Vector3 (body.velocity.x, pathMovement.jumpSpeed, body.velocity.z);
		body.velocity = setVelocity;
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
