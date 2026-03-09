using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using static Map;
using Rnd = UnityEngine.Random;


public static class ForestGenerator
{

	static float treeGenFrequency = 5;

	public static void GenVirtForest(Map map)
	{
		Dictionary<Vector3, float> areasInfo = EnvManager.Instance.GetBaseAreaInfo();

		for (int x = 0; x < map.GetSize()[0]; x++)
		{
			for (int z = 0; z < map.GetSize()[1]; z++)
			{
				if (Rnd.Range(0, 100) < treeGenFrequency)
				{ map.TrySetCell(new Vector2Int(x, z), CellType.Tree); }
			}
		}
	}

	public static void GenWholeMapForest(GameObject treePrefab, Map map)
	{
		if (treePrefab == null)
		{
			Debug.Log("Not initialized tree prefab");
			return;
		}

		for (int x = 0; x < map.GetSize()[0]; x++)
		{
			for (int z = 0; z < map.GetSize()[1]; z++)
			{
				if (map.CellIs(CellType.Tree, x, z))
				{ GameObject.Instantiate(treePrefab, map.MapToWorld(x, z), Quaternion.identity); }
			}
		}
	}

	public static void GenForestChunk(Dictionary<Vector2Int, Chunk> chunks, Vector3 cam_pos)
	{
		
	}
}

