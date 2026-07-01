using System;
using System.Collections.Generic;
using UnityEngine;
using Color = UnityEngine.Color;
using MapCoord = UnityEngine.Vector2Int;


public static class Map
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
	static MapData data = new MapData();


	public static bool TrySetCell(MapCoord coord, CellType type)
	{
		if (IsOutOfMap(coord) || data.Map[coord.x, coord.y] != CellType.Empty) 
			return false;

		data.Map[coord.x, coord.y] = type;
		return true;
	}

	public static void ForceSetCell(MapCoord coord, CellType type)
	{
		if (IsOutOfMap(coord)) return;
		data.Map[coord.x, coord.y] = type;
	}

	public static bool CellIs(CellType type, MapCoord coord)
	{
		if (IsOutOfMap(coord)) return false;
		return data.Map[coord.x, coord.y] == type;
	}

	public static bool CellIs(CellType type, int x, int z)
	{
		if (IsOutOfMap(new MapCoord(x,z))) return false;
		return data.Map[x, z] == type;
	}

	public static CellType GetCellType(MapCoord coord)
	{
		return !IsOutOfMap(coord) ? data.Map[coord.x, coord.y] : CellType.Error;
	}

	/// <summary>
	/// Fill the area on map (circular filling)
	/// </summary>
	/// <param name="centerCoords"> - coordinates of the center of the area that need to be filled</param>
	/// <param name="radius"> - translate world radius (radius, 0, 0) to map map radius (mapRadius, 0) with WorldToMap method</param>
	/// <param name="filling"> - cell type which will fill the area</param>
	public static void FillMapArea(MapCoord centerCoords, int radius, CellType filling)
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

	private static void AddNearbyCellsToQueue(ref Queue<MapCoord> queue, MapCoord currCoords)
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

	public static bool IsOutOfMap(MapCoord mapCoord)
	{
		return !(mapCoord.x < MapData.MapSize[0] && mapCoord.x >= 0
			 && mapCoord.y < MapData.MapSize[1] && mapCoord.y >= 0);
	}

	public static bool IsOutOfMap(Vector3 coord)
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
	public static Vector3 MapToWorld(int x, int y)
	{ return data.MapStart + new Vector3(x, 0, y); }

	public static Vector3 MapToWorld(Vector2Int map_pos)
	{ return data.MapStart + new Vector3(map_pos.x, 0, map_pos.y); }

	public static Vector3 GetCellSize()
	{ return data.CellSize; }

	public static int[] GetSize()
	{ return MapData.MapSize; }

	/// <summary>
	/// Convert world position to map index
	/// </summary>
	/// <param name="pos"></param>
	/// <returns>Map position (indicies)</returns>
	public static MapCoord WorldToMap(Vector3 pos)
	{
		int x = Mathf.FloorToInt(pos.x - data.MapStart.x);
		int z = Mathf.FloorToInt(pos.z - data.MapStart.z);


		if (IsOutOfMap(new MapCoord(x, z)))
		{
			return WorldToMapWithCut(pos);
		}

		return new MapCoord(x, z);
	}

	/// <summary>
	/// Convert world position to map index. If given position is out of map, 
	/// then the position will be cutted to fit into the map.
	/// </summary>
	/// <param name="pos"></param>
	/// <returns>Map position (indicies)</returns>
	public static MapCoord WorldToMapWithCut(Vector3 pos)
	{
		int x = Mathf.FloorToInt(pos.x - data.MapStart.x);
		int z = Mathf.FloorToInt(pos.z - data.MapStart.z);

		if (x < 0) { x = 0; }
		else if (x >= MapData.MapSize[0]) {  x = MapData.MapSize[0]; }
		if (z < 0) { z = 0; }
		else if (z >= MapData.MapSize[1]) { z = MapData.MapSize[1]; }

		return new MapCoord(x, z);
	}

}

