using MapSpace;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLayers = MapSpace.MapLayers.Maps;

public class MainController : MonoBehaviour
{
	public bool Ready { get; private set; } = false;
	public static MainController Instance;
	public static Plane groundPlane;
	GameObject managers;
	Team[] _teams;



	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

		groundPlane = new Plane(Vector3.up, new Vector3(0, 0, 0));
		managers = GameObject.Find("___Managers___");  // This object needed to control flow of scripts initialization
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

		// NAMES MUST BE DIFFERENT!!! (otherwise rewrites existed controller)
		_teams = new Team[3] {
			Player.Instance.Setup(new Vector2Int(50, 50), new Color(0.7f, 0.4f, 0.9f), "Nuts"),
			CreateEnemy(new Vector2Int(700,800), Color.red, ":3"),
			CreateEnemy(new Vector2Int(100, 300), Color.cadetBlue, "Alice")
		};
		foreach (var t in _teams) t.CreateBase();
		yield return null;

		managers.AddComponent<MapSpace.EnvManager>();
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
		var enemy = new GameObject($"{name}_EnemyController").AddComponent<EnemyController>();
		enemy.Setup(pos, c, name);
		
		return enemy;
	}

	public int TeamCount() { return _teams.Length; }

	/*/
	private void OnDrawGizmos()
	{
		Gizmos.color = Color.darkRed;

		var mapName = MLayers.MapNames.UnitMap;
		
		for (int i = 0; i < Chunk.GetSize().x; i++)
			for (int j = 0; j < Chunk.GetSize().y; j++)
			{
				Vector2Int mpos = new Vector2Int(i, j);
				if (MLayers.GetCellInMap(mapName, mpos) != Map.CellType.Empty)
				{ Gizmos.DrawCube(Map.MapToWorld(mpos), Map.GetCellSize());}
			}
	}
	/**/
}

