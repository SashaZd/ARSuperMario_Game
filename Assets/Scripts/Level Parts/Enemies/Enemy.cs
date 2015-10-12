using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	
	// Update is called once per frame.
	void Update () {
		if (PathUtil.OnFloor (gameObject)) {
			KillEntity ();
		}
	}
	
	// Resets the position and direction of the enemy.
	public void Reset () {
		gameObject.SetActive (true);
		IMovement movement = GetComponent(typeof(IMovement)) as IMovement;
		movement.Reset ();
	}
	
	// Kills the enemy.
	public void KillEntity () {
		gameObject.SetActive (false);
	}
}
