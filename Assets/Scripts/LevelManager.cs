using UnityEngine;
using System.Collections;

// Holds all entities in the level.
public class LevelManager : MonoBehaviour {

	// Singleton object.
	private static LevelManager instance;

	// Sets the level manager instance.
	void Awake () {
		instance = this;
	}

	// Gets the instance of the level manager.
	public static LevelManager GetInstance () {
		if (instance == null) {
			instance = GameObject.FindObjectOfType<LevelManager>();
		}
		return instance;
	}

	// The object that the player controls.
	public Player player;
	// All segments in the ribbon path.
	public PathComponent[] fullPath;
	// All enemies in the level;
	public BasicEnemy[] enemies;

	// Resets the positions of all entities in the level.
	public void ResetLevel () {
		player.GetComponent<PathMovement> ().ResetPosition ();
		foreach (BasicEnemy enemy in enemies) {
			enemy.ResetPosition ();
		}
	}
}
