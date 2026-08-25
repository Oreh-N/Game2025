using MapSpace;
using Palmmedia.ReportGenerator.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
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

	private void Update()
	{


	}

	IEnumerator InitializeManagers()
	{
		managers.AddComponent<UIManager>();
		yield return new WaitUntil(() => UIManager.Instance.Ready);

		managers.AddComponent<MapController>();
		yield return new WaitUntil(() => MLayers.Ready);

		managers.AddComponent<Player>();
		yield return new WaitUntil(() => Player.Instance.Ready());

		managers.AddComponent<BuildingManager>();
		yield return new WaitUntil(() => BuildingManager.Instance.Ready());

		managers.AddComponent<UnitSelectionManager>();
		UnitSelectionManager.Instance.GroundMarker = GameObject.FindWithTag("Marker");
		if (!UnitSelectionManager.Instance.GroundMarker)
			Debug.Log("Didn't find ground marker for UnitSelectionManager");
		UnitSelectionManager.Instance.GroundMarker.SetActive(false);
		yield return new WaitUntil(() => UnitSelectionManager.Instance.Ready);

		// NAMES MUST BE DIFFERENT!!! (otherwise rewrites existed controller)
		_teams = new Team[3] {
			Player.Instance.Setup(new Vector2Int(50, 50), Color.violetRed, "Nuts"),
			CreateEnemy(new Vector2Int(700,800), Color.lightSeaGreen, ":3"),
			CreateEnemy(new Vector2Int(100, 300), Color.darkBlue, "Alice")
		};

		managers.AddComponent<MapSpace.EnvManager>();   // TEAMS HAVE TO BE CREATED FIRST!
		yield return new WaitUntil(() => EnvManager.Instance.Ready);

		foreach (var t in _teams) t.CreateBase();
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

	///**/
	//private void OnDrawGizmos()
	//{
	//	if (!Application.isPlaying || !Ready) return;

	//	var mapName = MLayers.MapNames.EnvMap;
	//	//var size = Chunk.GetSize();
	//	var size = Map.GetSize();

	//	for (int i = 0; i < size.x; i++)
	//		for (int j = 0; j < size.y; j++)
	//		{
	//			Vector2Int mpos = new Vector2Int(i, j);
	//			/**/if (MLayers.GetBasicCellInMap(mapName, mpos) == Map.CellType.WorkerUnit)
	//			{
	//				Gizmos.color = Color.blanchedAlmond;
	//				Gizmos.DrawCube(Map.MapToWorld(mpos), Map.GetCellSize()); 
	//			}
	//			if (MLayers.GetBasicCellInMap(mapName, mpos) == Map.CellType.MainBuild)
	//			{
	//				Gizmos.color = Color.darkRed;
	//				Gizmos.DrawCube(Map.MapToWorld(mpos), Map.GetCellSize());
	//			}
	//			if (MLayers.GetBasicCellInMap(mapName, mpos) == Map.CellType.Spawner)
	//			{
	//				Gizmos.color = Color.blueViolet;
	//				Gizmos.DrawCube(Map.MapToWorld(mpos), Map.GetCellSize());
	//			}
	//			if (MLayers.GetBasicCellInMap(mapName, mpos) == Map.CellType.Warehouse)
	//			{
	//				Gizmos.color = Color.deepPink;
	//				Gizmos.DrawCube(Map.MapToWorld(mpos), Map.GetCellSize());
	//			}
	//			if (MLayers.GetBasicCellInMap(mapName, mpos) == Map.CellType.BasicTower)
	//			{
	//				Gizmos.color = Color.greenYellow;
	//				Gizmos.DrawCube(Map.MapToWorld(mpos), Map.GetCellSize());
	//			}/*/

	//			// DOESNT WORK
	//			if (MLayers.GetCellInMap(mapName, mpos) != Map.CellType.Empty)
	//			{
	//				int teamId = MLayers.GetCellTeamID(mapName, mpos);
	//				if (TeamCount() > teamId)
	//				{
	//					Team t = GetTeam(teamId);
	//					Gizmos.color = t.GetColor();
	//					Gizmos.DrawCube(Map.MapToWorld(mpos), Map.GetCellSize());
	//				}
	//			}
	//			/**/
	//		}
	//}
	///**/
}

