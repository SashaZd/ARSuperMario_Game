using UnityEngine;
using System.Collections;

// A projectile that the player can throw.
public class Toothpick : Item {

	// The rigidbody of the toothpick.
	Rigidbody body;

	// The initial direction the toothpick will travel in.
	[HideInInspector]
	public Vector3 direction;
	// The speed that the toothpick moves at.
	const float TRAVELSPEED = 4f;

	// Use this for initialization.
	protected override void Start () {
		body = GetComponent<Rigidbody> ();
		body.velocity = direction * TRAVELSPEED;
		base.Start ();
		Update ();
	}
	
	// Update is called once per frame.
	void Update () {
		if (body.velocity != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation (body.velocity);
			transform.eulerAngles -= new Vector3 (-90, 0, 0);
		}
	}

	// Triggers events when colliding with certain objects.
	void OnCollisionEnter (Collision collision) {
		Enemy enemy = GetComponent<Collider>().GetComponent<Enemy> ();
		if (enemy != null) {
			enemy.KillEntity ();
		}
		Destroy (gameObject);
	}
}
