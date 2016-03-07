using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Event listeners for the main menu.
/// </summary>
public class MainMenuUI : MonoBehaviour {

	/// <summary> The name of the scene that the scanning button will take the user to. </summary>
	[Tooltip("The name of the scene that the scanning button will take the user to.")]
	public string startScene = "";

	/// <summary> The input field for the user's username. </summary>
	[Tooltip("The input field for the user's username.")]
	public InputField usernameField;
	/// <summary> Data about the user of the application. </summary>
	[Tooltip("Data about the user of the application.")]
	public UserData userData;

	/// <summary>
	/// Takes the user to the level.
	/// </summary>
	public void StartScan() {
		userData.username = usernameField.text;
		if (userData.username != "") {
			SceneManager.LoadScene(startScene);
		}
	}
}
