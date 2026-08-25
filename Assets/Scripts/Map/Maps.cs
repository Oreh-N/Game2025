using System;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using UnityEngine;
using static MapSpace.MapLayers.Maps;
using static UnityEditor.PlayerSettings;

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
			//Debug.Log($"Type: {_Maps[(int)mapName][pos.x, pos.y]}\tTypeNum: {(int)(_Maps[(int)mapName][pos.x, pos.y])}\tTID: {GetCellTeamNum(_Maps[(int)mapName][pos.x, pos.y])}");
			return GetCellTeamNum(_Maps[(int)mapName][pos.x, pos.y]);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cellT"></param>
		/// <returns>Return only number without hundreds (team ID)</returns>
		static int GetTeamlessCellNum(Map.CellType cellT) { return (int)(cellT) % 100; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="cellT"></param>
		/// <returns>Returns only hundreds (team ID)</returns>
		static int GetCellTeamNum(Map.CellType cellT) { return (int)(cellT) / 100; }
		/// <summary>
		/// Takes Already existing basic cell type and replaces its hundreds with correcponding teamID
		/// </summary>
		/// <param name="objT"></param>
		/// <param name="teamID"></param>
		/// <returns></returns>
		public static Map.CellType CombineTeamCell(Map.CellType objT, int teamID) 
		{ return (Map.CellType)(GetTeamlessCellNum(objT) + teamID * 100); } 

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
		{ return _Maps[(int)name][pos.x, pos.y]; }

		/// <summary>
		/// No matter in what team current cell are the method will return original 
		/// number of corresponding object (basic cells start from 500)
		/// </summary>
		/// <param name="name"></param>
		/// <param name="pos"></param>
		/// <returns></returns>
		public static Map.CellType GetBasicCellInMap(MapNames name, Vector2Int pos)
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

		public static bool TrySetTeamCell(MapNames name, Vector2Int pos, Map.CellType newCellT, int teamID)
		{
			if (_Maps[(int)name][pos.x, pos.y] == Map.CellType.Empty)
			{
				_Maps[(int)name][pos.x, pos.y] = CombineTeamCell(newCellT, teamID);
				return true;
			}
			return false;
		}

		public static void ForceSetCell(MapNames name, Vector2Int pos, Map.CellType newCellT)
		{ _Maps[(int)name][pos.x, pos.y] = newCellT; }

		public static void ForceSetTeamCell(MapNames name, Vector2Int pos, Map.CellType newCellT, int teamID)
		{ _Maps[(int)name][pos.x, pos.y] = CombineTeamCell(newCellT, teamID); }
	}
}