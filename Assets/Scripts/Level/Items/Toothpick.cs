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

	// Whether to destroy the toothpick after it stops moving.
	[HideInInspector]
	public bool willDestroy = false;

	// Initializes the toothpick.
	protected override void Start () {
		body = GetComponent<Rigidbody> ();
		base.Start ();
	}

	// Sets up the toothpick just before it is thrown.
	public void Initiate () {
		body.velocity = direction * TRAVELSPEED;
		gameObject.SetActive (true);
		Update ();
	}
	
	// Makes the toothpick face the direction it is moving.
	void Update () {
		if (body.velocity != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation (body.velocity);
			transform.eulerAngles -= new Vector3 (-90, 0, 0);
		}
	}

	// Kills an enemy if the toothpick hits it.
	void OnCollisionEnter (Collision collision) {
		Enemy enemy = GetComponent<Collider>().GetComponent<Enemy> ();
		if (enemy != null) {
			enemy.KillEntity ();
		}
		if (willDestroy) {
			Destroy (gameObject);
		} else {
			gameObject.SetActive (false);
		}
	}
}
