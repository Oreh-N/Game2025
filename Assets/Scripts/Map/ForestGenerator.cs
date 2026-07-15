using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Rnd = UnityEngine.Random;


namespace MapSpace
{
	using MNames = MapLayers.Maps.MapNames;

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
					var pos = new Vector2Int(x, z);
					if (Rnd.Range(0, 100) < treeGenFrequency && 
						Map.CellIs(Map.CellType.Empty, pos, MNames.BuildingMap))
					{ Map.TrySetCell(pos, Map.CellType.Tree, MNames.EnvironmentMap); }
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
					if (Map.CellIs(Map.CellType.Tree, x, z, MNames.EnvironmentMap))
					{ TreeCreator.CreateTree(Map.MapToWorld(x, z)); }
				}
			}
		}
	}
}

// 03.07. Trying to optimize tree spawning.Right now might cause spikes in fps and take ~30 ms
// 