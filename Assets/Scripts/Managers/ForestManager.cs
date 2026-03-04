using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Map;


public class ForestManager : MonoBehaviour
{
	public static ForestManager Instance;

	static Dictionary<Vector2Int, Chunk> _chunks = new Dictionary<Vector2Int, Chunk>();
	[SerializeField] GameObject _treePrefab;
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
		
		SignBuildingArea(_map);
		RoadGenerator.GenRoadsBetweenAllTeams(_map, new Vector2Int(MapData.MapSize[1], MapData.MapSize[0]));
		ForestGenerator.GenVirtForest(_map);
		ForestGenerator.GenForest(_treePrefab, _map);

		InitChunks();
		DistributeTrees();
	}

	void Update()
	{
		foreach (var chunk in _chunks.Values)
		{
			chunk.ActivateIfHaveUnits();
			chunk.EraseCutOffTrees();
		}
	}

	public void DestroyPrefab(GameObject prefab)
	{ Destroy(prefab.gameObject); }

	public void SpawnTreePrefabInChunk(Chunk chunk, int treeIndex)
	{
		//TreeInstance tree = _terrain.terrainData.treeInstances[treeIndex];
		//Vector3 treeWorldPos = Vector3.Scale(tree.position, _terrain.terrainData.size) + _terrain.transform.position;
		//chunk.TreePrefabs.Add(Instantiate(_treePrefab, treeWorldPos, Quaternion.identity));
	}

	public void ChangeTreeColor(int treeIndex)
	{
		//var trees = _terrain.terrainData.treeInstances;
		//var t = trees[treeIndex];
		//t.color = new Color32(255, 255, 255, 0);
		//trees[treeIndex] = t;
		//_terrain.terrainData.treeInstances = trees;
	}

	private void InitChunks()
	{
		//Vector3 terrSize = _terrain.terrainData.size;

		//for (int i = 0; i < terrSize.x; i += Chunk._size.x)
		//{
		//	for (int j = 0; j < terrSize.z; j += Chunk._size.y)
		//	{
		//		Vector2Int chunkStartPos = new Vector2Int(i, j);
		//		_chunks.Add(chunkStartPos, new Chunk());
		//	}
		//}
	}

	private void DistributeTrees()
	{
		//var tData = _terrain.terrainData;

		//for (int i = 0; i < tData.treeInstances.Length; i++)
		//{
		//	Vector3 worldPos = Vector3.Scale(tData.treeInstances[i].position, tData.size) + _terrain.transform.position;

		//	if (worldPos.x < tData.size.x &&
		//		worldPos.z < tData.size.z)
		//	{ GetChunkOnPosition(worldPos).TreeIndices.Add(i); }
		//}
	}

	public void InitializeUnitInChunk(GameObject unit)
	{
		//if (!ObjIsOnTerrain(unit))
		//{ return; }

		Vector2Int chunkPos = GetChunkPosition(unit.transform.position);
		_chunks[chunkPos].UnitsOnChunk.Add(unit);
	}

	public void UpdateUnitCountInChunks(GameObject unit, Chunk prevChunk)
	{
		//if (!ObjIsOnTerrain(unit))
		//{ return; }

		Vector2Int chunkPos = GetChunkPosition(unit.transform.position);

		if (prevChunk != _chunks[chunkPos])
		{
			prevChunk.UnitsOnChunk.Remove(unit);
			_chunks[chunkPos].UnitsOnChunk.Add(unit);
		}
	}

	//private bool ObjIsOnTerrain(GameObject unit)
	//{
	//	if (unit.transform.position.x >= _terrain.terrainData.size.x ||
	//		unit.transform.position.z >= _terrain.terrainData.size.z ||
	//		unit.transform.position.x < 0 || unit.transform.position.z < 0)
	//		return false;
	//	return true;
	//}

	public Chunk GetChunkOnPosition(Vector3 worldPos)
	{
		return _chunks[GetChunkPosition(worldPos)];
	}

	public Vector2Int GetChunkPosition(Vector3 objWorldPos)
	{
		int x = (int)objWorldPos.x / Chunk._size.x;
		int z = (int)objWorldPos.z / Chunk._size.y;
		return new Vector2Int(x * Chunk._size.x, z * Chunk._size.y);
	}

	private void PrintChunks()
	{
		foreach (var chunk in _chunks)
		{
			Debug.Log($"{chunk.Key} : contains {chunk.Value.TreeIndices.Count} trees");
		}
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
