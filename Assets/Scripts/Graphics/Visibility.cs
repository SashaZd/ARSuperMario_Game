using UnityEngine;

/// <summary>
/// Handles platform visibility settings.
/// </summary>
public class Visibility : MonoBehaviour {

	/// <summary> The mesh to toggle visibility with. </summary>
	[SerializeField]
	[Tooltip("The mesh to toggle visibility with.")]
	private GameObject mesh;
	/// <summary> Whether or not the environment mesh is visible. </summary>
	private bool meshVisible;
	/// <summary> Timer to prevent input from occurring too fast. </summary>
	private int keyTimer;

	/// <summary>
	/// Checks for input toggling visibility settings.
	/// </summary>
	private void Update() {
		if (--keyTimer < 0 && Input.GetKey(KeyCode.Tab)) {
			keyTimer = 10;
			meshVisible = !meshVisible;
			Tracker.Instance.logAction("Visibility toggled: " + meshVisible);
			foreach (Transform platform in transform.FindChild("Platforms")) {
				if (platform.name == "Collider") {
					platform.GetComponent<Renderer>().enabled = !meshVisible;
				}
			}
			if (mesh != null) {
				mesh.SetActive(meshVisible);
			}
		}
	}
}
