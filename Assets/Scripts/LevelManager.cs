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
	
	// All enemies in the level.
	public Enemy[] enemies;
	// All coins in the level.
	public Coin[] coins;
	// All blocks in the level.
	public Block[] blocks;

	// Resets the positions of all entities in the level.
	public void ResetLevel () {
		player.Reset ();
		foreach (Enemy enemy in enemies) {
			enemy.Reset ();
		}
		foreach (Coin coin in coins) {
			coin.Reset ();
		}
		foreach (Block block in blocks) {
			block.Reset ();
		}
		foreach (Transform item in transform.FindChild ("Items").transform) {
			item.gameObject.GetComponent<Item> ().Reset ();
		}
	}
}
