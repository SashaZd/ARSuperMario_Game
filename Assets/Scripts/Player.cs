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

	// The current score of the player.
	public int score = 0;
	// The size (power-up status) of the player.
	public int size = 1;

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
	// Also increases the player's score.
	public void StompEnemy () {
		Vector3 setVelocity = new Vector3 (body.velocity.x, pathMovement.jumpSpeed, body.velocity.z);
		body.velocity = setVelocity;
		score += 100;
	}

	// Triggers events when colliding with certain objects.
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Finish") {
			HitGoal ();
		}
	}

	// Plays an animation upon reaching the goal.
	public void HitGoal () {
		body.useGravity = false;
		body.velocity = Vector3.up * pathMovement.jumpSpeed;
		goalTick = 1;
	}

	// Sets the size of the player.
	public void SetSize (int newSize) {
		float ratio = (float)newSize / (float)size;
		PathUtil.ScaleY (transform, ratio);
		PathUtil.SetY (transform, transform.position.y + GetComponent<Collider> ().bounds.extents.y * Mathf.Log (ratio, 2) * 2.01f);
		size = newSize;
		pathMovement.UpdateGroundOffset ();
	}

	// Reduces the player's size. Kills the player if the size is at a minimum.
	public void TakeDamage () {
		if (size > 1) {
			SetSize (1);
		} else {
			LevelManager.GetInstance ().ResetLevel ();
		}
	}

	// Increases the player's size.
	public void HitMushroom () {
		if (size == 1) {
			SetSize (2);
		}
	}

	// Increases the player's score after collecting a coin.
	public void CollectCoin () {
		score += 100;
	}

	// Resets the position of the player.
	public void Reset () {
		pathMovement.ResetPosition ();
		score = 0;
		SetSize (1);
	}
}
