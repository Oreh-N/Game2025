using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	public static readonly Vector2Int _size = new Vector2Int(32, 32);
	static readonly Map.CellType tree_type = Map.CellType.Tree;
	static readonly Map _map = Map.Instance;
	List<GameObject> trees = new List<GameObject>();
	Vector2Int map_pos;

	public Chunk(Vector3 world_pos)
	{ map_pos = GetChunkMapPos(world_pos); }
	public Chunk(Vector2Int map_pos)
	{ this.map_pos = GetChunkMapPos(map_pos); }

	public static Vector2Int GetChunkMapPos(Vector3 world_pos)
	{
		var mapPos = _map.WorldToMap(world_pos);
		return GetMapPos(mapPos);
	}

	public static Vector2Int GetChunkMapPos(Vector2Int map_pos)
	{ return GetMapPos(map_pos); }

	/// <summary>
	/// Takes any map position and returns which chunk does it belong to
	/// </summary>
	/// <param name="mapPos"> - Any map position</param>
	/// <returns></returns>
	static Vector2Int GetMapPos(Vector2Int mapPos)
	{
		var fullX = mapPos.x / _size.x;
		var fullY = mapPos.y / _size.y;

		if (mapPos.x < 0) fullX = 0;
		else if (mapPos.x > _map.GetSize()[0]) 
			fullX = _map.GetSize()[0] / _size.x;

		if (mapPos.y < 0) fullY = 0;
		else if (mapPos.y > _map.GetSize()[1])
			fullY = _map.GetSize()[1] / _size.y;

		return new Vector2Int(fullX, fullY);
	}

	public void Enable()
	{
		for (int x = 0; x < _size.x; x++)
			for (int y = 0; y < _size.y; y++)
			{
				var cell_pos = new Vector2Int(map_pos.x + x, map_pos.y + y);
				if (!_map.IsOutOfMap(cell_pos))
				{
					var cell_type = _map.GetCellType(cell_pos);
					if (cell_type != tree_type) continue;
					var tree_pos = _map.MapToWorld(map_pos.x + x, map_pos.y + y);
					if (_map.IsOutOfMap(tree_pos) || EnvManager.Instance._treePrefab == null)
						continue;
					trees.Add(GameObject.Instantiate(EnvManager.Instance._treePrefab,
						tree_pos, Quaternion.identity));
				}
			}
	}

	public void Disable() 
	{
		for (int i = 0; i < trees.Count; i++)
		{ GameObject.Destroy(trees[i]); }
	}
}
