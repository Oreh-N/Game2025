using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Map;


// Environment manager
public class EnvManager : MonoBehaviour
{
	public static EnvManager Instance;

	static Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();
	public GameObject _treePrefab;
	MainCameraMovement _cam_move;
	Map _map;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	private void Start()
	{
		_map = Map.Instance;
		_cam_move = Camera.main.GetComponent<MainCameraMovement>();

		SignBuildingArea(_map);
		RoadGenerator.GenRoadsBetweenAllTeams(_map, new Vector2Int(MapData.MapSize[1], MapData.MapSize[0]));
		ForestGenerator.GenVirtForest(_map);
		

	}

	void Update()
	{
		
	}

	void GenForestChunksInCameraView()
	{

	}

	public Dictionary<Vector3, float> GetBaseAreaInfo()
	{
		var areasInfo = new Dictionary<Vector3, float>();
		Team[] teams = MainController.Instance.GetAllTeams();

		if (teams != null)
			foreach (var t in teams)
			{ 
				if (!areasInfo.ContainsKey(t.GetCenter()))
				areasInfo.Add(t.GetCenter(), t.GetBuildingRadius());
			}

		return areasInfo;
	}


	private void SignBuildingArea(Map map)
	{
		Team[] ts = MainController.Instance.GetAllTeams();
		foreach (Team t in ts)
		{
			Vector2Int coord = map.WorldToMap(t.GetCenter());
			int mapRadius = Mathf.RoundToInt(t.GetBuildingRadius() / map.GetCellSize().x);
			map.FillMapArea(coord, mapRadius, CellType.BuildArea);
		}
	}


}
