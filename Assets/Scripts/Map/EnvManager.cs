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
	MainCameraMovement _cam_move;
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
		_cam_move = Camera.main.GetComponent<MainCameraMovement>();

		SignBuildingArea(_map);
		RoadGenerator.GenRoadsBetweenAllTeams(_map, new Vector2Int(MapData.MapSize[1], MapData.MapSize[0]));
		StartCoroutine(ForestGenerator.GenVirtForest(_map));
		UpdateForestChunksInCameraView(_cam.transform.position);
	}

	void Update()
	{
		if (_cam_move.GetDir() != Vector3.zero)
		{
			UpdateForestChunksInCameraView(_cam_move.GetPos());
			TryDisableChunksOutOfView();
		}
	}

	#region Dynamic Forest Generation

	/// <summary>
	/// Get 3 points from the position camera just left
	/// </summary>
	/// <param name="cam_dir"></param>
	/// <returns></returns>
	private Vector3[] GetJustLeftPoints()
	{
		var cam_proj_center = _cam.GetCamProjectionCenter();

		Vector3 dir = _cam_move._last_dir.normalized;
		dir.y = 0;
		dir.Normalize();

		Vector3 right = Vector3.Cross(Vector3.up, dir).normalized;
		Vector3 left = -right;

		float dist = ChunkData._size.x * 2.5f;

		Vector3 basePoint = cam_proj_center - dir * dist;

		Vector3[] points = new Vector3[5] {
		basePoint,
		basePoint + right * dist/2,
		basePoint - right * dist/2,
		basePoint + right * dist,
		basePoint - right * dist,
	};

		return points;
	}

	private void TryDisableChunksOutOfView()
	{
		var out_of_view_points = GetJustLeftPoints();
		foreach (var out_p in out_of_view_points)
		{
			var map_pos = _map.WorldToMapWithCut(out_p);
			var chunk_pos = Chunk.GetChunkMapPos(map_pos, _map);
			if (Chunk.GetChunkMapPos(_cam.GetCamProjectionCenter(), _map) == chunk_pos) 
				continue;
			if (_chunks.ContainsKey(chunk_pos) && _chunks[chunk_pos].IsEnabled())
			{ _chunks[chunk_pos].Disable(); }
		}
	}

	void UpdateForestChunksInCameraView(Vector3 cam_pos)
	{
		List<Vector3> map_border_points = _cam.GetCamProjBorderPoints();
		foreach (var p in map_border_points)
		{
			var world_pos = _map.WorldToMapWithCut(p);
			var chunk_pos = Chunk.GetChunkMapPos(world_pos, _map);
			if (_chunks.ContainsKey(chunk_pos) && !_chunks[chunk_pos].IsEnabled())
			{ _chunks[chunk_pos].Enable(_map); }
			else if (!_chunks.ContainsKey(chunk_pos))   // if we see chunk for the first time, add it to _chunks
			{
				_chunks.Add(chunk_pos, new Chunk(world_pos, _map));
				// Debug.Log($"{chunk_pos}      {_chunks.Count}");
				_chunks[chunk_pos].Enable(_map);
			}
		}
	}

	/*/
	private void OnDrawGizmos()
	{
		if (!Application.isPlaying || _cam == null) return;
		Gizmos.color = Color.darkGoldenRod;
		var points = _cam.GetCamProjBorderPoints();
		foreach (var p in points) { 
		
			Gizmos.DrawSphere(p,3);
		}
	}
	/**/

	/**/
	private void OnDrawGizmos()
	{
		if (!Application.isPlaying || _cam == null) return;
		
		var points = GetJustLeftPoints();

		Gizmos.color = Color.deepPink;
		Gizmos.DrawSphere(points[0], 0.5f);

		Gizmos.color = Color.red;
		Gizmos.DrawSphere(points[1], 0.5f);

		Gizmos.color = Color.black;
		Gizmos.DrawSphere(points[2], 0.5f);

		Gizmos.color = Color.pink;
		Gizmos.DrawSphere(points[3], 0.5f);

		Gizmos.color = Color.peachPuff;
		Gizmos.DrawSphere(points[4], 0.5f);
		
	}
	/**/
	#endregion


	public Dictionary<Vector3, float> GetBaseAreaInfo()
	{
		if (!MainController.Instance.Ready) return null;

		var areasInfo = new Dictionary<Vector3, float>();
		Team[] teams = MainController.Instance.GetAllTeams();

		if (teams != null)
			foreach (var t in teams)
			{
				if (t == null) { Debug.Log("Team is NULL"); continue; }
				if (!areasInfo.ContainsKey(t.GetCenter()))
					areasInfo.Add(t.GetCenter(), t.GetBuildingRadius());
			}

		return areasInfo;
	}


	private void SignBuildingArea(Map map)
	{
		if (map == null || MainController.Instance == null || !MainController.Instance.Ready) return;
		Team[] ts = MainController.Instance.GetAllTeams();
		foreach (Team t in ts)
		{
			if (t == null) { Debug.Log("Team is NULL"); continue; }
			Vector2Int coord = map.WorldToMap(t.GetCenter());
			int mapRadius = Mathf.RoundToInt(t.GetBuildingRadius() / map.GetCellSize().x);
			map.FillMapArea(coord, mapRadius, CellType.BuildArea);
		}
	}


}
