using UnityEngine;
using System.Collections;

// Attempts to float above and squash the player.
public class Book : MonoBehaviour, IMovement {

	// The initial position of the book.
	Vector3 startPosition;

	// The player the book is targeting.
	Player target;
	// The book's rigidbody.
	Rigidbody body;

	// Whether the book is currently chasing the player.
	bool targetingPlayer = false;
	// The current movement stage of the book.
	Stage stage = Stage.Idle;

	// The height of the book when on the ground.
	float groundedHeight;
	// The height that the book will rise to.
	float targetHeight;
	// The position that the book will move towards.
	Vector3 targetPosition;
	// The delay after the book falls before it begins moving again.
	int fallDelay = 0;

	// Stages in the book's movement.
	enum Stage {Idle, Rise, Chase, Fall};

	// Use this for initialization.
	void Start () {
		startPosition = transform.position;
		body = GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame.
	void Update () {
		if (stage == Stage.Idle && targetingPlayer) {
			stage = Stage.Rise;
			groundedHeight = transform.position.y;
			body.useGravity = false;
			targetHeight = target.GetComponent<Collider> ().bounds.extents.y * 4;
		} else if (stage == Stage.Rise) {
			PathUtil.SetY (transform, transform.position.y + 0.005f);
			if (transform.position.y - groundedHeight > targetHeight) {
				targetPosition = target.transform.position;
				stage = Stage.Chase;
			}
		} else if (stage == Stage.Chase) {
			PathUtil.MoveTowardsXZ (transform, targetPosition, 0.005f);
			if (PathUtil.GetMagnitudeXZ (transform.position - targetPosition) < Mathf.Epsilon) {
				stage = Stage.Fall;
				body.useGravity = true;
			}
		} else if (stage == Stage.Fall) {
			if (Mathf.Abs (body.velocity.y) < Mathf.Epsilon && fallDelay++ > 20) {
				fallDelay = 0;
				body.useGravity = false;
				stage = Stage.Idle;
			}
		}
	}

	// Kills the player instantly upon collision.
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			target = collider.GetComponent<Player> ();
			targetingPlayer = true;
		}
	}

	// Kills the player instantly upon collision.
	void OnTriggerExit (Collider collider) {
		if (collider.tag == "Player") {
			targetingPlayer = false;
		}
	}

	// Resets the book.
	public void Reset () {
		transform.position = startPosition;
		stage = Stage.Idle;
		targetingPlayer = false;
		fallDelay = 0;
	}
}
