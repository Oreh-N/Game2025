using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace MapSpace.MapLayers
{
	public static class Maps
	{
		public enum MapNames { BuildingMap, EnvironmentMap, UnitMap }   // Corresponds to _Maps to access them correctly
		static Map.CellType[][,] _Maps = new Map.CellType[3][,];


		static Maps()
		{
			for (int i = 0; i < _Maps.Length; i++)
			{ _Maps[i] = new Map.CellType[MapData.MapSize[0], MapData.MapSize[1]]; }
		}

		public static bool IsInMaps(Map.CellType cellT, Vector2Int pos)
		{
			if (!Map.IsOutOfMap(pos))
				for (int i = 0; i < _Maps.Length; i++)
				{
					if (_Maps[i][pos.x, pos.y] == cellT) return true;
				}
			return false;
		}

		public static Map.CellType GetCellInMap(MapNames name, Vector2Int pos)
		{
			return _Maps[(int)name][pos.x, pos.y];
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