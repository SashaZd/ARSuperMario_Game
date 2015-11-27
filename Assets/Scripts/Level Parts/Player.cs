using UnityEngine;
using System;
using System.Collections.Generic;

// Responds to keypresses to control the character.
public class Player : MonoBehaviour {

	// Controls the player's movement along the ribbon path.
	PathMovement pathMovement;
	// The rigid body controlling the object's physics.
	Rigidbody body;
	// Renderer for displaying the player.
	Renderer playerRenderer;
	
	// The normal walking speed of the player.
	public float baseMoveSpeed = 0.01f;
	// The maximum running speed of the player.
	public float runSpeed;
	// The amount that speed is incremented every tick when acclerating to run speed.
	float speedIncrement;
	// Whether the player can move around.
	[HideInInspector]
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

	// Whether the player is dead and in a dying animation.
	bool dead = false;

	// The current score of the player.
	public int score = 0;
	// The current power-ups of the player.
	List<Power> powers = new List<Power> ();
	// The number of power-ups the player has lost in this tick.
	int lostPowers = 0;
	// The current scale of the player.
	int size = 1;

	// Use this for initialization.
	void Start () {
		pathMovement = GetComponent<PathMovement> ();
		body = GetComponent<Rigidbody> ();
		playerRenderer = GetComponent<Renderer> ();

		canMove = true;

		pathMovement.moveSpeed = baseMoveSpeed;
		if (runSpeed < baseMoveSpeed) {
			runSpeed = baseMoveSpeed * 1.5f;
		}
		speedIncrement = (runSpeed - baseMoveSpeed) / 30f;
	}

	// Update is called once per frame.
	void FixedUpdate () {
		if (goalTick > 0) {
			// Wait for the win animation before resetting the level.
			if (goalTick > MAXGOALTICKS) {
				goalTick = 0;
				LevelManager.GetInstance ().ResetLevel ();
			} else {
				goalTick++;
			}
		} else if (dead) {
			Color playerColor = playerRenderer.material.color;
			playerColor.a -= 0.05f;
			playerRenderer.material.color = playerColor;
			body.constraints = body.constraints | RigidbodyConstraints.FreezePositionY;
			if (playerColor.a < Mathf.Epsilon) {
				LevelManager.GetInstance ().ResetLevel ();
			}
		} else {
			// Get player input.
			bool forward = Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D);
			bool backward = Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A);
			bool jump = Input.GetKey (KeyCode.Space) || Input.GetKey (KeyCode.UpArrow) || Input.GetKey (KeyCode.W);
			bool reset = Input.GetKey (KeyCode.R);
			bool run = Input.GetKey (KeyCode.H);
			bool powerKey = Input.GetKey (KeyCode.Return);

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

			lostPowers = 0;
			if (powerKey) {
				for (int i = 0; i < powers.Count; i++) {
					Power power = powers[i];
					power.PowerKey ();
					if (lostPowers > 0) {
						i -= lostPowers;
						lostPowers = 0;
					}
				}
			}

			if (reset) {
				LevelManager.GetInstance ().ResetLevel ();
			}

			if (PathUtil.OnFloor (gameObject)) {
				KillPlayer ();
			}
		}
	}

	// Checks when the player collides with certain objects.
	public void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Finish") {
			HitGoal ();
		} else if (isGround (collision.collider.tag)) {
			isGrounded = true;
		}
	}

	// Marks the player as airborne.
	public void OnCollisionExit (Collision collision) {
		if (isGround (collision.collider.tag)) {
			isGrounded = false;
		}
	}

	// Checks whether the collided object's tag is ground or not.
	bool isGround (string tag) {
		return tag != "Enemy" && tag != "Item" && tag != "Weapon";
	}

	// Handles the player jumping.
	void Jump (bool isJumping) {
		// Varies the player's jump height based on movement speed and held jump button.
		if (isJumping && jumpHeightTimer < baseMaxJumpTimer * pathMovement.moveSpeed / baseMoveSpeed) {
			bool incremented = false;
			if (isGrounded) {
				jumpHeightTimer++;
			}
			if (jumpHeightTimer > 0 && body.velocity.y > -0.01f) {
				body.velocity = PathUtil.SetY (body.velocity, jumpSpeed);
				if (!incremented) {
					jumpHeightTimer++;
				}
			} else {
				jumpHeightTimer = 0;
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
				playerRenderer.enabled = invincibleTimer / 20 % 2 == 1;
			} else {
				playerRenderer.enabled = invincibleTimer / 10 % 2 == 0;
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
			if (powers.Count == 0) {
				KillPlayer ();
			} else {
				int removeIndex = RandomUtil.RandomInt (0, powers.Count);
				LosePower (powers[removeIndex]);
				invincibleTimer = INVINCIBLEDELAY;
			}
		}
	}

	// Kills the player and resets the level.
	public void KillPlayer () {
		dead = true;
	}

	// Adds a power-up to the player.
	public void AddPower (Power newPower) {
		powers.Add (newPower);
	}

	// Removes a power from the player.
	public void LosePower (Power power) {
		if (power != null) {
			power.OnRemove ();
			powers.Remove (power);
			lostPowers++;
			Destroy ((MonoBehaviour)power);
		}
	}

	// Returns true if the player has won the level.
	public bool HasWon () {
		return goalTick > 0;
	}

	// Resets the player when the level restarts.
	public void Reset () {
		pathMovement.ResetPosition ();
		body.useGravity = true;
		body.velocity = Vector3.zero;
		body.constraints = body.constraints & ~RigidbodyConstraints.FreezePositionY;
		isGrounded = false;
		score = 0;
		jumpHeightTimer = 0;
		foreach (Power power in powers) {
			power.OnReset ();
			Destroy ((MonoBehaviour)power);
		}
		powers.Clear ();
		invincibleTimer = 0;
		playerRenderer.enabled = true;
		canMove = true;
		dead = false;
		Color playerColor = playerRenderer.material.color;
		playerColor.a = 1;
		playerRenderer.material.color = playerColor;
	}
}
