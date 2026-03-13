using System;
using System.Collections.Generic;
using UnityEngine;

public struct ChunkData
{
	public static readonly Vector2Int _size = new Vector2Int(32, 32);
	public static readonly Map.CellType tree_type = Map.CellType.Tree;
	public List<GameObject> trees;
	public Vector2Int map_pos;
}

