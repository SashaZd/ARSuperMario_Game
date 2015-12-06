using UnityEngine;
using System.Collections.Generic;

// Holds all entities in the level.
public class LevelManager : MonoBehaviour {

	// Singleton object.
	private static LevelManager instance;
	// Whether player tracking is enabled.
	public bool enableTracking = true;
	// The scale multiplier for the level's size.
	public float scaleMultiplier = 1;

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
	public List<PathComponent> fullPath;
	
	// All enemies in the level.
	public List<Enemy> enemies;
	// All coins in the level.
	public List<Item> items;
	// All blocks in the level.
	public List<Block> blocks;

	// Whether the items list is currently being iterated through.
	public bool itemIterating = false;

	// Resets the positions of all entities in the level.
	public void ResetLevel () {
		player.Reset ();
		foreach (Enemy enemy in enemies) {
			enemy.Reset ();
		}
		foreach (Block block in blocks) {
			block.Reset ();
		}
		itemIterating = true;
		foreach (Item item in items) {
			item.Reset ();
		}
		itemIterating = false;
		for (int i = 0; i < items.Count; i++) {
			Item item = items[i];
			if (item.willRemove) {
				items.RemoveAt (i);
				i--;
			}
		}
		Tracker tracker = GetComponent<Tracker> ();
		if (tracker != null) {
			tracker.Reset ();
		}
	}
}
