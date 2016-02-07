using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Button listeners for the main menu.
public class MainMenu : MonoBehaviour {

	// The name of the scene that the scanning button will take the user to.
	public string startScene = "";

	// Should normally allow the user to start scanning the environment with the Kinect.
	// Currently just takes the user directly to the first screen.
	public void StartScan() {
		SceneManager.LoadScene (startScene);
	}
}
