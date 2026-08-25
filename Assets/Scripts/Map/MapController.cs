using MapSpace;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using Color = UnityEngine.Color;
using Map = MapSpace.Map;
using MNames = MapSpace.MapLayers.Maps.MapNames;



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
		data.Tilemap_ = FindFirstObjectByType<Tilemap>();
	}


	private void Update()
	{
		if (!data.AllowBuilding || !data.CurrBuilding || !MainController.Instance.Ready) return;

		CheckPlace(data.CurrBuilding);

		if (data.CurrBuilding && Input.GetMouseButtonDown(0))
		{
			if (CanBePlaced(data.CurrBuilding))
			{ PlaceBuilding(data.CurrBuilding, MainController.Instance.GetTeam(data.CurrBuilding.GetTeamID())); }
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


	# region Grid
	const int _areaPadding = 3;
	const int _startPadding = 1;

	public bool CanBePlaced(Building build)
	{
		if (!build || Map.IsOutOfMap(build.transform.position) || !MainController.Instance.Ready) return false;
		int teamID = build.GetTeamID();
		Vector3 center = MainController.Instance.GetTeam(teamID).GetCenter();
		float radius = MainController.Instance.GetTeam(teamID).GetBuildingRadius();
		var mapPos = Map.WorldToMap(build.transform.position);
		if (Vector3.Distance(build.transform.position, center) > radius ||
			!Map.SquareAreaInAllMapsIs(Map.CellType.Empty, mapPos, build.GetSize()))
		{ return false; }

		return true;
	}



	public void SpawnMovableBuild(GameObject build, int teamID)
	{
		if (data.CurrBuilding && !data.CurrBuilding.IsPlaced())
		{ UIManager.Instance.UpdateWarningPanel("Place or delete current building first"); return; }

		//if (!Player.Instance.Shop_.TryBuyItem(build.GetName(), Player.Instance.MainBuilding_))
		//{ return; }
		if (data.CurrBuilding != null) 
			RemoveCurrBuild();
		var b = SpawnBuilding(build, teamID, GetMouseWorldPos());
		b.AddComponent<Movable>();
		Building building = b.GetComponent<Building>();
		if (building != null)
			data.CurrBuilding = building;
		data.AllowBuilding = true;
	}

	public void RemoveCurrBuild()
	{
		if (data.CurrBuilding != null)
		{
			Destroy(data.CurrBuilding.GameObject());
			data.CurrBuilding = null;
		}
	}

	private GameObject SpawnBuilding(GameObject buildPrefab, int teamID, Vector3 pos)
	{
		var obj = Creator.CreateBuilding(buildPrefab, pos);
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

		if (MainController.groundPlane.Raycast(ray, out float distance))
		{ return ray.GetPoint(distance); }
		return Vector3.zero;
	}

	public void PlaceBuilding(Building b, Team t)
	{
		if (!t || !b) return;
		b.SetTeam(t.GetID());
		b.Construct();
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