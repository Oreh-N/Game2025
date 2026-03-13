using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static Map;


// Environment manager
public class EnvManager : MonoBehaviour
{
	public static EnvManager Instance;

	static Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();
	public GameObject _treePrefab;
	Cam _cam;
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
		_cam = Camera.main.GetComponent<Cam>();

		SignBuildingArea(_map);
		RoadGenerator.GenRoadsBetweenAllTeams(_map, new Vector2Int(MapData.MapSize[1], MapData.MapSize[0]));
		ForestGenerator.GenVirtForest(_map);
		UpdateForestChunksInCameraView(_cam.GetPos());
	}

	void Update()
	{
		if (_cam.GetDir() != Vector3.zero)
		{ 
			UpdateForestChunksInCameraView(_cam.GetPos());
			TryDisableChunksOutOfView(_cam.GetDir());
		}
	}

	#region Dynamic Forest Generation

	private void TryDisableChunksOutOfView(Vector3 cam_dir)
	{
		var op_dir = -cam_dir;
		var out_of_view_pos = _cam.GetPos() + op_dir * _cam.width;
		var map_pos = _map.WorldToMapWithCut(out_of_view_pos);
		var chunk_pos = Chunk.GetChunkMapPos(map_pos, _map);
		if (_chunks.ContainsKey(chunk_pos))
		{
			_chunks[chunk_pos].Disable();
			_chunks.Remove(chunk_pos);
		}
	}

	void UpdateForestChunksInCameraView(Vector3 cam_pos)
	{
		List<Vector2Int> map_border_points = _cam.GetCamProjBorderPoints();
		foreach(var p in map_border_points)
		{
			var chunk_pos = Chunk.GetChunkMapPos(p, _map);
			if (!_chunks.ContainsKey(chunk_pos))
			{
				var chunk = new Chunk(chunk_pos, _map);
				chunk.Enable(_map);
				_chunks.Add(chunk_pos, chunk);
			}
		}
	}


	

	private void OnDrawGizmos()
	{
		return;
		Gizmos.color = Color.darkGoldenRod;
		var points = _cam.GetCamProjBorderPoints();
		foreach (var p in points)
		{ Gizmos.DrawSphere(_map.MapToWorld(p.x, p.y),5); }
	
	}
	#endregion


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
