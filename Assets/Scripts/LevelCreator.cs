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
	// Virtual platform resource to be instantiated from.
	public GameObject virtualPlatformPrefab;
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
	public string url = "http://127.0.0.1:8000/api/dummyJSON/";
	// Hard-coded JSON asset for now.
	public TextAsset json;

	// Use this for initialization.
	IEnumerator Start () {
		WWW www = new WWW (url);
		yield return www;

		// Test connecting to server for dummy JSON data.
		// Requires the back-end Django server to be set up.
		JSONObject input = new JSONObject (www.text);
		print (input.GetField ("GET").str);

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
		platformInput.Add (new PlatformInput (platformJSON));
		/*foreach (JSONObject platform in platformJSON) {
			platformInput.Add (new PlatformInput (platform));
		}*/

		// Parse enemies from JSON.
		JSONObject enemyJSON = input.GetField ("enemies");
		List<EnemyInput> enemyInput = new List<EnemyInput> (enemyJSON.list.Count);
		foreach (JSONObject e in enemyJSON.list) {
			JSONObject enemy = e.GetField ("enemy");
			JSONObject enemyPathJSON = enemy.GetField ("enemy_path");
			List<PathInput> enemyPath = new List<PathInput> (enemyPathJSON.list.Count);
			foreach (JSONObject pathComponent in enemyPathJSON.list) {
				enemyPath.Add (new PathInput (pathComponent));
			}
			enemyInput.Add (new EnemyInput ((int)enemy.GetField ("enemy_type").n, enemyPath));
		}

		//Parse coins from JSON.
		JSONObject coinJSON = input.GetField ("coins");
		List<CoinInput> coinInput = new List<CoinInput> (coinJSON.list.Count);
		foreach (JSONObject c in coinJSON.list) {
			JSONObject coin = c.GetField ("position");
			coinInput.Add (new CoinInput(coin));
		}

		// Hard-coded block input until JSON is figured out here.
		List<BlockInput> blockInput = new List<BlockInput> (1);
		blockInput.Add (new BlockInput (0, 0, 2.328f, 0.782f, -0.247f));

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
		goal.transform.position = fullPath[fullPath.Count - 1].GetEnd () + Vector3.up * 0.05f;
		
		// Create virtual platforms from the input.
		foreach (PlatformInput input in platformInput) {
			GameObject virtualPlatform = Instantiate (virtualPlatformPrefab);
			virtualPlatform.transform.parent = levelManager.transform.FindChild ("Platforms").transform;
			Vector3 startCorner = new Vector3 (input.startX, input.startY, input.startZ);
			Vector3 endCorner = new Vector3 (input.endX, input.endY, input.endZ);
			virtualPlatform.transform.position = (startCorner + endCorner) / 2;
			virtualPlatform.transform.localScale = endCorner - startCorner;
		}

		// Create enemies from the input.
		List<Enemy> enemies = new List<Enemy> (enemyInput.Count);
		foreach (EnemyInput input in enemyInput) {
			if (input.enemyIndex < enemyPrefabs.Length) {
				Enemy enemy = Instantiate (enemyPrefabs[input.enemyIndex]) as Enemy;
				enemies.Add (enemy);
				enemy.transform.parent = levelManager.transform.FindChild ("Enemies").transform;
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
				enemy.GetComponent<PathMovement> ().currentPath = enemyPath[0];
				enemy.GetComponent<PathMovement> ().startPath = enemyPath[0];
			}
		}

		// Create coins from the input.
		List<Coin> coins = new List<Coin>(coinInput.Count);
		foreach (CoinInput input in coinInput) {
			Coin coin = Instantiate (coinPrefab) as Coin;
			coin.transform.parent = levelManager.transform.FindChild ("Coins").transform;
			coin.transform.position = new Vector3 (input.x, input.y, input.z);
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
			block.transform.position = new Vector3 (input.x, input.y, input.z);
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
		Vector3 start = PathUtil.MakeVectorFromPathInput(startInput);
		Vector3 end = PathUtil.MakeVectorFromPathInput(endInput);
		Vector3 center = (start + end) / 2;
		PathComponent path = Instantiate (pathPrefab, center, Quaternion.LookRotation (end - start, Vector3.up)) as PathComponent;
		path.SetPath (start, end);
		path.transform.Rotate (new Vector3 (0, -90, 0));
		Vector3 tempScale = path.transform.localScale;
		tempScale.x *= Vector3.Magnitude (end - start);
		path.transform.localScale = tempScale;
		return path;
	}
}
