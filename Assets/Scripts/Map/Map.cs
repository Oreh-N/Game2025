using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using static MapSpace.Map;
using Color = UnityEngine.Color;
using MapCoord = UnityEngine.Vector2Int;


namespace MapSpace
{
	public static class Map
	{
		/* There might be MAXIMUM 99 types of cells!!! Hundreds used for certain team assignment!!! There are maximum 4 teams
		 ..0 - .29 Building types, .30 - .59 Unit types
		 Team assignment: Player (ID 0) cells start with 0 hundreds (15, 33, 5, etc.),
		 team with ID 1 cells start with 1 hundreds (115, 133, 105, etc.) and so on.
		 Everything that starts from 5 hundreds is basic cells
		*/
		public enum CellType
		{
			MainBuild = 500,
			Spawner = 501,
			Warehouse = 502,
			BasicTower = 503,

			GuardUnit = 530,
			HealerUnit = 531,
			MageUnit = 532,
			MeleeUnit = 533,
			MiningUnit = 534,
			SpyUnit = 535,
			WorkerUnit = 536,

			Empty = 560,
			Building = 561,
			Unit = 562,
			Tree = 563,
			Road = 564,
			BuildArea = 565,
			Error = 600
		}
		public enum MapNames { EnvMap, Invalid = 505 }   // Corresponds to _Maps to access them correctly

		static MapData data = new MapData();


		static Map()
		{
			for (int i = 0; i < MapData.Layers.Length; i++)
			{
				MapData.Layers[i] = new CellType[MapData.MapSize[0], MapData.MapSize[1]];
				ResetMap((MapNames)i);
			}
			MapData.Ready = true;
		}


		/// <summary>
		/// Removes all cells of type cellT from the map with name mapName
		/// </summary>
		/// <param name="cells"></param>
		/// <param name="mapName"></param>
		public static void RemoveCellTypeFromMap(MapNames mapName, List<CellType> cells)
		{
			for (int x = 0; x < MapData.MapSize[0]; x++)
				for (int y = 0; y < MapData.MapSize[1]; y++)
				{
					var cellPos = new MapCoord(x, y);
					if (cells.Contains(GetCellType(mapName, cellPos)))
						ForceSetCell(mapName, cellPos, CellType.Empty);
				}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapName"></param>
		/// <param name="pos"></param>
		/// <returns>Returns team's ID or higher number for basic cells (starting from 4)</returns>
		public static int GetCellTeamID(MapNames mapName, MapCoord pos)
		{
			//Debug.Log($"Type: {_Maps[(int)mapName][pos.x, pos.y]}\tTypeNum: {(int)(_Maps[(int)mapName][pos.x, pos.y])}\tTID: {GetCellTeamNum(_Maps[(int)mapName][pos.x, pos.y])}");
			return GetCellTeamNum(MapData.Layers[(int)mapName][pos.x, pos.y]);
		}

		public static bool IsBuilding(MapCoord pos)
		{
			int num = (int)GetBasicCellInMap(MapNames.EnvMap, pos);
			Debug.Log($"Cell num (building check): {num}");
			if (500 <= num && num < 530) return true;
			return false;
		}
		public static bool IsUnit(MapCoord pos)
		{
			int num = (int)GetBasicCellInMap(MapNames.EnvMap, pos);
			Debug.Log($"Cell num (unit check): {num}");

			if (530 <= num && num < 560) return true;
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cellT"></param>
		/// <returns>Return only number without hundreds (team ID)</returns>
		static int GetTeamlessCellNum(CellType cellT) { return (int)(cellT) % 100; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cellT"></param>
		/// <returns>Returns only hundreds (team ID)</returns>
		static int GetCellTeamNum(CellType cellT) { return (int)(cellT) / 100; }
		/// <summary>
		/// Takes Already existing basic cell type and replaces its hundreds with correcponding teamID
		/// </summary>
		/// <param name="objT"></param>
		/// <param name="teamID"></param>
		/// <returns></returns>
		public static CellType CombineTeamCell(CellType objT, int teamID)
		{ return (CellType)(GetTeamlessCellNum(objT) + teamID * 100); }

		public static void ResetMap(MapNames mapName)
		{
			for (int x = 0; x < MapData.MapSize[0]; x++)
				for (int y = 0; y < MapData.MapSize[1]; y++)
				{ MapData.Layers[(int)mapName][x, y] = Map.CellType.Empty; }
		}

		public static void CleanCell(MapNames mapName, MapCoord pos)
		{ MapData.Layers[(int)mapName][pos.x, pos.y] = CellType.Empty; }

		/// <summary>
		/// Checks if cell on position pos have type cellT on all map layers
		/// </summary>
		/// <param name="goalCellT"></param>
		/// <param name="pos"></param>
		/// <returns>Returns false if there any layer have not cell type cellT on position pos</returns>
		public static bool CellInAllMapsIs(CellType goalCellT, MapCoord pos, List<CellType> ignoreTypes = null)
		{
			bool Ignore(CellType cellT)
			{
				foreach (var ignoreT in ignoreTypes)
				{ if (cellT == ignoreT) return true; }
				return false;
			}
			if (!IsOutOfMap(pos))
				for (int i = 0; i < MapData.Layers.Length; i++)
				{
					var currMapCellT = MapData.Layers[i][pos.x, pos.y];

					if (currMapCellT != goalCellT)
					{
						if (ignoreTypes != null && Ignore(currMapCellT))
						{ continue; }
						return false;
					}
				}
			return true;
		}



		public static CellType GetCellType(MapNames name, MapCoord pos)
		{ return !IsOutOfMap(pos) ? MapData.Layers[(int)name][pos.x, pos.y] : CellType.Error; }

		/// <summary>
		/// No matter in what team current cell are the method will return original 
		/// number of corresponding object (basic cells start from 500)
		/// </summary>
		/// <param name="name"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public static CellType GetBasicCellInMap(MapNames name, MapCoord pos)
		{
			int teamlessCell = ((int)MapData.Layers[(int)name][pos.x, pos.y]) % 100 + 500; // basic cells use 5 hundreds
			return (CellType)teamlessCell;
		}

		public static bool TrySetCell(MapNames name, MapCoord pos, CellType newCellT)
		{
			if (IsOutOfMap(pos) || GetCellType(name, pos) != CellType.Empty)
				return false;
			if (MapData.Layers[(int)name][pos.x, pos.y] == CellType.Empty)
			{
				MapData.Layers[(int)name][pos.x, pos.y] = newCellT;
				return true;
			}
			return false;
		}

		public static bool TrySetTeamCell(MapNames name, MapCoord pos, CellType newCellT, int teamID)
		{
			if (MapData.Layers[(int)name][pos.x, pos.y] == CellType.Empty)
			{
				MapData.Layers[(int)name][pos.x, pos.y] = CombineTeamCell(newCellT, teamID);
				return true;
			}
			return false;
		}

		public static void ForceSetCell(MapNames name, MapCoord pos, CellType newCellT)
		{ MapData.Layers[(int)name][pos.x, pos.y] = newCellT; }

		public static void ForceSetTeamCell(MapNames name, MapCoord pos, CellType newCellT, int teamID)
		{ MapData.Layers[(int)name][pos.x, pos.y] = CombineTeamCell(newCellT, teamID); }

		public static bool Ready() { return MapData.Ready; }








		public static bool CellIs(CellType type, int x, int z, MapNames mapName)
		{
			if (IsOutOfMap(new MapCoord(x, z))) return false;
			return GetCellType(mapName, new MapCoord(x, z)) == type;
		}

		public static bool SquareAreaInAllMapsIs(CellType cellT, MapCoord pos, MapCoord size)
		{
			var points = GetSquareAreaPoints(pos, size);
			foreach (var p in points)
			{
				if (IsOutOfMap(p) || !CellInAllMapsIs(cellT, p))
					return false;
			}
			return true;
		}

		public delegate bool CheckIfDesiredCell(MapCoord nxtCellPos, CellType targetCellT,
			MapNames mapName = MapNames.Invalid, List<CellType> ignoreTypes = null);

		public static MapCoord FindNearestCell(MapCoord startCellPos, CellType targetCellT,
			 CheckIfDesiredCell check, MapNames mapName, Func<List<MapCoord>, List<MapCoord>> DirSortFunc,
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
			return new MapCoord(MapData.MapSize[0], MapData.MapSize[1]);    // Out of map
		}

		/// <summary>
		/// Looks for nearest non-empty cell in the specific radius
		/// </summary>
		/// <param name="centerCoords"></param>
		/// <param name="radius"></param>
		/// <returns></returns>
		public static MapCoord FindNearestCell(MapCoord centerCoords, int radius, MapNames mapName)
		{
			Queue<MapCoord> queue = new Queue<MapCoord>();
			List<MapCoord> visited = new List<MapCoord>();
			AddNearbyCellsToQueue(ref queue, centerCoords);

			while (queue.Count > 0)
			{
				var currCell = queue.Dequeue();
				visited.Add(currCell);
				int dx = currCell.x - centerCoords.x;
				int dy = currCell.y - centerCoords.y;
				if (dx * dx + dy * dy > radius * radius) continue;

				if (GetCellType(mapName, currCell) != CellType.Empty)
					return currCell;

				AddNearbyCellsToQueue(ref queue, currCell, visited);
			}
			return centerCoords;
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


		/// <summary>
		/// Fill the area on map (circular filling)
		/// </summary>
		/// <param name="centerCoords"> - coordinates of the center of the area that need to be filled</param>
		/// <param name="radius"> - translate world radius (radius, 0, 0) to map map radius (mapRadius, 0) with WorldToMap method</param>
		/// <param name="filling"> - cell type which will fill the area</param>
		public static void FillMapAreaCircle(MapCoord centerCoords, int radius, CellType filling, MapNames mapName)
		{
			ForceSetCell(mapName, centerCoords, filling);
			Queue<MapCoord> toFill = new Queue<MapCoord>();
			AddNearbyCellsToQueue(ref toFill, centerCoords);

			while (toFill.Count > 0)
			{
				var currCell = toFill.Dequeue();
				int dx = currCell.x - centerCoords.x;
				int dy = currCell.y - centerCoords.y;
				if (dx * dx + dy * dy > radius * radius) continue;

				if (!TrySetCell(mapName, currCell, filling))
					continue;
				AddNearbyCellsToQueue(ref toFill, currCell);
			}
		}

		public static List<MapCoord> GetSquareAreaPoints(MapCoord center, Vector2Int size)
		{   // all coordinates starts at (0,0), endCellPos can't be negative
			// as the positioning correctness should be checked beforehand
			List<MapCoord> points = new List<MapCoord>();
			var halfSize = size / 2;
			var startCellPos = center - halfSize; // min corner
			var endCellPos = center + halfSize;   // max corner

			for (int x = startCellPos.x; x < endCellPos.x; x++)
				for (int z = startCellPos.y; z < endCellPos.y; z++)
				{ points.Add(new MapCoord(x, z)); }
			return points;
		}


		public static void FillMapAreaSquare(MapCoord center, Vector2Int size, CellType filling, MapNames mapName)
		{
			var points = GetSquareAreaPoints(center, size);

			foreach (var p in points)
			{ ForceSetCell(mapName, p, filling); }
		}


		private static void AddNearbyCellsToQueue(ref Queue<MapCoord> queue, MapCoord currCoords, List<MapCoord> visited = null)
		{
			List<int> range = new List<int>() { -1, 0, 1 };
			foreach (int i in range)
			{
				foreach (int j in range)
				{
					if (i == 0 && j == 0) continue;
					MapCoord newCoords = new MapCoord(currCoords.x + i, currCoords.y + j);
					if (IsOutOfMap(newCoords) || (visited != null && visited.Contains(newCoords))) continue;
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
