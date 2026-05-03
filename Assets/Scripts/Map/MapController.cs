using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Color = UnityEngine.Color;

public class MapController : MonoBehaviour {
	public static MapController Instance;
	MapControllerData data = new MapControllerData();

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

		data.CurrBuilding = null;
		data.MapGrid = FindFirstObjectByType<Grid>();
		data.Tilemap_ = FindFirstObjectByType<Tilemap>();
	}

	private void Update()
	{
		if (!data.AllowBuilding || !data.CurrBuilding) return;

		CheckPlace(data.CurrBuilding);

		if (data.CurrBuilding && Input.GetMouseButtonDown(0))
		{
			if (CanBePlaced(data.CurrBuilding))
			{ PlaceBuilding(data.CurrBuilding); }
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			UIManager.Instance.ChangeCursor(true);
			int returnPrice = Player.Instance.data.Shop_.GetItemPrice(data.CurrBuilding.GetName());
			if (returnPrice > 0)
			{ /*Player.Instance.data.MainBuilding_.Earn(returnPrice);*/ }
			Destroy(data.CurrBuilding.gameObject);
		}
	}


	/// <summary>
	/// Maps coordinates to the grid
	/// </summary>
	/// <param name="worldPos">Real coordinates (will be transformed to the grid coordinates)</param>
	/// <returns></returns>
	public Vector3 MapCoordToGrid(Vector3 worldPos)
	{
		Vector3Int cellPos = data.MapGrid.WorldToCell(worldPos);
		worldPos = data.MapGrid.GetCellCenterWorld(cellPos);
		return worldPos;
	}


	# region Grid
	const int _areaPadding = 3;
	const int _startPadding = 1;

	public bool CanBePlaced(Building build)
	{
		if (!build || Map.Instance.IsOutOfMap(build.transform.position)) return false;
		int teamID = build.GetTeamID();
		Vector3 center = MainController.Instance.GetTeam(teamID).GetCenter();
		float radius = MainController.Instance.GetTeam(teamID).GetBuildingRadius();
		if (Vector3.Distance(build.transform.position, center) > radius)
		{ return false; }

		Vector3Int startInt = GetAreaStartPos(build);
		Vector2 size = build.GetSize();
		for (int x = 0; x < size.x + _areaPadding; x++)
		{
			for (int y = 0; y < size.y + _areaPadding; y++)
			{
				var currPos = new Vector3Int(startInt.x + x, y: 0, startInt.z + y);
				if (!Map.Instance.CellIs(Map.CellType.BuildArea, Map.Instance.WorldToMap(currPos)))
				{ return false; }
			}
		}
		return true;
	}

	private void TakeAreaForCurrBuild()
	{
		if (!data.CurrBuilding) return;
		Vector3Int start = GetAreaStartPos(data.CurrBuilding);
		Vector2 size = data.CurrBuilding.GetSize();

		for (int x = 0; x < size.x + _areaPadding; x++)
		{
			for (int y = 0; y < size.y + _areaPadding; y++)
			{
				var currPos = new Vector3Int(start.x + x, y: 0, start.z + y);
				Map.Instance.ForceSetCell(Map.Instance.WorldToMap(currPos), Map.CellType.Building);
			}
		}
	}

	private Vector3Int GetAreaStartPos(Building build)
	{
		Vector2 size = build.GetSize();
		Vector2Int sizeInt = new Vector2Int((int)size.x, (int)size.y);
		var size3 = new Vector3Int(sizeInt.x, 0, sizeInt.y);
		var center = build.transform.position;
		var start = new Vector3(center.x - (size3.x / 2) - _startPadding, center.y,
								center.z - (size3.z / 2) - _startPadding);
		return new Vector3Int(Mathf.RoundToInt(start.x),
							Mathf.RoundToInt(start.y),
							Mathf.RoundToInt(start.z));
	}

	public void SpawnPlayerMovableBuild(Building build)
	{
		SpawnMovableBuild(build, Player.Instance.GetID());
		Debug.Log($"Recieve: {build.GetSize()}");
	}

	public void SpawnMovableBuild(Building build, int teamID)
	{
		if (data.CurrBuilding && !data.CurrBuilding.IsPlaced())
		{ UIManager.Instance.UpdateWarningPanel("Place or delete current building first"); return; }

		//if (!Player.Instance.Shop_.TryBuyItem(build.GetName(), Player.Instance.MainBuilding_))
		//{ return; }
		if (data.CurrBuilding != null) 
			RemoveMovableBuild();
		var b = SpawnBuilding(build, teamID, MapCoordToGrid(GetMouseWorldPos()));
		b.AddComponent<Movable>();
		data.CurrBuilding = b;
		data.AllowBuilding = true;
	}

	public void RemoveMovableBuild()
	{
		if (data.CurrBuilding != null)
		{
			Destroy(data.CurrBuilding.GameObject());
			data.CurrBuilding = null;
		}
	}

	private Building SpawnBuilding(Building build, int teamID, Vector3 pos)
	{
		var obj = Instantiate(build, pos, build.transform.rotation);
		data.CurrBuilding = obj.GetComponent<Building>();
		((ITeamMember)data.CurrBuilding).SetTeam(teamID);

		return obj;
	}

	/// <summary>
	/// Get position of the mouse cursor on the world landscape
	/// </summary>
	/// <returns></returns>
	public static Vector3 GetMouseWorldPos()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
			return hit.point;
		return Vector3.zero;
	}

	public void PlaceBuilding(Building b)
	{
		var t = MainController.Instance.GetTeam(b.GetTeamID());
		if (!t) return;
		Color c = t.GetColor();
		BuildingManager.ColorCurrBuilding(b, c);
		b.Construct();
		TakeAreaForCurrBuild();
		data.CurrBuilding = null;
		data.AllowBuilding = false;
	}

	private void CheckPlace(Building b)
	{
		if (!CanBePlaced(b))
		{
			b.GetSize();
			UIManager.Instance.ChangeCursor(false);
			BuildingManager.ColorCurrBuilding(b, Color.red);
		}
		else
		{
			UIManager.Instance.ChangeCursor(true);
			BuildingManager.ColorCurrBuilding(b, Color.green);
		}
	}
	#endregion

}