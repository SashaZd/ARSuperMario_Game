using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;

// Creates a ribbon path from input.
public class LevelCreator : MonoBehaviour {

	// Path resource to be instantiated from.
	public PathComponent pathPrefab;
	// Goal resource to be instantiated from.
	public GameObject goalPrefab;
	// Texture for virtual platforms.
	public Material virtualPlatformMaterial;
	// Block resources to be instantiated from.
	public Block[] blockPrefabs;

	// Coin resource to be instantiated from.
	public Coin coinPrefab;
	// Item resources to be instantiated from.
	public GameObject[] itemPrefabs;

	// The player.
	public Player playerPrefab;
	// Enemy resources to be instantiated from.
	public Enemy[] enemyPrefabs;
	
	// The material to draw path lines with.
	public Material lineMaterial;

	// Where to get the JSON data from.
	public string url = "http://127.0.0.1:8000";
	// Hard-coded JSON asset for now.
	public TextAsset json;

	// The height of virtual platforms.
	const float PLATFORMHEIGHT = 0.1f;

	// Use this for initialization.
	IEnumerator Start () {
		WWW www = new WWW (url);
		yield return www;

		// Test connecting to server for dummy JSON data.
		// Requires the back-end Django server to be set up.
		JSONObject input = new JSONObject (www.text);
		print (input.HasField("GET") ? input.GetField ("GET").str : "Cannot connect to server.");

		//Hard-coded JSON resource for now.
		input = new JSONObject (json.text);

		// Parse the path from JSON.
		JSONObject pathJSON = input.GetField ("route");
		List<PathInput> pathInput = new List<PathInput> (pathJSON.list.Count);
		foreach (JSONObject pathComponent in pathJSON.list) {
			pathInput.Add (new PathInput (pathComponent));
		}

		// Parse platforms from JSON.
		JSONObject platformJSON = input.GetField ("virtual_platform");
		List<PlatformInput> platformInput = new List<PlatformInput> (platformJSON.list.Count);
		foreach (JSONObject platform in platformJSON.list) {
			platformInput.Add (new PlatformInput (platform));
		}

		// Parse enemies from JSON.
		JSONObject enemyJSON = input.GetField ("enemies");
		List<EnemyInput> enemyInput = new List<EnemyInput> (enemyJSON.list.Count);
		foreach (JSONObject enemy in enemyJSON.list) {
			enemyInput.Add (new EnemyInput (enemy.GetField ("enemy")));
		}

		//Parse coins from JSON.
		JSONObject coinJSON = input.GetField ("coins");
		List<CoinInput> coinInput = new List<CoinInput> (coinJSON.list.Count);
		foreach (JSONObject coin in coinJSON.list) {
			coinInput.Add (new CoinInput(coin.GetField ("position")));
		}

		// Hard-coded block input until JSON is figured out here.
		List<BlockInput> blockInput = new List<BlockInput> (1);
		//blockInput.Add (new BlockInput (0, 0, 2.328f, 0.782f, -0.247f));
		blockInput.Add (new BlockInput (0, 0, 0.5f, 1.5f, 0.5f));

		CreateLevel (pathInput, platformInput, enemyInput, coinInput, blockInput);
	}

	// Creates a level from the given input.
	public void CreateLevel (List<PathInput> pathInput, List<PlatformInput> platformInput, List<EnemyInput> enemyInput, List<CoinInput> coinInput, List<BlockInput> blockInput) {
		LevelManager levelManager = LevelManager.GetInstance ();
		
		// Construct the path from the input points.
		List<PathComponent> fullPath = new List<PathComponent>(pathInput.Count - 1);
		for (int i = 0; i < fullPath.Capacity; i++) {
			// Make and position the path component.
			PathComponent pathComponent = CreatePath (pathInput[i], pathInput[i + 1]);
			fullPath.Add (pathComponent);
			pathComponent.lineMaterial = lineMaterial;

			// Link paths together.
			if (i > 0) {
				pathComponent.previousPath = fullPath[i - 1];
				fullPath[i - 1].nextPath = pathComponent;
			}
		}

		// Set the player on a path.
		Player player = Instantiate (playerPrefab) as Player;
		player.GetComponent<PathMovement> ().currentPath = fullPath[0];
		player.GetComponent<PathMovement> ().startPath = fullPath[0];
		foreach (PathComponent pathComponent in fullPath) {
			pathComponent.transform.parent = player.transform;
		}
		levelManager.player = player;

		// Create the goal at the end of the path.
		GameObject goal = Instantiate (goalPrefab);
		goal.transform.parent = levelManager.transform.FindChild ("Platforms").transform;
		Vector3 pathEnd = fullPath[fullPath.Count - 1].GetEnd () + Vector3.up * 0.025f;
		RaycastHit hit;
		Physics.Raycast (pathEnd, Vector3.down, out hit, PathUtil.ceilingHeight * 1.1f);
		goal.transform.position = hit.point + Vector3.up * 0.025f;
		
		// Create virtual platforms from the input.
		foreach (PlatformInput input in platformInput) {
			GameObject virtualPlatform = CreatePlatform (input);
			virtualPlatform.transform.parent = levelManager.transform.FindChild ("Platforms").transform;
		}

		// Create enemies from the input.
		List<Enemy> enemies = new List<Enemy> (enemyInput.Count);
		foreach (EnemyInput input in enemyInput) {
			if (input.enemyIndex < enemyPrefabs.Length) {
				Enemy enemy = Instantiate (enemyPrefabs[input.enemyIndex]) as Enemy;
				enemies.Add (enemy);
				enemy.transform.parent = levelManager.transform.FindChild ("Enemies").transform;
				// Create the enemy path.
				int pathLength = input.path.Count - 1;
				List<PathComponent> enemyPath = new List<PathComponent> (pathLength);
				for (int i = 0; i < pathLength; i++) {
					PathComponent pathComponent = CreatePath (input.path[i], input.path[i + 1]);
					enemyPath.Add (pathComponent);
					if (i > 0) {
						pathComponent.previousPath = enemyPath[i - 1];
						enemyPath[i - 1].nextPath = pathComponent;
					}
					pathComponent.transform.parent = enemy.transform;
				}
				// Allow enemy paths to be circular.
				if (input.path[0].Equals (input.path[input.path.Count - 1])) {
					enemyPath[0].previousPath = enemyPath[pathLength - 1];
					enemyPath[pathLength - 1].nextPath = enemyPath[0];
				}
				enemy.GetComponent<PathMovement> ().currentPath = enemyPath[0];
				enemy.GetComponent<PathMovement> ().startPath = enemyPath[0];
			}
		}

		// Create coins from the input.
		List<Coin> coins = new List<Coin>(coinInput.Count);
		foreach (CoinInput input in coinInput) {
			Coin coin = Instantiate (coinPrefab) as Coin;
			coin.transform.parent = levelManager.transform.FindChild ("Coins").transform;
			coin.transform.position = input.position;
			coins.Add (coin);
		}

		// Create blocks from the input.
		List<Block> blocks = new List<Block> (blockInput.Count);
		foreach (BlockInput input in blockInput) {
			Block block = Instantiate (blockPrefabs[input.blockIndex]) as Block;
			if (input.contentIndex != -1) {
				block.contents = itemPrefabs[input.contentIndex];
			}
			block.transform.parent = levelManager.transform.FindChild ("Blocks").transform;
			block.transform.position = input.position;
			blocks.Add (block);
		}

		// Pass the needed data to the level manager to store.
		levelManager.fullPath = fullPath;
		levelManager.enemies = enemies;
		levelManager.coins = coins;
		levelManager.blocks = blocks;
	}

	// Creates a path component from a start and end point.
	PathComponent CreatePath (PathInput startInput, PathInput endInput) {
		Vector3 start = startInput.position;
		Vector3 end = endInput.position;
		Vector3 center = (start + end) / 2;
		PathComponent path = Instantiate (pathPrefab, center, Quaternion.LookRotation (end - start, Vector3.up)) as PathComponent;
		path.SetPath (start, end);
		path.transform.Rotate (new Vector3 (0, -90, 0));
		Vector3 tempScale = path.transform.localScale;
		tempScale.x *= Vector3.Magnitude (end - start);
		path.transform.localScale = tempScale;
		return path;
	}

	// Creates a virtual platform from its top vertices.
	GameObject CreatePlatform (PlatformInput input) {
		GameObject virtualPlatform = new GameObject ();
		virtualPlatform.name = "Virtual Platform";
		virtualPlatform.AddComponent<MeshFilter> ();
		virtualPlatform.AddComponent<MeshRenderer> ();
		virtualPlatform.GetComponent<Renderer> ().material = virtualPlatformMaterial;
		Mesh mesh = virtualPlatform.GetComponent<MeshFilter>().mesh;

		// Create the vertices of the platform.
		Vector3[] vertices =  new Vector3[input.vertices.Count * 2];
		for (int i = 0; i < input.vertices.Count; i++) {
			vertices[i] = input.vertices[i];
			vertices[i + input.vertices.Count] = PathUtil.SetY(input.vertices[i], input.vertices[0].y - PLATFORMHEIGHT);
		}
		mesh.vertices = vertices;

		// Find the triangles that can make up the top and bottom faces of the platform mesh.
		Triangulator triangulator = new Triangulator (input.vertices.ToArray ());
		int[] topTriangles = triangulator.Triangulate ();
		int[] triangles = new int[topTriangles.Length * 2 + input.vertices.Count * 6];
		for (int i = 0; i < topTriangles.Length; i += 3) {
			triangles[i] = topTriangles[i];
			triangles[i + 1] = topTriangles[i + 1];
			triangles[i + 2] = topTriangles[i + 2];
			triangles[topTriangles.Length + i] = topTriangles[i + 2] + input.vertices.Count;
			triangles[topTriangles.Length + i + 1] = topTriangles[i + 1] + input.vertices.Count;
			triangles[topTriangles.Length + i + 2] = topTriangles[i] + input.vertices.Count;
		}

		// Find the triangles for the sides of the platform.
		for (int i = 0; i < input.vertices.Count; i++) {
			int offset = topTriangles.Length * 2 + i * 6;
			if (i < input.vertices.Count - 1) {
				triangles[offset] = i;
				triangles[offset + 1] = i + 1;
				triangles[offset + 2] = input.vertices.Count + i;
				triangles[offset + 3] = input.vertices.Count + i + 1;
				triangles[offset + 4] = input.vertices.Count + i;
				triangles[offset + 5] = i + 1;
			} else {
				triangles[offset] = i;
				triangles[offset + 1] = 0;
				triangles[offset + 2] = input.vertices.Count + i;
				triangles[offset + 3] = input.vertices.Count;
				triangles[offset + 4] = input.vertices.Count + i;
				triangles[offset + 5] = 0;
			}
		}

		mesh.triangles = triangles;

		virtualPlatform.AddComponent<MeshCollider> ();
		virtualPlatform.GetComponent<MeshCollider> ().sharedMesh = mesh;
		return virtualPlatform;
	}

	/*
	// Debug method used to print a mesh's triangles.
	public void PrintMesh (Mesh mesh) {
		for (int i = 0; i < mesh.triangles.Length; i += 3) {
			print (mesh.vertices[mesh.triangles[i]] + "" + mesh.vertices[mesh.triangles[i + 1]] + "" + mesh.vertices[mesh.triangles[i + 2]]);
		}
	}
	*/
}
