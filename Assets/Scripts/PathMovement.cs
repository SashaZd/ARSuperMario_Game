using UnityEngine;
using System;

// Controls movement along the ribbon path.
public class PathMovement : MonoBehaviour {

	// The part of the path where the object begins the level on.
	public PathComponent startPath;
	// Where the object begins the level at.
	Vector3 startPosition;
	// Whether the object has been placed on the ribbon path.
	bool initiated = false;

	// The part of the level that the object is currently on. 
	public PathComponent currentPath;
	// How far along the path the object is.
	public float pathProgress = 0;

	// The speed that the object moves at per tick.
	public float moveSpeed = 0.01f;
	// The rigid body controlling the object's physics.
	Rigidbody body;
	// The distance from the center of the object to the ground while grounded.
	float groundOffset;
	// The distance from the center of the object to its side.
	float sideOffset;
	// The number of places on the object to check for side collisions.
	const int NUMSIDECHECKS = 30;
	// Extra distance to count as a side collision.
	const float COLLISIONOFFSET = 0.001f;
	// The layers to detect as collisions.
	public LayerMask collisionLayers;
	// Whether the object rotates at a constant speed.
	bool rotates = false;
	
	// Use this for initialization.
	void Start () {
		body = GetComponent<Rigidbody> ();
		if (startPath != null) {
			ResetPosition ();
		}
		UpdateGroundOffset ();
		sideOffset = GetComponent<Collider> ().bounds.extents.z;
		transform.eulerAngles = Vector3.up * Vector3.Angle(Vector3.right, currentPath.GetDirection (true));
		rotates = GetComponent<Rotate> () != null;
	}

	// Update is called once per frame.
	void Update () {
		if (!initiated && startPath != null) {
			ResetPosition ();
		}
	}

	// Finds the offset from the entity's center to the bottom of the entity.
	public void UpdateGroundOffset () {
		groundOffset = GetComponent<Collider> ().bounds.extents.y;
	}

	// Gets the offset between the center of the player and the ground while grounded.
	public float GetGroundOffset () {
		return groundOffset;
	}

	// Gets the offset between the center of the player and the side of the player.
	public float GetSideOffset () {
		return sideOffset;
	}

	// Resets the position of the object to its initial position.
	public void ResetPosition () {
		currentPath = startPath;
		pathProgress = 0;
		if (body == null) {
			body = GetComponent<Rigidbody> ();
		}
		body.velocity = Vector3.zero;

		// Determine the starting position and save it in case the player resets.
		if (!initiated) {
			RaycastHit hit;
			startPosition = startPath.GetPositionInPath (pathProgress);
			startPosition.y = PathUtil.ceilingHeight;
			Physics.Raycast (startPosition, Vector3.down, out hit, PathUtil.ceilingHeight * 1.1f);
			startPosition.y = hit.point.y + groundOffset + 0.25f;
			initiated = true;
		}
		transform.position = startPosition;
		transform.eulerAngles = Vector3.up * Vector3.Angle(Vector3.right, currentPath.GetDirection (true));
	}

	// Moves the object forward along the ribbon path.
	// Returns whether the object was able to move.
	public bool MoveAlongPath () {
		return MoveAlongPath (true);
	}

	// Moves the object along the ribbon path.
	// Returns whether the object was able to move.
	public bool MoveAlongPath (bool forward) {
		float moveDistance = moveSpeed;

		// Check for side collision.
		Vector3 sidePosition = transform.position + Vector3.down * (groundOffset - COLLISIONOFFSET);
		float sideIncrement = groundOffset * 2 / NUMSIDECHECKS + 2 * COLLISIONOFFSET;
		for (int i = 0; i < NUMSIDECHECKS; i++) {
			RaycastHit hit;
			if (Physics.Raycast (sidePosition, currentPath.GetDirection (forward), out hit, sideOffset + moveSpeed + COLLISIONOFFSET, collisionLayers)) {
				moveDistance = Mathf.Min (moveDistance, hit.distance - sideOffset - COLLISIONOFFSET);
				if (moveDistance < Mathf.Epsilon) {
					return false;
				}
			}
			sidePosition.y += sideIncrement;
		}

		pathProgress = currentPath.IncrementPathProgress (pathProgress, moveDistance, forward);

		// Check if the object has moved past the path's bounds.
		// Switch to the next/previous path if so.
		while (pathProgress > 1) {
			if (currentPath.nextPath == null) {
				pathProgress = 1;
				return false;
			} else {
				float overflow = currentPath.GetMagnitudeFromProgress(pathProgress - 1);
				currentPath = currentPath.nextPath;
				pathProgress = currentPath.GetProgressFromMagnitude(overflow);
			}
		}
		while (pathProgress < 0) {
			if (currentPath.previousPath == null) {
				pathProgress = 0;
				return false;
			} else {
				float underflow = currentPath.GetMagnitudeFromProgress(-pathProgress);
				currentPath = currentPath.previousPath;
				pathProgress = currentPath.GetProgressFromMagnitude(currentPath.GetMagnitude() - underflow);
			}
		}

		// Move the object.
		PathUtil.SetXZ (transform, currentPath.GetPositionInPath (pathProgress));
		// Rotate the object to face forward.
		if (!rotates) {
			Vector3 direction = currentPath.GetDirection (forward);
			float angle = Mathf.Atan2 (direction.x, direction.z) * 180 / Mathf.PI;
			Vector3 facing = Vector3.up * angle;
			transform.eulerAngles = facing;
		}
		return true;
	}
}
