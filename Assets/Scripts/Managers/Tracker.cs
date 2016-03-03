using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// Tracks player and entity movement.
/// </summary>
public class Tracker : MonoBehaviour {

	/// <summary> The singleton tracker instance. </summary>
	private static Tracker instance;
	/// <summary> The singleton tracker instance. </summary>
	public static Tracker Instance {
		get {
			if (instance == null) {
				instance = GameObject.FindObjectOfType<Tracker>();
			}
			return instance;
		}
	}

	/// <summary> The level manager in the scene. </summary>
	private LevelManager manager;

	/// <summary> The name of the file to write to. </summary>
	[Tooltip("The name of the file to write to.")]
	public string filePath = "Assets/Tracking/";
	/// <summary> The number of frames to wait before recording a state. </summary>
	[Tooltip("The number of frames to wait before recording a state.")]
	public int interval;
	/// <summary> Timer for keeping track of state recording. </summary>
	private int intervalTimer;

	/// <summary> The log file. </summary>
	private StreamWriter file;

	/// <summary> Whether player logging is enabled. </summary>
	[Tooltip("Whether player logging is enabled.")]
	public bool loggingEnabled = true;

	/// <summary>
	/// Sets the tracker instance.
	/// </summary>
	private void Awake() {
		instance = this;
	}

	/// <summary>
	/// Gets the level manager instance.
	/// </summary>
	private void Start() {
		manager = LevelManager.Instance;
	}

	/// <summary>
	/// Periodically logs the positions of entities.
	/// </summary>
	private void Update () {
		intervalTimer++;
		if (intervalTimer > interval) {
			intervalTimer = 0;
			float time = Time.time;
			if (manager.player != null) {
				logEntity(manager.player, time);
				foreach (Enemy enemy in manager.enemies) {
					if (enemy != null && enemy.gameObject.activeSelf) {
						logEntity(enemy, time);
					}
				}
				foreach (Item item in manager.items) {
					if (item != null && item.gameObject.activeSelf) {
						logEntity(item, time);
					}
				}
			}
		}
	}

	/// <summary>
	/// Writes an entity's state to the log.
	/// </summary>
	/// <param name="entity">The entity to log.</param></param>
	/// <param name="time">The current game time.</param>
	private void logEntity(MonoBehaviour entity, float time) {
		string name = entity.name.Replace("(Clone)", "");
		Vector3 position = entity.transform.position;
		WriteToFile(time + " " + name + " " + position);
	}

	/// <summary>
	/// Writes an action to the log.
	/// </summary>
	/// <param name="action">The action to log.</param>
	public void logAction(string action) {
		WriteToFile(Time.time + " " + action);
	}

	/// <summary>
	/// Creates a new log file and writes the level JSON data to it.
	/// </summary>
	/// <param name="text">The level JSON data to log.</param>
	public void logJSON(string text) {
		if (!loggingEnabled) {
			return;
		}

		if (file != null) {
			file.Close();
		} else {
			System.IO.Directory.CreateDirectory(filePath);
		}
		string date = DateTime.Now.ToString().Replace(":", "").Replace("/", "").Replace(" ", "_");

		file = File.CreateText(filePath + "log_" + date);
		WriteToFile(text);
	}

	/// <summary>
	/// Writes text to the current log file.
	/// </summary>
	/// <param name="text">The text to log.</param>
	private void WriteToFile (String text) {
		if (loggingEnabled && file != null) {
			file.WriteLine(text);
		}
	}

	/// <summary>
	/// Saves the log if the user quits the game.
	/// </summary>
	void OnApplicationQuit() {
		if (file != null) {
			file.Close();
		}
	}
}