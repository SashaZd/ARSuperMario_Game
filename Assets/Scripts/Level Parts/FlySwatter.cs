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

	// Use this for initialization.
	void Start () {
		initScale = transform.localScale;
		transform.parent.GetComponent<Player> ().canMove = false;
	}
	
	// Update is called once per frame.
	void Update () {
		if (!hit) {
			float changeAngle = transform.localEulerAngles.x + 10;
			float angleSin = Mathf.Sin (changeAngle * Mathf.Deg2Rad);
			transform.localEulerAngles = PathUtil.SetX (transform.localEulerAngles, changeAngle);
			transform.localScale = PathUtil.SetY (transform.localScale, initScale.y + angleSin / 1);
		}
		if (timer++ > timeLimit) {
			transform.parent.GetComponent<Player> ().canMove = true;
			Destroy (gameObject);
		} else if (timer > timeLimit / 2) {
			hit = true;
		}
	}

	// Triggers events when colliding with certain objects.
	void OnTriggerEnter (Collider collider) {
		if (!collider.isTrigger && !hit) {
			if (collider.GetComponent<Enemy> ()) {
				collider.gameObject.SetActive (false);
			}
			hit = true;
		}
	}
}
