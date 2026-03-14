using System;
using System.Collections.Generic;
using UnityEngine;

public struct ChunkData
{
	public static readonly Vector2Int _size = new Vector2Int(40, 40);
	public static readonly Map.CellType tree_type = Map.CellType.Tree;
	public List<GameObject> trees;
	public Vector2Int map_pos;
	public bool is_enabled;
}

