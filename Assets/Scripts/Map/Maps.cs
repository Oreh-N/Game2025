using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using UnityEngine;

namespace MapSpace.MapLayers
{
	public static class Maps
	{
		public enum MapNames { EnvMap, ForestMap, Invalid = 505 }   // Corresponds to _Maps to access them correctly
		static Map.CellType[][,] _Maps = new Map.CellType[2][,];
		public static bool Ready { get; private set; } = false;


		static Maps()
		{
			for (int i = 0; i < _Maps.Length; i++)
			{ 
				_Maps[i] = new Map.CellType[MapData.MapSize[0], MapData.MapSize[1]];
				ResetMap((MapNames)i);
			}
			Ready = true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="mapName"></param>
		/// <param name="pos"></param>
		/// <returns>Returns team's ID or higher number for basic cells (starting from 4)</returns>
		public static int GetCellTeamID(MapNames mapName, Vector2Int pos)
		{
			var cellT = (int)(_Maps[(int)mapName][pos.x, pos.y]);
			int teamID = cellT / 100;
			return teamID;
		}

		public static void ResetMap(MapNames mapName)
		{
			for (int x = 0; x < MapData.MapSize[0]; x++)
				for (int y = 0; y < MapData.MapSize[1]; y++)
				{ _Maps[(int)mapName][x, y] = Map.CellType.Empty; }
		}

		public static void CleanCell(MapNames mapName, Vector2Int pos)
		{ _Maps[(int)mapName][pos.x, pos.y] = Map.CellType.Empty; }

		/// <summary>
		/// Checks if cell on position pos have type cellT on all map layers
		/// </summary>
		/// <param name="goalCellT"></param>
		/// <param name="pos"></param>
		/// <returns>Returns false if there any layer have not cell type cellT on position pos</returns>
		public static bool CellInAllMapsIs(Map.CellType goalCellT, Vector2Int pos, List<Map.CellType> ignoreTypes = null)
		{
			bool Ignore(Map.CellType cellT)
			{
				foreach (var ignoreT in ignoreTypes)
				{ if (cellT == ignoreT) return true; }
				return false;
			}
			// TO FIX
			if (!Map.IsOutOfMap(pos))
				for (int i = 0; i < _Maps.Length; i++)
				{
					var currMapCellT = _Maps[i][pos.x, pos.y];

					if (currMapCellT != goalCellT)
					{
						if (ignoreTypes != null && Ignore(currMapCellT))
						{ continue; }
						return false;
					}
				}
			return true;
		}

		public static Map.CellType GetCellInMap(MapNames name, Vector2Int pos)
		{
			return _Maps[(int)name][pos.x, pos.y];
		}

		public static Map.CellType GetTeamlessCellInMap(MapNames name, Vector2Int pos)
		{
			int teamlessCell = ((int)_Maps[(int)name][pos.x, pos.y]) % 100 + 500; // basic cells use 5 hundreds
			return (Map.CellType)teamlessCell;
		}

		public static bool TrySetCell(MapNames name, Vector2Int pos, Map.CellType newCellT)
		{
			if (_Maps[(int)name][pos.x, pos.y] == Map.CellType.Empty)
			{
				_Maps[(int)name][pos.x, pos.y] = newCellT;
				return true;
			}
			return false;
		}

		public static void ForceSetCell(MapNames name, Vector2Int pos, Map.CellType newCellT)
		{
			_Maps[(int)name][pos.x, pos.y] = newCellT;
		}
	}
}