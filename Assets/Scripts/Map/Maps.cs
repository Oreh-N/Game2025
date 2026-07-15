using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace MapSpace.MapLayers
{
	public static class Maps
	{
		public enum MapNames { BuildingMap, EnvironmentMap, UnitMap, Invalid = 505 }   // Corresponds to _Maps to access them correctly
		static Map.CellType[][,] _Maps = new Map.CellType[3][,];


		static Maps()
		{
			for (int i = 0; i < _Maps.Length; i++)
			{ _Maps[i] = new Map.CellType[MapData.MapSize[0], MapData.MapSize[1]]; }
		}

		/// <summary>
		/// Checks if cell on position pos have type cellT on all map layers
		/// </summary>
		/// <param name="goalCellT"></param>
		/// <param name="pos"></param>
		/// <returns>Returns false if there any layer have not cell type cellT on position pos</returns>
		public static bool CellInAllMapsIs(Map.CellType goalCellT, Vector2Int pos, List<Map.CellType> ignoreTypes = null)
		{
			if (!Map.IsOutOfMap(pos))
				for (int i = 0; i < _Maps.Length; i++)
				{
					var currMapCellT = _Maps[i][pos.x, pos.y];
					if (ignoreTypes != null)
					{
						foreach (var ignoreT in ignoreTypes)
						{ if (currMapCellT == ignoreT) continue; }
					}
					if (currMapCellT != goalCellT) return false;
				}
			return true;
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