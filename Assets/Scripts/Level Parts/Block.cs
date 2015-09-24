using UnityEngine;
using System.Collections;

// A block that has an effect when hit from underneath.
public class Block : MonoBehaviour {

	// The item inside the block.
	public GameObject contents;

	// Checks if a player hit the block from underneath.
	void OnCollisionEnter (Collision collision) {
		if (collision.collider.tag == "Player") {
			float playerHeadHeight = collision.transform.position.y + collision.collider.bounds.extents.y / 2;
			float blockBaseHeight = transform.position.y - GetComponent<Collider> ().bounds.extents.y * 0.55f;
			if (playerHeadHeight < blockBaseHeight) {
				HitBlock ();
			}
		}
	}

	// Triggers an effect when the player hits the block from underneath.
	public virtual void HitBlock () {
	}

	// Used if subclasses need to reset the block.
	public virtual void Reset () {
	}
}
