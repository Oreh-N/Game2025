using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestManager : MonoBehaviour
{
	Dictionary<Chunk, List<GameObject>> _treePrefabsInChunk = new Dictionary<Chunk, List<GameObject>>();
	Dictionary<Chunk, List<GameObject>> _unitsInChunk = new Dictionary<Chunk, List<GameObject>>();
	Dictionary<Vector2, Chunk> _chunks = new Dictionary<Vector2, Chunk>();
	Terrain _terrain;

	[SerializeField] GameObject _treePrefab;

	private void Awake()
	{
		_terrain = Terrain.activeTerrain;
		InitChunks();
		DistributeTrees();
	}

	void Update()
	{
		TryActivateBusyChunk();
	}

	private void TryActivateBusyChunk()
	{
		foreach (var pair in _unitsInChunk)
		{
			if (pair.Value.Count == 0)
			{
				foreach (var prefab in _treePrefabsInChunk[pair.Key])
				{ Destroy(prefab); }

				_treePrefabsInChunk[pair.Key].Clear();
			}
			else
			foreach (var treeIndex in pair.Key._treeIndices)
			{
				TreeInstance tree = _terrain.terrainData.treeInstances[treeIndex];
				_treePrefabsInChunk[pair.Key].Add(Instantiate(_treePrefab, tree.position, Quaternion.identity));
			}
		}
	}

	private void InitChunks()
	{
		Vector3 terrainSize = _terrain.terrainData.size;

		for (int i = 0; i < terrainSize.x; i += Chunk._size.x)
		{
			for (int j = 0; j < terrainSize.z; j += Chunk._size.y)
			{           // Chunk's start position
				Vector2 chunkStartPos = new Vector2(i, j);
				_chunks.Add(chunkStartPos, new Chunk());
				_treePrefabsInChunk.Add(_chunks[chunkStartPos], new List<GameObject>());
				_unitsInChunk.Add(_chunks[chunkStartPos], new List<GameObject>());
			}
		}
	}

	public void UpdateUnitCountInChunks(GameObject unit, Chunk prevChunk)
	{
		if (unit.transform.position.x >= _terrain.terrainData.size.x ||
			unit.transform.position.z >= _terrain.terrainData.size.z)
		{ return; }

		Vector2 chunkPos = GetChunkPosition(unit.transform.position);
		if (prevChunk != _chunks[chunkPos])
		{
			_unitsInChunk[prevChunk].Remove(unit);
			_unitsInChunk[_chunks[chunkPos]].Add(unit);
		}
	}

	private void DistributeTrees()
	{
		for (int i = 0; i < _terrain.terrainData.treeInstances.Length; i++)
		{
			Vector3 worldPos = Vector3.Scale(_terrain.terrainData.treeInstances[i].position, _terrain.terrainData.size) + _terrain.transform.position;

			if (worldPos.x < _terrain.terrainData.size.x &&
				worldPos.z < _terrain.terrainData.size.z)
			{ _chunks[GetChunkPosition(worldPos)]._treeIndices.Add(i); }
		}
	}

	private Vector2 GetChunkPosition(Vector3 objWorldPos)
	{
		int x = (int)objWorldPos.x / Chunk._size.x;
		int z = (int)objWorldPos.z / Chunk._size.y;
		return new Vector2(x * Chunk._size.x, z * Chunk._size.y);
	}

	private void PrintChunks()
	{
		foreach (var chunk in _chunks)
		{
			Debug.Log($"{chunk.Key} : contains {chunk.Value._treeIndices.Count} trees");
		}
	}
}
