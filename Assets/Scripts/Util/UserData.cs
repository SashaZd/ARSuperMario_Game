using UnityEngine;

/// <summary>
/// Holds data about the user of the application.
/// </summary>
public class UserData : MonoBehaviour {

	/// <summary> The username of the user. </summary>
	[Tooltip("The username of the user.")]
	public string username = "";

	/// <summary>
	/// Makes the user data persists between scenes.
	/// </summary>
	private void Awake() {
		DontDestroyOnLoad(transform.gameObject);
	}
}
