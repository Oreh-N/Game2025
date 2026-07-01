using System;
using System.Collections;
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

	public static IEnumerator GenVirtForest()
	{
		if (!MainController.Instance.Ready) yield return null;

		Dictionary<Vector3, float> areasInfo = EnvManager.Instance.GetBaseAreaInfo();

		for (int x = 0; x < Map.GetSize()[0]; x++)
		{
			for (int z = 0; z < Map.GetSize()[1]; z++)
			{
				if (Rnd.Range(0, 100) < treeGenFrequency)
				{ Map.TrySetCell(new Vector2Int(x, z), CellType.Tree); }
			}
		}
		yield return null;
	}

	public static void GenWholeMapForest(GameObject treePrefab)
	{
		if (treePrefab == null)
		{
			Debug.Log("Not initialized tree prefab");
			return;
		}

		for (int x = 0; x < Map.GetSize()[0]; x++)
		{
			for (int z = 0; z < Map.GetSize()[1]; z++)
			{
				if (Map.CellIs(CellType.Tree, x, z))
				{ TreeCreator.CreateTree(Map.MapToWorld(x, z)); }
			}
		}
	}
}

