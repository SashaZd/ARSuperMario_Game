using UnityEngine;
using System.Collections;

// Handle platform visibility settings.
public class Visibility : MonoBehaviour {

	// The mesh to toggle visibility with.
	public GameObject mesh;
	// Whether or not the environment mesh is visible.
	bool meshVisible;
	// Timer to prevent input from occurring too fast.
	int keyTimer;
	
	// Update is called once per frame.
	void Update () {
		if (--keyTimer < 0 && Input.GetKey (KeyCode.Tab)) {
			keyTimer = 10;
			meshVisible = !meshVisible;
			Tracker.GetInstance ().logAction ("Visibility toggled: " + meshVisible);
			foreach (Transform platform in transform.FindChild ("Platforms")) {
				if (platform.name == "Collider") {
					platform.GetComponent<Renderer> ().enabled = !meshVisible;
				}
			}
			if (mesh != null) {
				mesh.SetActive (meshVisible);
			}
		}
	}
}
