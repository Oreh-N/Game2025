using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
    public static readonly Vector2Int _size = new Vector2Int(32, 32);
    public List<int> _treeIndices = new List<int>();
}
