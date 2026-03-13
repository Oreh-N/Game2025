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
		var mapPos = map.WorldToMap(world_pos);
		return GetMapPos(mapPos, map);
	}

	public static Vector2Int GetChunkMapPos(Vector2Int map_pos, Map map)
	{ return GetMapPos(map_pos, map); }

	/// <summary>
	/// Takes any map position and returns which chunk does it belong to
	/// </summary>
	/// <param name="mapPos"> - Any map position</param>
	/// <returns></returns>
	static Vector2Int GetMapPos(Vector2Int mapPos, Map map)
	{
		var fullX = mapPos.x / ChunkData._size.x;
		var fullY = mapPos.y / ChunkData._size.y;

		if (mapPos.x < 0) fullX = 0;
		else if (mapPos.x > map.GetSize()[0])
			fullX = map.GetSize()[0] / ChunkData._size.x;

		if (mapPos.y < 0) fullY = 0;
		else if (mapPos.y > map.GetSize()[1])
			fullY = map.GetSize()[1] / ChunkData._size.y;

		return new Vector2Int(fullX, fullY);
	}

	public void Enable(Map map)
	{
		for (int x = 0; x < ChunkData._size.x; x++)
			for (int y = 0; y < ChunkData._size.y; y++)
			{
				var cell_pos = new Vector2Int(data.map_pos.x + x, data.map_pos.y + y);
				if (!map.IsOutOfMap(cell_pos))
				{
					var cell_type = map.GetCellType(cell_pos);
					if (cell_type != ChunkData.tree_type) continue;
					var tree_pos = map.MapToWorld(data.map_pos.x + x, data.map_pos.y + y);
					if (map.IsOutOfMap(tree_pos) || EnvManager.Instance._treePrefab == null)
						continue;
					data.trees.Add(GameObject.Instantiate(EnvManager.Instance._treePrefab,
						tree_pos, Quaternion.identity));
				}
			}
	}

	public void Disable()
	{
		for (int i = 0; i < data.trees.Count; i++)
		{ 
			GameObject.Destroy(data.trees[i]); 
		}
		Debug.Log("Left trees count: " + data.trees.Count.ToString());
	}
}
