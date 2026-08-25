using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapSpace
{
	using MNames = MapLayers.Maps.MapNames;

	public class Chunk
	{
		ChunkData data;


		public Chunk(Vector3 world_pos)
		{
			data.MapPos = GetChunkMapPos(world_pos);
			data.Trees = new List<GameObject>();
		}
		public Chunk(Vector2Int map_pos_)
		{
			data.MapPos = GetMapPos(map_pos_);
			data.Trees = new List<GameObject>();
		}

		public static Vector2Int GetSize() { return ChunkData.Size; }

		public static Vector2Int GetChunkMapPos(Vector3 world_pos)
		{
			var mapPos = Map.WorldToMapWithCut(world_pos);
			return GetMapPos(mapPos);
		}

		public static Vector2Int GetChunkMapPos(Vector2Int map_pos)
		{ return GetMapPos(map_pos); }

		public bool IsEnabled() { return data.IsEnabled; }

		/// <summary>
		/// Takes any map position and returns which chunk does it belong to
		/// </summary>
		/// <param name="mapPos"> - Any map position</param>
		/// <returns></returns>
		static Vector2Int GetMapPos(Vector2Int mapPos)
		{
			//Debug.Log($"GetMapPos (mapPos): {mapPos}");
			var fullX = mapPos.x / ChunkData.Size.x;
			var fullY = mapPos.y / ChunkData.Size.y;

			if (mapPos.x < 0) fullX = 0;
			else if (mapPos.x > Map.GetSize().x)
				fullX = Map.GetSize().x / ChunkData.Size.x;

			if (mapPos.y < 0) fullY = 0;
			else if (mapPos.y > Map.GetSize().y)
				fullY = Map.GetSize().y / ChunkData.Size.y;

			//Debug.Log($"X: {fullX}    Y: {fullY}");
			return new Vector2Int(fullX, fullY);
		}

		public void Enable()       
		{
			if (data.IsEnabled) return;

			if (!data.Initialized) Initialize();
			else
			{
				for (int i = 0; i < data.Trees.Count; i++)
				{ data.Trees[i].SetActive(true); }
			}
			data.IsEnabled = true;

			//Debug.Log($"Enabled {data.map_pos} chunk");
		}

		void Initialize()
		{
			for (int x = 0; x < ChunkData.Size.x; x++)
				for (int y = 0; y < ChunkData.Size.y; y++)
				{
					var cell_pos = new Vector2Int(data.MapPos.x * ChunkData.Size.x + x,
						data.MapPos.y * ChunkData.Size.y + y);

					if (Map.IsOutOfMap(cell_pos) ||
						Map.GetCellType(cell_pos, MNames.EnvMap) != ChunkData.TreeType)
						continue;

					var tree = Creator.CreateTree(Map.MapToWorld(cell_pos));
					data.Trees.Add(tree);
				}
			data.Initialized = true;
		}

		public void Disable()
		{
			if (!data.IsEnabled)
				return;

			for (int i = 0; i < data.Trees.Count; i++)
			{ data.Trees[i].SetActive(false); }
			data.IsEnabled = false;
			// Debug.Log("RemChunk");
		}

		/// <summary>
		/// Destroys all tree prefabs the chunk contains
		/// </summary>
		public void DeleteFilling()
		{
			for (int i = 0; i < data.Trees.Count; i++)
			{ GameObject.Destroy(data.Trees[i]); }
			data.IsEnabled = false;
			//Debug.Log($"Disabled {data.map_pos} chunk");
		}
	}
}
