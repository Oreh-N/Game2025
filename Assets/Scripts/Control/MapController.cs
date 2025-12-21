using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
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
	}

	private void Update()
	{

		if (!data.AllowBuilding || data.CurrBuilding == null) return;

		CheckPlace();

		if (Input.GetMouseButtonDown(1))
		{
			if (CanBePlaced(data.CurrBuilding))
			{ PlaceBuilding(); }
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
	private GameObject SpawnBuildOnPos(GameObject build, int teamID, Vector3 pos)
	{
		var obj = SpawnBuilding(build, teamID, pos);
		TakeAreaForCurrBuild();
		return obj;
	}

	// Grid____________________________________________________________
	const int _areaPadding = 3;
	const int _startPadding = 1;

	public bool CanBePlaced(Building build)
	{
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
				if (data.Tilemap_.GetTile(data.MapGrid.WorldToCell(currPos)) == data.BusyTile)
				{ return false; }
			}
		}
		return true;
	}

	private void TakeAreaForCurrBuild()
	{
		Vector3Int start = GetAreaStartPos(data.CurrBuilding);
		Vector2 size = data.CurrBuilding.GetSize();

		for (int x = 0; x < size.x + _areaPadding; x++)
		{
			for (int y = 0; y < size.y + _areaPadding; y++)
			{
				var currPos = new Vector3Int(start.x + x, y: 0, start.z + y);
				data.Tilemap_.SetTile(data.MapGrid.WorldToCell(currPos), data.BusyTile);
			}
		}
	}

	private Vector3Int GetAreaStartPos(Building build)
	{
		Vector2 size = data.CurrBuilding.GetSize();
		Vector2Int sizeInt = new Vector2Int((int)size.x, (int)size.y);
		var size3 = new Vector3Int(sizeInt.x, 0, sizeInt.y);
		var center = build.transform.position;
		var start = new Vector3(center.x - (size3.x / 2) - _startPadding, center.y,
								center.z - (size3.z / 2) - _startPadding);
		return new Vector3Int(Mathf.RoundToInt(start.x),
							Mathf.RoundToInt(start.y),
							Mathf.RoundToInt(start.z));
	}

	public void SpawnMovableBuild(Building build, int teamID)
	{
		if (data.CurrBuilding != null && !data.CurrBuilding.IsPlaced())
		{ UIManager.Instance.UpdateWarningPanel("Place or delete current building first"); return; }

		//if (!Player.Instance.Shop_.TryBuyItem(build.GetName(), Player.Instance.MainBuilding_))
		//{ return; }

		var obj = SpawnBuilding(build.gameObject, teamID, MapCoordToGrid(GetMouseWorldPos()));
		obj.AddComponent<Movable>();

		//data.Childrens_rends = data.CurrBuilding.GetComponentsInChildren<Renderer>();
		data.AllowBuilding = true;
	}

	private GameObject SpawnBuilding(GameObject build, int teamID, Vector3 pos)
	{
		GameObject obj = Instantiate(build.gameObject, pos, build.transform.rotation);
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

	public void PlaceBuilding()
	{
		BuildingManager.Instance.ColorCurrBuilding(Color.white);
		data.CurrBuilding.Construct();
		TakeAreaForCurrBuild();
		data.AllowBuilding = false;
	}

	private void CheckPlace()
	{
		if (!CanBePlaced(data.CurrBuilding))
		{
			UIManager.Instance.ChangeCursor(false);
			BuildingManager.Instance.ColorCurrBuilding(Color.red);
		}
		else if (CanBePlaced(data.CurrBuilding))
		{
			UIManager.Instance.ChangeCursor(true);
			BuildingManager.Instance.ColorCurrBuilding(Color.green);
		}
	}


}