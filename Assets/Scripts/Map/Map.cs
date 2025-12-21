using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;
using Rnd = UnityEngine.Random;


public class Map : MonoBehaviour
{
	public enum CellType
	{
		Empty = 0,
		Building = 1,
		Unit = 2,
		Tree = 3
	}
	public static Map Instance;
	MapData data = new MapData();
	float treeGenFrequency = 60;


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		GenVirtForest();
	}


	private void GenVirtForest()
	{
		Dictionary<Vector3, float> areasInfo = ForestManager.Instance.GetBaseAreaInfo();

		for (int x = 0; x < data.Map.GetLength(0); x++)
		{
			for (int z = 0; z < data.Map.GetLength(1); z++)
			{
				if (data.Map[x, z] != CellType.Empty) continue;

				if (Rnd.Range(0, treeGenFrequency) < treeGenFrequency)
					data.Map[x, z] = CellType.Tree;
			}
		}
	}


	/// <summary>
	/// Convert map index to world position (vertex of the cell)
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns>World position</returns>
	public Vector3 MapToWorld(int x, int y)
	{
		return data.MapStart + new Vector3(x, 0, y);
	}


	/// <summary>
	/// Convert world position to map index
	/// </summary>
	/// <param name="pos"></param>
	/// <returns>Map position (indicies)</returns>
	public Vector2Int WorldToMap(Vector3 pos)
	{
		int x = Mathf.FloorToInt(pos.x - data.MapStart.x);
		int z = Mathf.FloorToInt(pos.z - data.MapStart.z);


		if (x >= MapData.MapSize[0] || z >= MapData.MapSize[1] || x < 0 || z < 0)
		{
			Debug.Log("Out of range (WorldToMap)");
			return new Vector2Int(0, 0);
		}

		return new Vector2Int(x, z);
	}

	#region TESTING
	public bool showGrid;
	public GameObject targetForGizmo;
	public bool showMap;

	private void OnDrawGizmos()
	{
		if (showGrid)
		{
			Gizmos.color = new Color(0.8f, 0, 0, 0.3f);
			for (int x = 0; x < MapData.MapSize[0]; x++)
				for (int z = 0; z < MapData.MapSize[1]; z++)
				{
					Gizmos.DrawCube(MapToWorld(x, z), data.CellSize);
				}
		}
		
		IPlaceableOnMap comp;

		if (targetForGizmo && targetForGizmo.TryGetComponent<IPlaceableOnMap>(out comp))
		{
			Gizmos.color = new Color(1f, 0.4f, 1f, 0.5f);
			Vector2Int worldPos = WorldToMap(comp.GetPos());
			var sizeOnMap = (comp.GetTakeAreaSize());
			var size = new Vector3(sizeOnMap.x, 0, sizeOnMap.y);
			Gizmos.DrawCube(MapToWorld(worldPos.x, worldPos.y), size);
		}

		if (showMap) ShowMapGizmo();
	}
	void ShowMapGizmo()
	{
		Gizmos.color = new Color(0.7f, 0, 0.4f, 0.3f);

		for (int x = 0; x < MapData.MapSize[0]; x++)
			for (int z = 0; z < MapData.MapSize[1]; z++)
			{
				if (data.Map[x, z] == 0) continue;   // will through an error if used not during the game

				Vector3 world = MapToWorld(x, z);
				Gizmos.DrawCube(world, data.CellSize);
			}
	}
	#endregion
}

