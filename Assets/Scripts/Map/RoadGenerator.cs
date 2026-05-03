using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Map;
using static UnityEditor.PlayerSettings;

/// <summary>
/// Simple road generator. Doesn't see obstacles. Not optimized. 
/// Call only at the very beggining of the game, because taking too much time.
/// </summary>
public static class RoadGenerator
{
	const int roadWidth = 30;


	/// <summary>
	/// Generates roads from each base to the center of the map.
	/// </summary>
	public static void GenRoadsBetweenAllTeams(Map map, Vector2Int map_size)
	{
		if (!MainController.Instance.Ready) return;
		var mapCenter = new Vector2Int(map_size.x / 2, map_size.y / 2);
		Team[] ts = MainController.Instance.GetAllTeams();
		for (int i = 0; i < ts.Length; i++)
		{
			if (ts[i] == null) continue;
			var baseCenter = new Vector2Int((int)ts[i].GetCenter().x, (int)ts[i].GetCenter().z);
			SignRoadOnMap(baseCenter, mapCenter, map);
		}
	}

	/// <summary>
	/// Assignes road on map from start to end
	/// </summary>
	/// <param name="start"> - start coordinates on map</param>
	/// <param name="end"> - end coordinates on map</param>
	static void SignRoadOnMap(Vector2Int start, Vector2Int end, Map map)
	{
		List<Vector2Int> road = GetShortestRoad(start, end, map);

		foreach (var part in road)
		{
			Vector2Int dir = new Vector2Int(
				Math.Sign(end.x - part.x),
				Math.Sign(end.y - part.y)
			);
			Vector2Int perpDir = new Vector2Int(-dir.y, dir.x);
			WidenRoad(part, roadWidth, perpDir, map);
		}
	}

	static Vector2Int GetClosestDir(Map map, Vector2Int curr, Vector2Int goal)
	{
		Vector2Int closest_dir = new Vector2Int(0, 0);

		for (int x = -1; x <= 1; x++)
			for (int y = -1; y <= 1; y++)
			{
				var dir = new Vector2Int(x, y);
				if (Vector2Int.Distance(curr + dir, goal) <
					Vector2Int.Distance(curr + closest_dir, goal))
				{ closest_dir = dir; }
			}

		return closest_dir;
	}

	static List<Vector2Int> GetShortestRoad(Vector2Int start, Vector2Int end, Map map)
	{
		List<Vector2Int> road = new List<Vector2Int>();
		var curr = start;
		while (!is_close(curr, end, roadWidth/2))
		{
			Vector2Int dir = GetClosestDir(map, curr, end);
			curr = curr + dir;
			road.Add(curr);
		}
		return road;
	}

	static bool is_close(Vector2Int curr, Vector2Int goal, int goal_radius = 50)
	{
		return Vector2Int.Distance(curr, goal) < goal_radius;
	}

	static void WidenRoad(Vector2Int center, int width, Vector2Int perpDir, Map map)
	{
		int r = width / 2;
		
		for (int x = -r; x <= r; x++)
			for (int y = -r; y <= r; y++)
			{
				if (x * x + y * y > r * r)
					continue;
				SetRoadOnPosition(new Vector2Int(center.x + x, center.y + y), map);
			}
	}

	static public void SetRoadOnPosition(Vector2Int coord, Map map)
	{
		if (map.IsOutOfMap(coord)) return;

		map.TrySetCell(coord, CellType.Road);
	}
}
