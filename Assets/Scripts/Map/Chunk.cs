using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	ChunkData data;


	public Chunk(Vector3 world_pos)
	{
		data.map_pos = GetChunkMapPos(world_pos);
		data.trees = new List<GameObject>();
	}
	public Chunk(Vector2Int map_pos_)
	{
		data.map_pos = GetMapPos(map_pos_);
		data.trees = new List<GameObject>();
	}

	public static Vector2Int GetChunkMapPos(Vector3 world_pos)
	{
		var mapPos = Map.WorldToMapWithCut(world_pos);
		return GetMapPos(mapPos);
	}

	public static Vector2Int GetChunkMapPos(Vector2Int map_pos)
	{ return GetMapPos(map_pos); }

	public bool IsEnabled() { return data.IsEnabled; }

	/// <summary>
	/// Takes any map position and returns which chunk does it belong to
	/// </summary>
	/// <param name="mapPos"> - Any map position</param>
	/// <returns></returns>
	static Vector2Int GetMapPos(Vector2Int mapPos)
	{
		//Debug.Log($"GetMapPos (mapPos): {mapPos}");
		var fullX = mapPos.x / ChunkData._size.x;
		var fullY = mapPos.y / ChunkData._size.y;

		if (mapPos.x < 0) fullX = 0;
		else if (mapPos.x > Map.GetSize()[0])
			fullX = Map.GetSize()[0] / ChunkData._size.x;

		if (mapPos.y < 0) fullY = 0;
		else if (mapPos.y > Map.GetSize()[1])
			fullY = Map.GetSize()[1] / ChunkData._size.y;

		//Debug.Log($"X: {fullX}    Y: {fullY}");
		return new Vector2Int(fullX, fullY);
	}

	public void Enable()        // Chunks gen error is here
	{
		if (data.IsEnabled) return;

		if (!data.Initialized) Initialize();
		else
		{
			for (int i = 0; i < data.trees.Count; i++)
			{ data.trees[i].SetActive(true); }
		}
		data.IsEnabled = true;

		//Debug.Log($"Enabled {data.map_pos} chunk");
	}

	void Initialize()
	{
		for (int x = 0; x < ChunkData._size.x; x++)
			for (int y = 0; y < ChunkData._size.y; y++)
			{
				var cell_pos = new Vector2Int(data.map_pos.x * ChunkData._size.x + x,
					data.map_pos.y * ChunkData._size.y + y);

				if (Map.IsOutOfMap(cell_pos) ||
					Map.GetCellType(cell_pos) != ChunkData.tree_type)
					continue;

				var tree = TreeCreator.CreateTree(Map.MapToWorld(cell_pos));
				data.trees.Add(tree);
			}
		data.Initialized = true;
	}

	public void Disable()
	{
		if (!data.IsEnabled) return;

		for (int i = 0; i < data.trees.Count; i++)
		{ data.trees[i].SetActive(false); }
		data.IsEnabled = false;
	}

	/// <summary>
	/// Destroys all tree prefabs the chunk contains
	/// </summary>
	public void DeleteFilling()
	{
		for (int i = 0; i < data.trees.Count; i++)
		{ GameObject.Destroy(data.trees[i]); }
		data.IsEnabled = false;
		//Debug.Log($"Disabled {data.map_pos} chunk");
	}
}
