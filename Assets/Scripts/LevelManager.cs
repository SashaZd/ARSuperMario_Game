using UnityEngine;
using System.Collections.Generic;

// Holds all entities in the level.
public class LevelManager : MonoBehaviour {

	// Singleton object.
	private static LevelManager instance;
	// The audio source playing the background music.
	AudioSource music;

	// Sets the level manager instance.
	void Awake () {
		instance = this;
	}

	// Use this for initialization.
	void Start () {
		music = GetComponent<AudioSource> ();
	}

	// Update is called once per frame.
	void Update () {
		// Handles looping of background music.
		if (music.time >= 77.068817f) {
			music.time = 9.118005f;
			music.Play ();
		}
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
	public List<Coin> coins;
	// All blocks in the level.
	public List<Block> blocks;

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
