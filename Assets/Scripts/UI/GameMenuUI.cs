using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Button listener for in-game pause menu.
/// </summary>
public class GameMenuUI : MonoBehaviour {

	/// <summary> The pause menu to open when the game is paused. </summary>
	private GameObject pauseMenu;

	/// <summary> Whether the game is paused. </summary>
	public static bool paused = false;
	/// <summary> Timer for slowing down repeated pausing and unpausing. </summary>
	private int pauseTimer = 0;
	/// <summary> The delay between pausing and unpausing. </summary>
	private const int PAUSEDELAY = 30;

	/// <summary>
	/// Registers the pause menu canvas.
	/// </summary>
	private void Start() {
		pauseMenu = transform.FindChild("Canvas").gameObject;
	}

	/// <summary>
	/// Listens for pausing and unpausing the game with the escape key.
	/// </summary>
	private void Update() {
		if (pauseTimer > 0) {
			pauseTimer--;
		}
		if (pauseTimer <= 0 && Input.GetKey(KeyCode.Escape)) {
			if (paused) {
				Unpause();
			} else {
				Pause();
			}
			pauseTimer = PAUSEDELAY;
		}
	}

	/// <summary>
	/// Pauses the game.
	/// </summary>
	private void Pause() {
		paused = true;
		pauseMenu.SetActive(true);
		Time.timeScale = 0;
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
	}

	/// <summary>
	/// Unpauses the game.
	/// </summary>
	public void Unpause() {
		paused = false;
		pauseMenu.SetActive(false);
		Time.timeScale = 1;
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	/// <summary>
	/// Returns the user to the main menu.
	/// </summary>
	public void ReturnToMain() {
		paused = false;
		Time.timeScale = 1;
		SceneManager.LoadScene("MainMenu");
	}
}
