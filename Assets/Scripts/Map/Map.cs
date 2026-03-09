using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Color = UnityEngine.Color;
using MapCoord = UnityEngine.Vector2Int;


public class Map : MonoBehaviour
{
	public enum CellType
	{
		Empty = 0,
		Building = 1,
		Unit = 2,
		Tree = 3,
		BuildArea = 100,
		Road = 101,
		Error = 505
	}
	public static Map Instance;
	MapData data = new MapData();


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

	public bool TrySetCell(MapCoord coord, CellType type)
	{
		if (IsOutOfMap(coord) || data.Map[coord.x, coord.y] != CellType.Empty) 
			return false;

		data.Map[coord.x, coord.y] = type;
		return true;
	}

	public void ForceSetCell(MapCoord coord, CellType type)
	{
		if (IsOutOfMap(coord)) return;
		data.Map[coord.x, coord.y] = type;
	}

	public bool CellIs(CellType type, MapCoord coord)
	{
		if (IsOutOfMap(coord)) return false;
		return data.Map[coord.x, coord.y] == type;
	}

	public bool CellIs(CellType type, int x, int z)
	{
		if (IsOutOfMap(new MapCoord(x,z))) return false;
		return data.Map[x, z] == type;
	}

	public CellType GetCellType(MapCoord coord)
	{
		return !IsOutOfMap(coord) ? data.Map[coord.x, coord.y] : CellType.Error;
	}

	/// <summary>
	/// Fill the area on map (circular filling)
	/// </summary>
	/// <param name="centerCoords"> - coordinates of the center of the area that need to be filled</param>
	/// <param name="radius"> - translate world radius (radius, 0, 0) to map map radius (mapRadius, 0) with WorldToMap method</param>
	/// <param name="filling"> - cell type which will fill the area</param>
	public void FillMapArea(MapCoord centerCoords, int radius, CellType filling)
	{
		ForceSetCell(centerCoords, filling);
		Queue<MapCoord> toFill = new Queue<MapCoord>();
		AddNearbyCellsToQueue(ref toFill, centerCoords);

		while (toFill.Count > 0)
		{
			var currCell = toFill.Dequeue();
			int dx = currCell.x - centerCoords.x;
			int dy = currCell.y - centerCoords.y;
			if (dx * dx + dy * dy > radius * radius) continue;

			if (!TrySetCell(currCell, filling))
				continue;
			AddNearbyCellsToQueue(ref toFill, currCell);
		}
	}

	private void AddNearbyCellsToQueue(ref Queue<MapCoord> queue, MapCoord currCoords)
	{
		List<int> range = new List<int>() { -1, 0, 1 };
		foreach (int i in range)
		{
			foreach (int j in range)
			{
				if (i == 0 && j == 0) continue;
				MapCoord newCoords = new MapCoord(currCoords.x + i, currCoords.y + j);
				if (IsOutOfMap(newCoords)) continue;
				queue.Enqueue(newCoords);
			}
		}
	}

	public bool IsOutOfMap(MapCoord mapCoord)
	{
		return !(mapCoord.x < MapData.MapSize[0] && mapCoord.x >= 0
			 && mapCoord.y < MapData.MapSize[1] && mapCoord.y >= 0);
	}

	public bool IsOutOfMap(Vector3 coord)
	{
		var mapCoord = WorldToMap(new Vector3(coord.x, 0, coord.z));
		return IsOutOfMap(mapCoord);
	}

	/// <summary>
	/// Convert map index to world position (vertex of the cell)
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns>World position</returns>
	public Vector3 MapToWorld(int x, int y)
	{ return data.MapStart + new Vector3(x, 0, y); }

	public Vector3 GetCellSize()
	{ return data.CellSize; }

	public int[] GetSize()
	{ return MapData.MapSize; }

	/// <summary>
	/// Convert world position to map index
	/// </summary>
	/// <param name="pos"></param>
	/// <returns>Map position (indicies)</returns>
	public MapCoord WorldToMap(Vector3 pos)
	{
		int x = Mathf.FloorToInt(pos.x - data.MapStart.x);
		int z = Mathf.FloorToInt(pos.z - data.MapStart.z);


		if (x >= MapData.MapSize[0] || z >= MapData.MapSize[1] || x < 0 || z < 0)
		{
			throw new Exception($"Map.WorldToMap: Out of range. Did you mean ({pos.x}, {pos.y}, {pos.z}) position");
		}

		return new MapCoord(x, z);
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
			MapCoord worldPos = WorldToMap(comp.GetPos());
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
				if (CellIs(CellType.Empty, x, z)) continue;

				Vector3 world = MapToWorld(x, z);
				Gizmos.DrawCube(world, data.CellSize);
			}
	}
	#endregion
}

