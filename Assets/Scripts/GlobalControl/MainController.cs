using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
	public static MainController Instance;
	GameObject managers;
	Team[] _teams;
	public bool Ready { get; private set; } = false;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }


		managers = GameObject.FindWithTag("Managers");  // This object needed to control flow of scripts initialization
		if (!managers) Debug.Log("Can't find manager holder");
	}

	private void Start()
	{
		StartCoroutine(InitializeManagers());

	}


	IEnumerator InitializeManagers()
	{
		managers.AddComponent<UIManager>();
		yield return null;

		managers.AddComponent<Map>();
		yield return null;

		managers.AddComponent<MapController>();
		yield return null;

		managers.AddComponent<Player>();
		yield return null;

		managers.AddComponent<BuildingManager>();
		yield return null;

		managers.AddComponent<UnitSelectionManager>();
		UnitSelectionManager.Instance.GroundMarker = GameObject.FindWithTag("Marker");
		if (!UnitSelectionManager.Instance.GroundMarker) 
			Debug.Log("Didn't find ground marker for UnitSelectionManager");
		UnitSelectionManager.Instance.GroundMarker.SetActive(false);
		yield return null;

		_teams = new Team[3] {
			Player.Instance.Setup(new Vector2Int(50, 50), new Color(0.7f, 0.4f, 0.9f), "Nuts").CreateBase(),
			CreateEnemy(new Vector2Int(700,800), Color.red, ":3").CreateBase(),
			CreateEnemy(new Vector2Int(100, 300), Color.green, "Alice").CreateBase()
		};
		yield return null;

		managers.AddComponent<EnvManager>();
		EnvManager.Instance._treePrefab = Prefabs.Tree1;
		yield return null;

		Ready = true;
	}

	public Team[] GetAllTeams() { return _teams; }

	/// <summary>
	/// Returns team by its ID
	/// </summary>
	/// <param name="teamID"></param>
	/// <returns>Returns team if exists, else returns null</returns>
	public Team GetTeam(int teamID)
	{
		if (teamID < 0) 
			return null;
		if (_teams != null && teamID < _teams.Length)
			return _teams[teamID];
		Debug.LogWarning("Team with ID " + teamID + " does not exist.");
		return null;
	}

	private Team CreateEnemy(Vector2Int pos, Color c, string name)
	{
		var enemyObj = new GameObject();
		var enemy = enemyObj.AddComponent<EnemyController>();
		enemy.Setup(pos, c, name);
		
		return enemyObj.GetComponent<Team>();
	}

	public int TeamCount() { return _teams.Length; }
}

// coroutine (nebo vedlejsi vlakno)
// nemenit verze unity
