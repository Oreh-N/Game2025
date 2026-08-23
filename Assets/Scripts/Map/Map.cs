using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using MapSpace.MapLayers;
using Color = UnityEngine.Color;
using MapCoord = UnityEngine.Vector2Int;
using Unity.VisualScripting;


namespace MapSpace
{
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


		public static bool TrySetCell(MapCoord coord, CellType type, Maps.MapNames mapName)
		{
			if (IsOutOfMap(coord) || Maps.GetCellInMap(mapName, coord) != CellType.Empty)
				return false;

			Maps.TrySetCell(mapName, coord, type);
			return true;
		}

		public static void ForceSetCell(MapCoord coord, CellType type, Maps.MapNames mapName)
		{
			if (IsOutOfMap(coord)) return;
			Maps.ForceSetCell(mapName, coord, type);
		}

		public static bool CellIs(CellType type, MapCoord coord, Maps.MapNames mapName)
		{
			if (IsOutOfMap(coord)) return false;
			return Maps.GetCellInMap(mapName, coord) == type;
		}

		public static bool CellIs(CellType type, int x, int z, Maps.MapNames mapName)
		{
			if (IsOutOfMap(new MapCoord(x, z))) return false;
			return Maps.GetCellInMap(mapName, new Vector2Int(x, z)) == type;
		}

		public static bool CellInAllMapsIs(Map.CellType cellT, Vector2Int pos)
		{
			if (IsOutOfMap(pos)) return false;
			return Maps.CellInAllMapsIs(cellT, pos);
		}

		/// <summary>
		/// Removes all cells of type cellT from the map with name mapName
		/// </summary>
		/// <param name="cellT"></param>
		/// <param name="mapName"></param>
		public static void RemoveCellTypeFromMap(CellType cellT, Maps.MapNames mapName)
		{
			for (int x = 0; x < MapData.MapSize[0]; x++)
				for (int y = 0; y < MapData.MapSize[1]; y++)
				{
					var cellPos = new MapCoord(x, y);
					if (Maps.GetCellInMap(mapName, cellPos) == cellT)
						Maps.ForceSetCell(mapName, cellPos, CellType.Empty);
				}
		}

		public delegate bool CheckIfDesiredCell(MapCoord nxtCellPos, CellType targetCellT,  
			Maps.MapNames mapName = Maps.MapNames.Invalid, List<CellType> ignoreTypes = null);

		public static MapCoord FindNearestCell(MapCoord startCellPos, CellType targetCellT,
			 CheckIfDesiredCell check, Maps.MapNames mapName, Func<List<MapCoord>, List<MapCoord>> DirSortFunc,
			 List<CellType> ignoreTypes = null)
		{
			var dirs = new List<MapCoord>() {
				new MapCoord(1, 0),
				new MapCoord(0, 1),
				new MapCoord(-1, 0),
				new MapCoord(0, -1),
				new MapCoord(1, 1),
				new MapCoord(1, -1),
				new MapCoord(-1, 1),
				new MapCoord(-1, -1),

			};
			dirs = DirSortFunc(dirs);   // Used for sufficient pathfinding.
										// Firtly tries dir which closest to target cell and after this
										// tries others dirs (for example when obsticle on the way)
			//LogList(dirs);
			HashSet<MapCoord> visited = new HashSet<MapCoord>() { startCellPos };
			Queue<MapCoord> queue = new Queue<MapCoord>();
			queue.Enqueue(startCellPos);

			while (queue.Count > 0)
			{
				var curCell = queue.Dequeue();

				foreach (var dir in dirs)
				{
					var nxtCellPos = curCell + dir;

					if (!IsOutOfMap(nxtCellPos) && visited.Add(nxtCellPos))
					{
						if (check(nxtCellPos, targetCellT, mapName, ignoreTypes)) 
							return nxtCellPos;
						queue.Enqueue(nxtCellPos);
					}
				}
			}
			return new MapCoord(MapData.MapSize[0], MapData.MapSize[1]);	// Out of map
		}

		#region Debug
		public static void LogList<T>(List<T> list)
		{
			Debug.Log("List______________________");
			foreach (var i in list)
			{
				Debug.Log(i);
			}
			Debug.Log("__________________________");

		}
		#endregion

		public static CellType GetCellType(MapCoord coord, Maps.MapNames mapName)
		{
			return !IsOutOfMap(coord) ? Maps.GetCellInMap(mapName, coord) : CellType.Error;
		}

		/// <summary>
		/// Fill the area on map (circular filling)
		/// </summary>
		/// <param name="centerCoords"> - coordinates of the center of the area that need to be filled</param>
		/// <param name="radius"> - translate world radius (radius, 0, 0) to map map radius (mapRadius, 0) with WorldToMap method</param>
		/// <param name="filling"> - cell type which will fill the area</param>
		public static void FillMapAreaCircle(MapCoord centerCoords, int radius, CellType filling, Maps.MapNames mapName)
		{
			ForceSetCell(centerCoords, filling, mapName);
			Queue<MapCoord> toFill = new Queue<MapCoord>();
			AddNearbyCellsToQueue(ref toFill, centerCoords);

			while (toFill.Count > 0)
			{
				var currCell = toFill.Dequeue();
				int dx = currCell.x - centerCoords.x;
				int dy = currCell.y - centerCoords.y;
				if (dx * dx + dy * dy > radius * radius) continue;

				if (!TrySetCell(currCell, filling, mapName))
					continue;
				AddNearbyCellsToQueue(ref toFill, currCell);
			}
		}




		public static void FillMapAreaSquare(MapCoord center, Vector2Int size, CellType filling, Maps.MapNames mapName)
		{	// all coordinates starts at (0,0), endCellPos can't be negative
			// as the positioning correctness should be checked beforehand
			var halfSize = size / 2;
			var startCellPos = center - halfSize; // min corner
			var endCellPos = center + halfSize;   // max corner

			for (int x = startCellPos.x; x < endCellPos.x; x++)
				for (int z = startCellPos.y; z < endCellPos.y; z++)
				{
					var mapPos = new MapCoord(x, z);
					Maps.ForceSetCell(mapName, mapPos, filling);
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

		public static Vector3 MapToWorld(MapCoord map_pos)
		{ return data.MapStart + new Vector3(map_pos.x, 0, map_pos.y); }

		public static Vector3 GetCellSize()
		{ return data.CellSize; }

		public static Vector2Int GetSize()
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
			else if (x >= MapData.MapSize[0]) { x = MapData.MapSize[0] - 1; }
			if (z < 0) { z = 0; }
			else if (z >= MapData.MapSize[1]) { z = MapData.MapSize[1] - 1; }

			return new MapCoord(x, z);
		}

	}
}
