﻿using UnityEngine;
using System;
using System.Collections;
using System.Net;

/// <summary>
/// Handles sending and receiving from the internet.
/// </summary>
public class NetworkingManager : MonoBehaviour {
	
	/// <summary> Where to send and receive data from. </summary>
	[Tooltip("Where to send and receive data from.")]
	public string url;

	/// <summary> Holds data about the user of the application. </summary>
	private UserData userData;

	/// <summary> The singleton instance of the networking manager. </summary>
	public static NetworkingManager instance;

	/// <summary>
	/// Sets the singleton instance.
	/// </summary>
	private void Awake() {
		instance = this;
	}

	/// <summary>
	/// Finds the user data for the user.
	/// </summary>
	private void Start() {
		userData = FindObjectOfType<UserData>();
		if (userData == null) {
			userData = gameObject.AddComponent<UserData>();
		} else if (userData.gameObject != gameObject) {
			instance = userData.GetComponent<NetworkingManager>();
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Processes a string obtained from a URL.
	/// </summary>
	/// <param name="OnGet">The process to run on the string obtained from the URL.</param>
	public void ProcessStringFromURL(Action<string> OnGet) {
		StartCoroutine(GetURL(OnGet));
	}

	/// <summary>
	/// Processes a string obtained from a URL.
	/// </summary>
	/// <param name="OnGet">The process to run on the string obtained from the specified URL.</param>
	private IEnumerator GetURL(Action<string> OnGet) {
		WWWForm form = new WWWForm();
		form.AddField("username", GetUsername());
		WWW www = new WWW(url, form);
		yield return www;

		if (www.error == null) {
			OnGet(www.text);
		} else {
			Debug.Log(www.error);
		} 
	}

	/// <summary>
	/// Posts a position to a URL.
	/// </summary>
	/// <param name="position">The position to post.</param>
	public void PostPositionInURL(Vector3 position) {
		WWWForm form = new WWWForm();
		form.AddField("username", GetUsername());
		form.AddField("x", position.x.ToString());
		form.AddField("y", position.y.ToString());
		form.AddField("z", position.z.ToString());
		PostURL(form);
	}

	/// <summary>
	/// Posts a form to a URL.
	/// </summary>
	/// <param name="form">The form to post.</param>
	private IEnumerator PostURL(WWWForm form) {
		WWW www = new WWW(url, form);
		yield return www;

		if (www.error != null) {
			Debug.Log(www.error);
		}
	}

	/// <summary>
	/// Gets the name of the player.
	/// </summary>
	/// <returns>The name of the player.</returns>
	private string GetUsername() {
		return userData == null ? "Someone" : userData.username;
	}
}