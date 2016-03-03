using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Button listeners for the main menu.
/// </summary>
public class MainMenuUI : MonoBehaviour {

	/// <summary> The name of the scene that the scanning button will take the user to. </summary>
	[Tooltip("The name of the scene that the scanning button will take the user to.")]
	public string startScene = "";

	/// <summary>
	/// Should normally allow the user to start scanning the environment with the Kinect.
	/// Currently just takes the user directly to the first screen.
	/// </summary>
	public void StartScan() {
		SceneManager.LoadScene(startScene);
	}
}
