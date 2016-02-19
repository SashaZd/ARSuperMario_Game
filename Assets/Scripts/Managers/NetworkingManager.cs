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
	public void ProcessStringFromURL (String url, Action<string> OnGet) {
		StartCoroutine(FetchURL (url, OnGet));
	}

	// Processes a string obtained from a URL.
	private IEnumerator FetchURL (String url, Action<string> OnGet) {
		WWW www = new WWW(url);
		yield return www;
		OnGet(www.text);
	}
}