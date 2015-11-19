using UnityEngine;
using System.Collections;

// A melee weapon the player can use.
public class FlySwatter : MonoBehaviour {

	// The rigidbody of the swatter.
	Rigidbody body;
	// The maximum amount of time the swatter can exist.
	const int timeLimit = 18;
	// The time that the swatter has existed.
	int timer = 0;
	// The initial local scale of the swatter.
	Vector3 initScale;
	// Whether the swatter has hit something.
	bool hit;
	// Whether hit will be set to true next frame.
	bool willHit = false;
	// Whether the swatter will be destroyed after its animation finishes.
	[HideInInspector]
	public bool willDestroy = false;

	// Move the fly swatter to its beginning position.
	public void Initiate () {
		timer = 0;
		willHit = false;
		hit = false;
		transform.localPosition = Vector3.forward * 0.5f;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		initScale = transform.localScale;
		transform.parent.GetComponent<Player> ().canMove = false;
		gameObject.SetActive (true);
	}
	
	// Update is called once per frame.
	void Update () {
		if (willHit) {
			hit = true;
		}
		if (!hit) {
			float changeAngle = transform.localEulerAngles.x + 10;
			float angleSin = Mathf.Sin (changeAngle * Mathf.Deg2Rad);
			transform.localEulerAngles = PathUtil.SetX (transform.localEulerAngles, changeAngle);
			transform.localScale = PathUtil.SetY (transform.localScale, initScale.y + angleSin / 1);
			if (changeAngle == 100) {
				willHit = true;
			}
		}
		if (timer++ > timeLimit) {
			transform.parent.GetComponent<Player> ().canMove = true;
			if (willDestroy) {
				Destroy (gameObject);
			} else {
				gameObject.SetActive (false);
			}
		}
	}

	// Triggers events when colliding with certain objects.
	void OnTriggerEnter (Collider collider) {
		if (!collider.isTrigger && !hit) {
			Enemy enemy = collider.GetComponent<Enemy> ();
			if (enemy != null) {
				enemy.KillEntity ();
			}
			hit = true;
		}
	}
}
