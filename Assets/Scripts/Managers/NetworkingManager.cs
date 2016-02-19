using UnityEngine;
using System;
using System.Collections;
using System.Net;

// Handles sending and receiving from the internet.
public class NetworkingManager : MonoBehaviour {

	// The singleton instance of the networking manager.
	public static NetworkingManager instance;

	// Sets the singleton instance.
	void Awake () {
		instance = this;
	}

	// Processes a string obtained from a URL.
	public void ProcessStringFromURL (string url, Action<string> OnGet) {
		StartCoroutine(GetURL (url, OnGet));
	}

	// Processes a string obtained from a URL.
	private IEnumerator GetURL (string url, Action<string> OnGet) {
		WWW www = new WWW(url);
		yield return www;
		OnGet(www.text);
	}

	// Posts a position to a URL.
	public void PostPositionInURL (string url, Vector3 position) {
		WWWForm form = new WWWForm();
		form.AddField ("x", position.x.ToString ());
		form.AddField ("y", position.y.ToString ());
		form.AddField ("z", position.z.ToString ());
		PostURL (url, form);
	}

	// Posts a form to a URL.
	private IEnumerator PostURL (string url, WWWForm form) {
		WWW www = new WWW(url, form);
		yield return www;
	}
}