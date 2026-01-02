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
		Tree = 3,
		BuildArea = 100
	}
	public static Map Instance;
	public GameObject TreePrefab;
	MapData data = new MapData();
	float treeGenFrequency = 5;


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
		SignBuildingArea();
		GenVirtForest();
		GenForest();
	}

	private void SignBuildingArea()
	{
		Team[] ts = MainController.Instance.GetAllTeams();
		foreach (Team t in ts)
		{
			Vector2Int coord = WorldToMap(t.GetCenter());
			int mapRadius = Mathf.RoundToInt(t.GetBuildingRadius() / data.CellSize.x);
			FillMapArea(coord, mapRadius, CellType.BuildArea);
		}
	}

	/// <summary>
	/// Fill the area on map (circular filling)
	/// </summary>
	/// <param name="centerCoords"> - coordinates of the center of the area that need to be filled</param>
	/// <param name="mapRadius"> - translate world radius (radius, 0, 0) to map map radius (mapRadius, 0) with WorldToMap method</param>
	/// <param name="filling"> - cell type which will fill the area</param>
	private void FillMapArea(Vector2Int centerCoords, int mapRadius, CellType filling)
	{
		data.Map[centerCoords.x, centerCoords.y] = filling;
		Queue<Vector2Int> toFill = new Queue<Vector2Int>();
		AddNearbyCellsToQueue(ref toFill, centerCoords);
		
		while (toFill.Count > 0)
		{
			var currCell = toFill.Dequeue();
			if (data.Map[currCell.x, currCell.y] == filling) continue; 
			int dx = currCell.x - centerCoords.x;
			int dy = currCell.y - centerCoords.y;
			if (dx * dx + dy * dy > mapRadius * mapRadius) continue;

			data.Map[currCell.x, currCell.y] = filling;
			AddNearbyCellsToQueue(ref toFill, currCell);
		}
	}

	private void AddNearbyCellsToQueue(ref Queue<Vector2Int> queue, Vector2Int currCoords)
	{
		List<int> range = new List<int>() { -1, 0, 1 };
		foreach (int i in range)
		{
			foreach (int j in range)
			{
				if (i == 0 && j == 0) continue;
				Vector2Int newCoords = new Vector2Int(currCoords.x + i, currCoords.y + j);
				if (IsOutOfMap(newCoords)) continue;
				queue.Enqueue(newCoords);
			}
		}
	}

	private bool IsOutOfMap(Vector2Int mapCoord)
	{
		return !(mapCoord.x < MapData.MapSize[0] && mapCoord.x >= 0 
			 && mapCoord.y < MapData.MapSize[1] && mapCoord.y >= 0);
	}


	private void GenVirtForest()
	{
		Dictionary<Vector3, float> areasInfo = ForestManager.Instance.GetBaseAreaInfo();

		for (int x = 0; x < data.Map.GetLength(0); x++)
		{
			for (int z = 0; z < data.Map.GetLength(1); z++)
			{
				if (data.Map[x, z] != CellType.Empty) continue;

				if (Rnd.Range(0, 100) < treeGenFrequency)
				{ data.Map[x, z] = CellType.Tree; }
			}
		}
	}

	private void GenForest()
	{
		if (TreePrefab == null)
		{
			Debug.Log("Not initialized tree prefab");
			return;
		}

		for (int x = 0; x < data.Map.GetLength(0); x++)
		{
			for (int z = 0; z < data.Map.GetLength(1); z++)
			{
				if (data.Map[x, z] == CellType.Tree)
				{ Instantiate(TreePrefab, MapToWorld(x, z), Quaternion.identity); }
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
			throw new Exception("WorldToMap: Out of range");
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
			var sizeOnMap = (comp.GetTakeAreaSize());   // Some objects sizes are initialized during the game
			var size = new Vector3(sizeOnMap.x, 0, sizeOnMap.y);
			Gizmos.DrawCube(MapToWorld(worldPos.x, worldPos.y), size);
		}

		if (showMap) ShowMapGizmo();
	}
	void ShowMapGizmo()// will through an error if used not during the game
	{
		Gizmos.color = new Color(0.7f, 0, 0.4f, 0.3f);

		for (int x = 0; x < MapData.MapSize[0] / 10; x++)
			for (int z = 0; z < MapData.MapSize[1] / 10; z++)
			{
				if (data.Map[x, z] == CellType.Empty) continue;

				Vector3 world = MapToWorld(x, z);
				Gizmos.DrawCube(world, data.CellSize);
			}
	}
	#endregion
}

