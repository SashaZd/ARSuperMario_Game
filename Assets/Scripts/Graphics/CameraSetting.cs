using UnityEngine;

/// <summary>
/// Handles switching the camera between first-person and third-person.
/// </summary>
public class CameraSetting : MonoBehaviour {

	/// <summary> The first-person camera in the scene. </summary>
	[SerializeField]
	[Tooltip("The first-person camera in the scene.")]
	private Camera firstPersonCamera;
	/// <summary> The third-person camera in the scene. </summary>
	[SerializeField]
	[Tooltip("The third-person camera in the scene.")]
	private Camera thirdPersonCamera;

	/// <summary> Whether the camera is in first-person mode. </summary>
	private bool isFirstPerson;
	/// <summary> Timer to prevent input from occurring too fast. </summary>
	private int keyTimer;

	/// <summary>
	/// Initializes the cameras according to the initial setting.
	/// </summary>
	private void Start() {
		SetCameras();
	}

	/// <summary>
	/// Watches for camera change input.
	/// </summary>
	private void Update() {
		if (--keyTimer < 0 && Input.GetKey(KeyCode.C)) {
			keyTimer = 8;
			isFirstPerson = !isFirstPerson;
			SetCameras();
		}
	}

	/// <summary>
	/// Sets which camera is active.
	/// </summary>
	private void SetCameras() {
		firstPersonCamera.gameObject.SetActive(isFirstPerson);
		thirdPersonCamera.gameObject.SetActive(!isFirstPerson);
	}
}
