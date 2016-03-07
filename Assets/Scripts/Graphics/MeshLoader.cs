using UnityEngine;
using System.Collections;

/// <summary>
/// Periodically loads the world object mesh.
/// </summary>
public class MeshLoader : MonoBehaviour {

	/// <summary> The mesh filter to load to. </summary>
	private MeshFilter meshFilter;
	/// <summary> The importer to load .obj files with. </summary>
	private ObjImporter objImporter;

	/// <summary> The file path to the .obj to load from. </summary>
	[SerializeField]
	[Tooltip("The file path to the .obj to load from.")]
	private string objPath;

	/// <summary> The time (in seconds) between updates to the mesh. </summary>
	[SerializeField]
	[Tooltip("The time (in seconds) between updates to the mesh.")]
	private float updateTime;
	/// <summary> Timer for updating the mesh. </summary>
	private float updateTimer;

	/// <summary>
	/// Finds the mesh to load to.
	/// </summary>
	private void Start () {
		Transform mesh = transform.FindChild("Mesh");
		meshFilter = mesh.GetComponent<MeshFilter>();
		objImporter = new ObjImporter();
	}

	/// <summary>
	/// Periodically reloads the mesh.
	/// </summary>
	private void Update () {
		updateTimer -= Time.deltaTime;
		if (updateTimer <= 0) {
			updateTimer = updateTime;
			meshFilter.mesh = objImporter.ImportFile(objPath);
		}
	}
}
