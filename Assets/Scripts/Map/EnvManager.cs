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
	public bool Ready { get; private set; } = false;
	public static EnvManager Instance;

	static Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();
	MainCameraMovement _cam_move;
	Coroutine _updateCoroutine = null;
	Cam _cam;
	float _timer = 0;
	float _update_time = 0.1f;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	private void Start()
	{
		_cam = Camera.main.GetComponent<Cam>();
		_cam_move = Camera.main.GetComponent<MainCameraMovement>();


	}

	void Update()
	{
		if (!MainController.Instance.Ready) return;
		if (!Ready)
		{
			Initialize();
			Ready = true;
		}

		if (_timer < 0)
		{
			StartCoroutine(UpdateForestChunksInCameraView(_cam_move.GetPos()));
			StartCoroutine(TryDisableChunksOutOfView());
			_timer = _update_time;
		}

		_timer -= Time.deltaTime;
	}

	void Initialize()
	{
		SignBuildingArea();
		RoadGenerator.GenRoadsBetweenAllTeams(new Vector2Int(MapData.MapSize[1], MapData.MapSize[0]));
		StartCoroutine(ForestGenerator.GenVirtForest());
		StartCoroutine(UpdateForestChunksInCameraView(_cam.transform.position));
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

	IEnumerator TryDisableChunksOutOfView()
	{
		var out_of_view_points = GetJustLeftPoints();
		foreach (var out_p in out_of_view_points)
		{
			var map_pos = Map.WorldToMapWithCut(out_p);
			var chunk_pos = Chunk.GetChunkMapPos(map_pos);
			if (Chunk.GetChunkMapPos(_cam.GetCamProjectionCenter()) == chunk_pos)
				continue;
			if (_chunks.ContainsKey(chunk_pos) && _chunks[chunk_pos].IsEnabled())
			{
				_chunks[chunk_pos].Disable();
				yield return null;
			}
		}
	}


	IEnumerator UpdateForestChunksInCameraView(Vector3 cam_pos)
	{
		List<Vector3> map_border_points = _cam.GetCamProjBorderPoints();

		float maxTime = 0.02f;
		float startTime = Time.realtimeSinceStartup;

		foreach (var p in map_border_points)
		{
			var world_pos = Map.WorldToMapWithCut(p);
			var chunk_pos = Chunk.GetChunkMapPos(world_pos);

			if (_chunks.ContainsKey(chunk_pos) && !_chunks[chunk_pos].IsEnabled())
			{ _chunks[chunk_pos].Enable(); }

			else if (!_chunks.ContainsKey(chunk_pos))   // if we see chunk for the first time, add it to _chunks
			{
				_chunks.Add(chunk_pos, new Chunk(world_pos));
				_chunks[chunk_pos].Enable();
			}

			if (Time.realtimeSinceStartup - startTime > maxTime)
			{
				yield return null;
				startTime = Time.realtimeSinceStartup;
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


	private void SignBuildingArea()
	{
		Team[] ts = MainController.Instance.GetAllTeams();
		foreach (Team t in ts)
		{
			if (t == null) { Debug.Log("Team is NULL"); continue; }
			Vector2Int coord = Map.WorldToMap(t.GetCenter());
			int mapRadius = Mathf.RoundToInt(t.GetBuildingRadius() / Map.GetCellSize().x);
			Map.FillMapArea(coord, mapRadius, CellType.BuildArea);
		}
	}


}
