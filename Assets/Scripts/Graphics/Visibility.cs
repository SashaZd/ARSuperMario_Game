using UnityEngine;

/// <summary>
/// Handles visibility settings.
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

	/// <summary> Possible visibility settings. </summary>
	private enum Settings {Mesh = 1, Collider, Both};
	/// <summary> The current visibility setting. </summary>
	private Settings currentSetting = Settings.Both;

	/// <summary>
	/// Checks for input toggling visibility settings.
	/// </summary>
	private void Update() {
		if (--keyTimer < 0 && Input.GetKey(KeyCode.Tab)) {
			keyTimer = 2;
			ChangeSetting();
			meshVisible = ((int)currentSetting & 1) > 0;
			Tracker.Instance.logAction("Visibility toggled: " + meshVisible);
			foreach (Transform platform in transform.FindChild("Platforms")) {
				if (platform.name == "Collider") {
					platform.GetComponent<Renderer>().enabled = ((int)currentSetting & 2) > 0;
				}
			}
			if (mesh != null) {
				mesh.SetActive(meshVisible);
			}
		}
	}

	/// <summary>
	/// Changes the visibility setting to the next one.
	/// </summary>
	private void ChangeSetting() {
		int newSetting = (int)currentSetting + 1;
		if (newSetting > 3) {
			newSetting = 1;
		}
		currentSetting = (Settings)newSetting;
	}
}
