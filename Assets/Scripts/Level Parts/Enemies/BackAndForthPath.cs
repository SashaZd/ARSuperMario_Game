using UnityEngine;
using System.Collections;

// Moves along the path until hitting a barrier, then turns around.
public class BackAndForthPath : MonoBehaviour, Movement {

	// Controls the entity's movement along the ribbon path.
	PathMovement pathMovement;
	// Whether the entity is moving forwards.
	bool forward;
	// The direction the entity is moving at the start of the level.
	bool startDirection;
	
	// Use this for initialization.
	void Start () {
		startDirection = RandomUtil.RandomBoolean ();
		forward = startDirection;
		pathMovement = GetComponent<PathMovement> ();
	}
	
	// Update is called once per frame.
	void Update () {
		if (!pathMovement.MoveAlongPath (forward)) {
			forward = !forward;
		}
	}

	// Resets the entity's direction and position.
	public void Reset () {
		forward = startDirection;
		pathMovement.ResetPosition ();
	}
}
