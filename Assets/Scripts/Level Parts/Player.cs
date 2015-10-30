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
	// Whether the player can move around.
	public bool canMove;

	// The initial velocity of the player's jump.
	public float jumpSpeed = 2.5f;
	// Threshold for limiting jump height.
	public int baseMaxJumpTimer = 6;
	// Timer for varying the player's jump height.
	int jumpHeightTimer = 0;
	// Whether the player is on the ground and can jump.
	bool isGrounded;

	// The amount of frames that the player will be invincible after being hit.
	const int INVINCIBLEDELAY = 120;
	// Timer for the player being invincible.
	int invincibleTimer = 0;

	// Ticks after the player has reached the goal.
	int goalTick;
	// The number of ticks to wait before resetting the level after winning.
	const int MAXGOALTICKS = 60;

	// The current score of the player.
	public int score = 0;
	// The current power-up of the player.
	Power power = Power.None;
	// The current scale of the player.
	int size = 1;

	// The current power-up the player has.
	public enum Power {None, Mushroom, Coffee};

	// Use this for initialization.
	void Start () {
		canMove = true;
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

			if (canMove) {
				if (forward ^ backward) {
					pathMovement.MoveAlongPath (forward);
				}

				UpdateRun (run && (forward ^ backward));

				Jump (jump);
			}

			UpdateInvincible ();

			// Terminal velocity
			if (-body.velocity.y > jumpSpeed) {
				body.velocity = PathUtil.SetY (body.velocity, -jumpSpeed);
			}

			if (reset) {
				LevelManager.GetInstance ().ResetLevel ();
			}
			if (PathUtil.OnFloor (gameObject)) {
				KillPlayer ();
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

	// Marks the player as grounded.
	public void OnTriggerEnter (Collider collider) {
		if (isGround (collider.tag)) {
			isGrounded = true;
		}
	}

	// Marks the player as airborne.
	public void OnTriggerExit (Collider collider) {
		if (isGround (collider.tag)) {
			isGrounded = false;
		}
	}

	// Checks whether the collided object's tag is ground or not.
	bool isGround (string tag) {
		return tag != "Enemy" && tag != "Item";
	}

	// Handles the player jumping.
	void Jump (bool isJumping) {
		// Varies the player's jump height.
		if (isJumping && jumpHeightTimer < baseMaxJumpTimer * pathMovement.moveSpeed / baseMoveSpeed) {
			if (isGrounded) {
				jumpHeightTimer++;
			}
			if (jumpHeightTimer > 0) {
				body.velocity = PathUtil.SetY (body.velocity, jumpSpeed);
				jumpHeightTimer++;
			}
		} else {
			jumpHeightTimer = 0;
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

	// Handles invincibility frames after being hit) {
	void UpdateInvincible () {
		if (invincibleTimer > 0) {
			invincibleTimer--;
			if (invincibleTimer > INVINCIBLEDELAY / 2) {
				GetComponent<Renderer> ().enabled = invincibleTimer / 20 % 2 == 1;
			} else {
				GetComponent<Renderer> ().enabled = invincibleTimer / 10 % 2 == 0;
			}
		}
	}

	// Causes the player to bounce after stomping on an enemy.
	// Also increases the player's score.
	public void StompEnemy () {
		Vector3 setVelocity = new Vector3 (body.velocity.x, jumpSpeed, body.velocity.z);
		body.velocity = setVelocity;
		jumpHeightTimer++;
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
		if (invincibleTimer == 0) {
			if (power == Power.None) {
				KillPlayer ();
			} else {
				LosePower ();
				invincibleTimer = INVINCIBLEDELAY;
			}
		}
	}

	// Kills the player and resets the level.
	public void KillPlayer () {
		LevelManager.GetInstance ().ResetLevel ();
	}

	// Sets the player's current power-up.
	public void SetPower (Power newPower) {
		if (power == newPower) {
			return;
		}
		if (power != Power.None) {
			LosePower ();
		}
		switch (newPower) {
		case Power.Mushroom: 
			SetSize (2);
			break;
		case Power.Coffee:
			baseMoveSpeed *= 2;
			runSpeed *= 2;
			break;
		default:
			break;
		}
		power = newPower;
	}

	// Causes the player to lose the current power-up
	public void LosePower () {
		if (power == Power.None) {
			KillPlayer ();
		} else {
			switch (power) {
			case Power.Mushroom:
				SetSize (1);
				break;
			case Power.Coffee: 
				baseMoveSpeed /= 2;
				runSpeed /= 2;
				break;
			default:
				break;
			}
			power = Power.None;
		}
	}

	// Returns true if the player has won the level.
	public bool HasWon () {
		return goalTick > 0;
	}

	// Resets the position of the player.
	public void Reset () {
		pathMovement.ResetPosition ();
		score = 0;
		if (power != Power.None) {
			LosePower ();
		}
		invincibleTimer = 0;
		GetComponent<Renderer> ().enabled = true;
		ToothpickPower toothpickPower = GetComponent<ToothpickPower> ();
		if (toothpickPower) {
			GameObject.Destroy (toothpickPower);
		}
		FlySwatterPower swatterPower = GetComponent<FlySwatterPower> ();
		if (swatterPower) {
			GameObject.Destroy (swatterPower);
		}
		Transform swatter = transform.FindChild ("FlySwatter(Clone)");
		if (swatter) {
			Destroy (swatter.gameObject);
		}
		canMove = true;
	}
}
