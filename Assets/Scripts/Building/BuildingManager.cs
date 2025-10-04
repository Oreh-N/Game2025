using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Unity.VisualScripting;
using System;


public class BuildingManager : MonoBehaviour
{
	public static BuildingManager Instance;

	public static Tilemap Tilemap_ { get; private set; }
	public Building CurrBuilding { get; private set; }
	public static Grid Grid_ { get; private set; }

	[SerializeField] Texture2D _declineCursor;
	[SerializeField] Texture2D _defaultCursor;
	[SerializeField] LayerMask _obstacles;
	[SerializeField] TileBase _busyTile;

	bool _is_default_cursor = true;
	bool _allowBuilding = false;
	Renderer[] _childrens_rends;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

		Grid_ = FindObjectOfType<Grid>();
		Tilemap_ = FindObjectOfType<Tilemap>();
	}


	void Update()
	{
		if (!_allowBuilding || CurrBuilding == null) return;

		CheckPlace();

		if (Input.GetMouseButtonDown(1))
		{
			if (CanBePlaced(CurrBuilding))
			{ PlaceBuilding(); }
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			ChangeCursor(_defaultCursor, true);
			int returnPrice = Player.Instance.Shop_.GetItemPrice(CurrBuilding.Name);
			if (returnPrice > 0)
			{ Player.Instance.MainBuilding_.Earn(returnPrice); }
			Destroy(CurrBuilding.gameObject);
		}
	}

	public void PlaceBuilding()
	{
		ColorCurrBuilding(Color.white);
		CurrBuilding.Construct();
		TakeAreaForCurrBuild();
		_allowBuilding = false;
	}

	private void CheckPlace()
	{
		if (!CanBePlaced(CurrBuilding) && _is_default_cursor)
		{
			ChangeCursor(_declineCursor, false);
			ColorCurrBuilding(Color.red);
		}
		else if (CanBePlaced(CurrBuilding))
		{
			ChangeCursor(_defaultCursor, true);
			ColorCurrBuilding(Color.green);
		}
	}


	// Grid____________________________________________________________
	const int _areaPadding = 3;
	const int _startPadding = 1;

	public bool CanBePlaced(Building build)
	{ 
		Vector3 center = CurrBuilding.Team_.MainBuilding_.transform.position;
		int radius = CurrBuilding.Team_.MainBuilding_.BuildingRadius;

		if (Vector3.Distance(build.transform.position, center) > radius)
		{ return false; }

		Vector3Int startInt = GetAreaStartPos(build);

		for (int x = 0; x < build.Size.x + _areaPadding; x++)
		{
			for (int y = 0; y < build.Size.y + _areaPadding; y++)
			{
				var currPos = new Vector3Int(startInt.x + x, y: 0, startInt.z + y);
				if (Tilemap_.GetTile(Grid_.WorldToCell(currPos)) == _busyTile)
				{ return false; }
			}
		}
		return true;
	}

	public void TakeAreaForCurrBuild()
	{
		Vector3Int start = GetAreaStartPos(CurrBuilding);

		for (int x = 0; x < CurrBuilding.Size.x + _areaPadding; x++)
		{
			for (int y = 0; y < CurrBuilding.Size.y + _areaPadding; y++)
			{
				var currPos = new Vector3Int(start.x + x, y: 0, start.z + y);
				Tilemap_.SetTile(Grid_.WorldToCell(currPos), _busyTile);
			}
		}
	}

	public Vector3Int GetAreaStartPos(Building build)
	{
		var size3 = new Vector3Int(build.Size.x, 0, build.Size.y);
		var center = build.transform.position;
		var start = new Vector3(center.x - (size3.x / 2) - _startPadding, center.y,
								center.z - (size3.z / 2) - _startPadding);
		return new Vector3Int(Mathf.RoundToInt(start.x),
							Mathf.RoundToInt(start.y),
							Mathf.RoundToInt(start.z));
	}

	/// <summary>
	/// Maps coordinates to the grid
	/// </summary>
	/// <param name="worldPos">Real coordinates (will be transformed to the grid coordinates)</param>
	/// <returns></returns>
	public Vector3 MapCoordToGrid(Vector3 worldPos)
	{
		Vector3Int cellPos = Grid_.WorldToCell(worldPos);
		worldPos = Grid_.GetCellCenterWorld(cellPos);
		return worldPos;
	}
	// ________________________________________________________________


	// Actions_________________________________________________________
	public void SpawnMovableBuild(Building build, Team team)
	{
		if (CurrBuilding != null && !CurrBuilding.Placed)
		{ UIManager.Instance.UpdateWarningPanel("Place or delete current building first"); return ; }

		if (!Player.Instance.Shop_.TryBuyItem(build.GetComponent<Building>().Name, Player.Instance.MainBuilding_))
		{ return ; }

		var obj = SpawnBuilding(build.gameObject, team, MapCoordToGrid(GetMouseWorldPos()));
		obj.AddComponent<Movable>();

		_childrens_rends = CurrBuilding.GetComponentsInChildren<Renderer>();
		_allowBuilding = true;
	}

	private GameObject SpawnBuilding(GameObject build, Team team, Vector3 pos)
	{
		GameObject obj = Instantiate(build.gameObject, pos, build.transform.rotation);
		CurrBuilding = obj.GetComponent<Building>();
		((ITeamMember)CurrBuilding).SetTeam(team);
		
		return obj;
	}

	public GameObject SpawnBuildOnPos(GameObject build, Team team, Vector3 pos)
	{
		var obj = SpawnBuilding(build, team, pos);
		TakeAreaForCurrBuild();
		return obj;
	}

	private void ChangeCursor(Texture2D cursor, bool is_default)
	{
		Cursor.SetCursor(cursor, new Vector2(0, 0), CursorMode.Auto);
		_is_default_cursor = is_default;
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

	public void ColorCurrBuilding(Color color)
	{
		foreach (Renderer rend in _childrens_rends)
		{ rend.material.color = color; }
	}
	// ________________________________________________________________
}
