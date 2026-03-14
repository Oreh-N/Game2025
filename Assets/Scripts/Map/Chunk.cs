using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	ChunkData data;


	public Chunk(Vector3 world_pos, Map map)
	{
		data.map_pos = GetChunkMapPos(world_pos, map);
		data.trees = new List<GameObject>();
	}
	public Chunk(Vector2Int map_pos_, Map map)
	{
		data.map_pos = GetMapPos(map_pos_, map);
		data.trees = new List<GameObject>();
	}

	public static Vector2Int GetChunkMapPos(Vector3 world_pos, Map map)
	{
		var mapPos = map.WorldToMapWithCut(world_pos);
		return GetMapPos(mapPos, map);
	}

	public static Vector2Int GetChunkMapPos(Vector2Int map_pos, Map map)
	{ return GetMapPos(map_pos, map); }

	public bool IsEnabled() { return data.is_enabled; }

	/// <summary>
	/// Takes any map position and returns which chunk does it belong to
	/// </summary>
	/// <param name="mapPos"> - Any map position</param>
	/// <returns></returns>
	static Vector2Int GetMapPos(Vector2Int mapPos, Map map)
	{
		//Debug.Log($"GetMapPos (mapPos): {mapPos}");
		var fullX = mapPos.x / ChunkData._size.x;
		var fullY = mapPos.y / ChunkData._size.y;

		if (mapPos.x < 0) fullX = 0;
		else if (mapPos.x > map.GetSize()[0])
			fullX = map.GetSize()[0] / ChunkData._size.x;

		if (mapPos.y < 0) fullY = 0;
		else if (mapPos.y > map.GetSize()[1])
			fullY = map.GetSize()[1] / ChunkData._size.y;

		//Debug.Log($"X: {fullX}    Y: {fullY}");
		return new Vector2Int(fullX, fullY);
	}

	/// <summary>
	/// Instanciates trees in chunk by looking where are they on the map
	/// </summary>
	/// <param name="map"></param>
	public void Enable(Map map)		// Chunks gen error is here
	{
		for (int x = 0; x < ChunkData._size.x; x++)
			for (int y = 0; y < ChunkData._size.y; y++)
			{
				var cell_pos = new Vector2Int(data.map_pos.x * ChunkData._size.x + x,
					data.map_pos.y * ChunkData._size.y + y);

				if (map.IsOutOfMap(cell_pos) ||
					map.GetCellType(cell_pos) != ChunkData.tree_type ||
					EnvManager.Instance._treePrefab == null)
					continue;

				data.trees.Add(GameObject.Instantiate(EnvManager.Instance._treePrefab,
					map.MapToWorld(cell_pos), Quaternion.identity));
			}
		data.is_enabled = true;
		//Debug.Log($"Enabled {data.map_pos} chunk");
	}

	/// <summary>
	/// Destroys all tree prefabs the chunk contains
	/// </summary>
	public void Disable()
	{
		for (int i = 0; i < data.trees.Count; i++)
		{ GameObject.Destroy(data.trees[i]); }
		data.is_enabled = false;
		//Debug.Log($"Disabled {data.map_pos} chunk");
	}
}
