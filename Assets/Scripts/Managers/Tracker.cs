using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;

// Tracks player and entity movement.
public class Tracker : MonoBehaviour {

	// The singleton tracker instance.
	private static Tracker instance;

	// The level manager in the scene.
	LevelManager manager;

	// The name of the file to write to.
	public string filePath = "Assets/Tracking/";
	// The number of frames to wait before recording a state.
	public int interval;
	// Timer for keeping track of state recording.
	int intervalTimer;

	// The log file.
	StreamWriter file;

	// Whether player logging is enabled.
	public bool loggingEnabled = true;

	// Sets the tracker instance.
	void Awake () {
		instance = this;
	}

	// Use this for initialization.
	void Start () {
		manager = LevelManager.GetInstance ();
	}
	
	// Gets the instance of the tracker.
	public static Tracker GetInstance () {
		if (instance == null) {
			instance = GameObject.FindObjectOfType<Tracker>();
		}
		return instance;
	}
	
	// Update is called once per frame.
	void Update () {
		intervalTimer++;
		if (intervalTimer > interval) {
			intervalTimer = 0;
			float time = Time.time;
			if (manager.player != null) {
				logEntity (manager.player, time);
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

	// Writes an entity's state to the log.
	void logEntity(MonoBehaviour entity, float time) {
			string name = entity.name.Replace ("(Clone)", "");
			Vector3 position = entity.transform.position;
			WriteToFile (time + " " + name + " " + position);
	}

	// Writes an action to the log.
	public void logAction(string action) {
		WriteToFile (Time.time + " " + action);
	}

	// Creates a new log file and writes the level JSON data to it.
	public void logJSON(string text) {
		if (!loggingEnabled) {
			return;
		}

		if (file != null) {
			file.Close ();
		} else {
			System.IO.Directory.CreateDirectory(filePath);
		}
		string date = DateTime.Now.ToString ().Replace (":", "").Replace ("/", "").Replace (" ", "_");

		file = File.CreateText (filePath + "log_" + date);
		WriteToFile (text);
	}

	// Writes text to the current log file.
	private void WriteToFile (String text) {
		if (loggingEnabled && file != null) {
			file.WriteLine (text);
		}
	}

	// Saves the log if the user quits the game.
	void OnApplicationQuit() {
		if (file != null) {
			file.Close ();
		}
	}
}