using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	// Whether the enemy can be destroyed.
	public bool invincible = false;
	
	// Update is called once per frame.
	void Update () {
		if (PathUtil.OnFloor (gameObject)) {
			KillEntity ();
		}
	}
	
	// Resets the position and direction of the enemy.
	public void Reset () {
		gameObject.SetActive (true);
		Movement movement = GetComponent(typeof(Movement)) as Movement;
		if (movement != null) {
			movement.Reset ();
		}
	}
	
	// Kills the enemy.
	public void KillEntity () {
		if (!invincible) {
			gameObject.SetActive (false);
		}
	}
}
