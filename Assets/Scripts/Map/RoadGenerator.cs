using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static Map;

public static class RoadGenerator
{
	const int roadWidth = 30;


	// Lets assume that there will be max 4 teams
	/// <summary>
	/// If two teams are close to each other the road will be generated directly between them,
	/// if they are far from each other, then roads will be generated to the map center from 
	/// centeres of each team. The roads can only be generated over empty cells.
	/// </summary>
	public static void GenRoadsBetweenAllTeams(Map map, Vector2Int map_size)
	{
		Team[] ts = MainController.Instance.GetAllTeams();
		for (int i = 0; i < ts.Length; i++)
			for (int j = i + 1; j < ts.Length; j++)
			{
				GenRoadBetween(
					map.WorldToMap(ts[i].GetCenter()),
					map.WorldToMap(ts[j].GetCenter()), 
					map,
					map_size
				);
			}
	}

	/// <summary>
	/// Generates roads on map between two positions
	/// </summary>
	/// <param name="pos1"> - first on map position</param>
	/// <param name="pos2"> - second on map position</param>
	static void GenRoadBetween(Vector2Int pos1, Vector2Int pos2, Map map, Vector2Int map_size)
	{
		float dist = Vector2Int.Distance(pos1, pos2);
		int center_x = map_size[0] / 2;
		int center_y = map_size[1] / 2;

		if (dist < Vector2Int.Distance(pos1, new Vector2Int(center_x, center_y)))
		{ SignRoadOnMap(pos1, pos2, map); }
		else
		{
			Vector2Int mapCenter = new Vector2Int(center_x, center_y);
			SignRoadOnMap(pos1, mapCenter, map);
			SignRoadOnMap(pos2, mapCenter, map);
		}
	}

	/// <summary>
	/// Assignes road on map from start to end
	/// </summary>
	/// <param name="start"> - start coordinates on map</param>
	/// <param name="end"> - end coordinates on map</param>
	static void SignRoadOnMap(Vector2Int start, Vector2Int end, Map map)
	{
		Vector2Int dir = new Vector2Int(
			Math.Sign(end.x - start.x),
			Math.Sign(end.y - start.y)
		);

		Vector2Int perpDir = new Vector2Int(-dir.y, dir.x);
		int max_step = (map.GetSize()[0] + map.GetSize()[1]) / 2;

		if (dir != new Vector2Int(0, 0))
			while (!is_close(start, end) && max_step > 0)
			{
				GenRoad(start, perpDir, roadWidth, map);
				start += dir;
				max_step--;
			}
	}

	static bool is_close(Vector2Int curr, Vector2Int goal)
	{
		return Vector2Int.Distance(curr, goal) < 50;
	}

	static public void GenRoad(Vector2Int start, Vector2Int end, int width, Map map)
	{
		int x0 = start.x;
		int y0 = start.y;
		int x1 = end.x;
		int y1 = end.y;

		int dx = Mathf.Abs(x1 - x0);
		int dy = Mathf.Abs(y1 - y0);

		int sx = x0 < x1 ? 1 : -1;
		int sy = y0 < y1 ? 1 : -1;

		int err = dx - dy;

		while (true)
		{
			GenRoadOnMap(new Vector2Int(x0, y0), width, map);

			if (x0 == x1 && y0 == y1)
				break;

			int e2 = 2 * err;

			if (e2 > -dy)
			{
				err -= dy;
				x0 += sx;
			}

			if (e2 < dx)
			{
				err += dx;
				y0 += sy;
			}
		}
	}


	static void GenRoadOnMap(Vector2Int center, int width, Map map)
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
