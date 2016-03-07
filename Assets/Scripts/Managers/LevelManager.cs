using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Holds all entities and paths in the level.
/// </summary>
public class LevelManager : MonoBehaviour {

	/// <summary> Singleton object. </summary>
	private static LevelManager instance;
	public static LevelManager Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<LevelManager>();
			}
			return instance;
		}
	}
	/// <summary> The scale multiplier for the level's size. </summary>
	public float scaleMultiplier = 1;

	/// <summary> The object that the player controls. </summary>
	[Tooltip("The object that the player controls.")]
	public Player player;
	/// <summary> All segments in the ribbon path. </summary>
	[Tooltip("All segments in the ribbon path.")]
	public List<PathComponent> fullPath;
	/// <summary> The currently rendered lines in the path. </summary>
	[HideInInspector]
	public PathRendererList pathRendererList;

	/// <summary> All enemies in the level. </summary>
	[Tooltip("All enemies in the level.")]
	public List<Enemy> enemies;
	/// <summary> All coins in the level. </summary>
	[Tooltip("All coins in the level.")]
	public List<Item> items;
	/// <summary> All blocks in the level. </summary>
	[Tooltip("All blocks in the level.")]
	public List<Block> blocks;

	/// <summary> Whether the items list is currently being iterated through. </summary>
	[HideInInspector]
	public bool itemIterating = false;

	/// <summary>
	/// Sets the level manager instance.
	/// </summary>
	private void Awake() {
		instance = this;
	}

	/// <summary>
	/// Resets the positions of all entities in the level.
	/// </summary>
	public void ResetLevel() {
		Tracker.Instance.logAction ("Reset level.");
		player.Reset();
		if (pathRendererList == null) {
			pathRendererList = new PathRendererList(fullPath[0]);
		} else {
			pathRendererList.Init(fullPath[0]);
		}
		foreach (Enemy enemy in enemies) {
			enemy.Reset();
		}
		foreach (Block block in blocks) {
			block.Reset();
		}
		itemIterating = true;
		foreach (Item item in items) {
			item.Reset();
		}
		itemIterating = false;
		for (int i = 0; i < items.Count; i++) {
			Item item = items[i];
			if (item.willRemove) {
				items.RemoveAt(i);
				i--;
			}
		}
	}
}
