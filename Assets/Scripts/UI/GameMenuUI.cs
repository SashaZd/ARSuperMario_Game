using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

// Button listener for in-game pause menu.
public class GameMenuUI : MonoBehaviour {

	// The pause menu to open when the game is paused.
	GameObject pauseMenu;

	// Whether the game is paused.
	public static bool paused = false;
	// Timer for slowing down repeated pausing and unpausing.
	int pauseTimer = 0;
	// The delay between pausing and unpausing.
	const int PAUSEDELAY = 30;

	// Registers the pause menu canvas.
	void Start () {
		pauseMenu = transform.FindChild ("Canvas").gameObject;
	}

	// Listens for pausing and unpausing the game with the escape key.
	void Update () {
		if (pauseTimer > 0) {
			pauseTimer--;
		}
		if (pauseTimer <= 0 && Input.GetKey (KeyCode.Escape)) {
			if (paused) {
				Unpause ();
			} else {
				Pause ();
			}
			pauseTimer = PAUSEDELAY;
		}
	}

	// Pauses the game.
	public void Pause () {
		paused = true;
		pauseMenu.SetActive (true);
		Time.timeScale = 0;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	// Unpauses the game.
	public void Unpause () {
		paused = false;
		pauseMenu.SetActive (false);
		Time.timeScale = 1;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	// Returns the user to the main menu.
	public void ReturnToMain () {
		paused = false;
		Time.timeScale = 1;
		SceneManager.LoadScene ("MainMenu");
	}
}
