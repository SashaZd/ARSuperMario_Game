using UnityEngine;
using System.Collections;

// Moves like BackAndForthPath, but takes a long time to make turns.
public class Pencil : MonoBehaviour, IMovement {
	
	// Controls the entity's movement along the ribbon path.
	PathMovement pathMovement;
	// Whether the entity is moving forwards.
	bool forward;
	// The direction the entity is moving at the start of the level.
	bool startDirection;

	// The rotation of the pencil in the previous frame.
	float prevRotation = -1;
	// The direction the pencil is turning.
	int turningMode = 0;
	// The rotation the pencil is turning towards.
	float targetRotation;
	// The initial rotation of the pencil when the level begins.
	float startRotation;

	// The speed at which the pencil rotates.
	public float turnSpeed = 3;
	
	// Use this for initialization.
	void Start () {
		startDirection = RandomUtil.RandomBoolean ();
		forward = startDirection;
		pathMovement = GetComponent<PathMovement> ();
	}
	
	// Update is called once per frame.
	void Update () {
		if (turningMode == 0) {
			if (!pathMovement.MoveAlongPath (forward)) {
				forward = !forward;
			}
			float difference = gameObject.transform.eulerAngles.y - prevRotation;
			if (Mathf.Abs (difference) > 0.05f) {
				if (prevRotation == -1) {
					prevRotation = startRotation = gameObject.transform.eulerAngles.y;
				} else {
					if (difference < 0) {
						difference += 360;
					}
					turningMode = difference <= 180 ? 1 : -1;
					targetRotation = gameObject.transform.eulerAngles.y;
					gameObject.transform.eulerAngles = PathUtil.SetY (gameObject.transform.eulerAngles, prevRotation);
				}
			}
		} else {
			float newY = gameObject.transform.eulerAngles.y + turningMode * turnSpeed;
			if ((turningMode > 0 && newY >= targetRotation ||
			    turningMode < 0 && newY <= targetRotation) &&
			    Mathf.Abs ((newY - targetRotation) % 360) <= turnSpeed) {
				turningMode = 0;
				newY = targetRotation;
				prevRotation = newY;
			}
			gameObject.transform.eulerAngles = PathUtil.SetY (gameObject.transform.eulerAngles, newY);
		}
	}
	
	// Resets the entity's direction and position.
	public void Reset () {
		forward = startDirection;
		pathMovement.ResetPosition ();
		gameObject.transform.eulerAngles = PathUtil.SetY (gameObject.transform.eulerAngles, startRotation);
		prevRotation = startRotation;
		turningMode = 0;
	}
}
