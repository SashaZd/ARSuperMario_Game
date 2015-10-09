using UnityEngine;
using System;

// Responds to keypresses to control the character.
public class Player : MonoBehaviour {

	// Controls the player's movement along the ribbon path.
	PathMovement pathMovement;
	// The rigid body controlling the object's physics.
	Rigidbody body;
	
	// The normal walking speed of the player.
	public float baseMoveSpeed = 0.01f;
	// The maximum running speed of the player.
	public float runSpeed;
	// The amount that speed is incremented every tick when acclerating to run speed.
	float speedIncrement;

	// The initial velocity of the player's jump.
	public float jumpSpeed = 2.5f;
	// Timer for varying the player's jump height.
	int jumpTimer = 0;
	// Threshold for limiting jump height.
	public int baseMaxJumpTimer = 6;

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

		pathMovement.moveSpeed = baseMoveSpeed;
		if (runSpeed < baseMoveSpeed) {
			runSpeed = baseMoveSpeed * 1.5f;
		}
		speedIncrement = (runSpeed - baseMoveSpeed) / 30f;
	}

	// Update is called once per frame.
	void Update () {
		if (goalTick == 0) {
			// Get player input.
			bool forward = Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D);
			bool backward = Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A);
			bool jump = Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W);
			bool reset = Input.GetKey (KeyCode.R);
			bool run = Input.GetKey (KeyCode.H);

			if (forward ^ backward) {
				pathMovement.MoveAlongPath (forward);
			}

			UpdateRun (run && (forward ^ backward));

			Jump (jump);

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

	// Handles the player jumping.
	void Jump (bool isJumping) {
		// Varies the player's jump height.
		if (isJumping && jumpTimer < baseMaxJumpTimer * pathMovement.moveSpeed / baseMoveSpeed) {
			// Check if the player is on the ground.
			if ((Physics.Raycast (transform.position + Vector3.right * pathMovement.GetSideOffset (), Vector3.down, pathMovement.GetGroundOffset () + 0.001f) ||
				Physics.Raycast (transform.position + Vector3.left * pathMovement.GetSideOffset (), Vector3.down, pathMovement.GetGroundOffset () + 0.001f) ||
				Physics.Raycast (transform.position + Vector3.back * pathMovement.GetSideOffset (), Vector3.down, pathMovement.GetGroundOffset () + 0.001f) ||
				Physics.Raycast (transform.position + Vector3.forward * pathMovement.GetSideOffset (), Vector3.down, pathMovement.GetGroundOffset () + 0.001f) ||
				Physics.Raycast (transform.position, Vector3.down, pathMovement.GetGroundOffset () + 0.001f)) &&
				Math.Abs (body.velocity.y) < 0.001) {
				jumpTimer++;
			}
			if (jumpTimer > 0) {
				body.velocity = PathUtil.SetY (body.velocity, jumpSpeed);
				jumpTimer++;
			}
		} else {
			jumpTimer = 0;
		}
	}

	// Speeds up the player if running. Reverts back to normal speed if not.
	void UpdateRun (bool isRunning) {
		if (isRunning) {
			if (pathMovement.moveSpeed < runSpeed) {
				pathMovement.moveSpeed += speedIncrement;
			}
		} else {
			if (pathMovement.moveSpeed > baseMoveSpeed) {
				pathMovement.moveSpeed -= speedIncrement;
			}
		}
	}

	// Causes the player to bounce after stomping on an enemy.
	// Also increases the player's score.
	public void StompEnemy () {
		Vector3 setVelocity = new Vector3 (body.velocity.x, jumpSpeed, body.velocity.z);
		body.velocity = setVelocity;
		jumpTimer++;
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
		body.velocity = Vector3.up * jumpSpeed;
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
