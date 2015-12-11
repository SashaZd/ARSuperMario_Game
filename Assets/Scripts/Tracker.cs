using UnityEngine;
using System.Collections.Generic;
using System.IO;

// Tracks player and entity movement.
public class Tracker : MonoBehaviour {

	// The state of an entity at a point in time.
	struct EntityState {
		// The name of the entity.
		string name;
		// The position of the entity.
		Vector3 position;
		// The time this position was recorded.
		float time;

		// Initializes a state.
		public EntityState(MonoBehaviour entity, float time) {
			this.name = entity.name.Replace("(Clone)", "");
			this.position = entity.transform.position;
			this.time = time;
		}

		// Returns a string representation of the state.
		public override string ToString() {
			return time + " " + name + " " + position;
		}
	}

	// Archive of entity states.
	List<EntityState> states = new List<EntityState> ();

	// The level manager in the scene.
	LevelManager manager;

	// The name of the file to write to.
	public string fileName = "Assets/Temp/Tracking.txt";
	// The number of frames to wait before recording a state.
	public int interval;
	// Timer for keeping track of state recording.
	int intervalTimer;

	// Use this for initialization.
	void Start () {
		manager = LevelManager.GetInstance ();
	}
	
	// Update is called once per frame.
	void Update () {
		intervalTimer++;
		if (intervalTimer > interval) {
			intervalTimer = 0;
			float time = Time.time;
			if (manager.player != null) {
				states.Add (new EntityState (manager.player, time));
				foreach (Enemy enemy in manager.enemies) {
					if (enemy != null && enemy.gameObject.activeSelf) {
						states.Add (new EntityState (enemy, time));
					}
				}
				foreach (Item item in manager.items) {
					if (item != null && item.gameObject.activeSelf) {
						states.Add (new EntityState (item, time));
					}
				}
			}
		}
	}

	// Writes the tracked data to a file and resets the data.
	public void Reset () {
		/*
		if (states.Count > 10) {
			StreamWriter logFile = File.CreateText (fileName);
			foreach (EntityState state in states) {
				logFile.Write (state.ToString () + "\n");
			}
			logFile.Close ();
		}
		*/
		states.Clear ();
	}
}
