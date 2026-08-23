using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


namespace MapSpace
{
	using MNames = MapLayers.Maps.MapNames;


	// Environment manager
	public class EnvManager : MonoBehaviour
	{
		public bool Ready { get; private set; } = false;

		bool ChunkUpdateReady = true;
		bool ChunkRemReady = true;

		public static EnvManager Instance;

		static Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();
		MainCameraMovement _cam_move;
		Cam _cam;


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

			StartCoroutine(Initialize());
		}

		void Update()
		{
			if (!MainController.Instance.Ready) return;

			if (ChunkUpdateReady)
			{
				ChunkUpdateReady = false;
				StartCoroutine(UpdateForestChunks(_cam_move.GetPos()));
			}
		}

		void OnDestroy()
		{ _chunks.Clear(); }

		IEnumerator Initialize()
		{
			SignBuildingArea();
			RoadGenerator.GenRoadsBetweenAllTeams(new Vector2Int(MapData.MapSize[1], MapData.MapSize[0]));
			yield return StartCoroutine(ForestGenerator.GenVirtForest());
			MapSpace.MapLayers.Maps.ResetMap(MNames.EnvMap);
			Ready = true;
		}

		#region Dynamic Forest Generation


		IEnumerator UpdateForestChunks(Vector3 cam_pos)
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
			DisableOutOfViewChunks(map_border_points);

			ChunkUpdateReady = true;
		}

		void DisableOutOfViewChunks(List<Vector3> pointsInView)
		{
			foreach (var pair in _chunks)
			{
				if (pair.Value.IsEnabled())
				{
					bool inView = false;
					foreach (var p in pointsInView)
					{
						var chunkMapPos = Map.WorldToMapWithCut(p);
						var chunkInViewPos = Chunk.GetChunkMapPos(chunkMapPos);
						if (pair.Key == chunkInViewPos)
						{ inView = true; break; }
					}
					if (!inView) pair.Value.Disable();
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

		/*/
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
				Map.FillMapAreaCircle(coord, mapRadius, Map.CellType.BuildArea, MNames.EnvMap);
			}
		}


	}
}