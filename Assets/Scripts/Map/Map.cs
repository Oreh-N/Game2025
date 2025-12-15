using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;

public class Map : MonoBehaviour
{
	public static Map Instance;
	MapData data = new MapData();


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	/// <summary>
	/// Convert map index to world position (vertex of the cell)
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <returns>World position</returns>
	public Vector3 MapToWorld(int x, int y)
	{
		return data.MapStart + new Vector3(x, 0, y);
	}


	/// <summary>
	/// Convert world position to map index
	/// </summary>
	/// <param name="pos"></param>
	/// <returns>Map position (indicies)</returns>
	public Vector2Int WorldToMap(Vector3 pos)
	{
		int x = Mathf.FloorToInt(pos.x - data.MapStart.x);
		int z = Mathf.FloorToInt(pos.z - data.MapStart.z);


		if (x >= data.MapSize.x || z >= data.MapSize.y || x < 0 || z < 0)
		{
			Debug.Log("Out of range (WorldToMap)");
			return new Vector2Int(0, 0);
		}

		return new Vector2Int(x, z);
	}

	#region
	public bool showGrid;
	public GameObject targetForGizmo;

	private void OnDrawGizmos()
	{
		if (showGrid)
		{
			Gizmos.color = new Color(0.8f, 0, 0, 0.3f);
			for (int x = 0; x < data.MapSize.x; x++)
				for (int z = 0; z < data.MapSize.y; z++)
				{
					Gizmos.DrawCube(MapToWorld(x, z), data.CellSize);
				}
		}
		
		IPlaceableOnMap comp;

		if (targetForGizmo && targetForGizmo.TryGetComponent<IPlaceableOnMap>(out comp))
		{
			Gizmos.color = new Color(1f, 0.4f, 1f, 0.5f);
			Vector2Int worldPos = WorldToMap(comp.GetPos());
			var sizeOnMap = (comp.GetTakeAreaSize());
			var size = new Vector3(sizeOnMap.x, 0, sizeOnMap.y);
			Gizmos.DrawCube(MapToWorld(worldPos.x, worldPos.y), size);
		}
	}
	void ShowMapGizmo()
	{
		Gizmos.color = new Color(0.7f, 0, 0.4f, 0.3f);

		for (int x = 0; x < data.MapSize.x; x++)
			for (int y = 0; y < data.MapSize.y; y++)
			{
				if (data.Map[x, y] == 0) continue;   // will through an error if used not during the game

				Vector3 world = MapToWorld(x, y);
				Gizmos.DrawCube(world, data.CellSize);
			}
	}
	#endregion
}

