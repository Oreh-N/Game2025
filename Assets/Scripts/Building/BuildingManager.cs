using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Unity.VisualScripting;


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

	Vector3 _buildingAreaCenter = new Vector3();
	bool _is_default_cursor = true;
	bool _allowBuilding = false;
	int _buildingRadius;


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

		if (!CanBePlaced(CurrBuilding) && _is_default_cursor)
		{ ChangeCursor(_declineCursor, false); }
		else if (CanBePlaced(CurrBuilding) && !_is_default_cursor)
		{ ChangeCursor(_defaultCursor, true); }

		if (Input.GetMouseButtonDown(1))
		{
			if (CanBePlaced(CurrBuilding))
			{
				CurrBuilding.Construct();
				TakeArea(CurrBuilding.transform.position, CurrBuilding.Size, _busyTile);
				_allowBuilding = false;
			}
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			int returnPrice = Player.Instance.Shop.GetItemPrice(CurrBuilding.Name);
			if (returnPrice > 0)
			{ Player.Instance.MainBuilding_.Wallet_.Earn(returnPrice); }
			Destroy(CurrBuilding.gameObject);
		}
	}


	// Grid____________________________________________________________
	public bool CanBePlaced(Building building)
	{
		if (Vector3.Distance(building.transform.position, _buildingAreaCenter) > _buildingRadius)
		{ return false; }
		BoundsInt area = new BoundsInt();
		area.position = Grid_.WorldToCell(building.transform.position);
		area.size = building.Size;
		TileBase[] baseArr = GetTilesBlock(area, Tilemap_);
		foreach (var b in baseArr)
		{
			if (b == _busyTile) return false;
		}
		return true;
	}

	private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
	{
		TileBase[] arr = new TileBase[area.size.x * area.size.y * area.size.z];
		int counter = 0;

		foreach (var vector in area.allPositionsWithin)
		{
			Vector3Int pos = new Vector3Int(vector.x, vector.y, z: 0);
			arr[counter] = Tilemap_.GetTile(pos);
			counter++;
		}
		return arr;
	}

	/// <summary>
	/// Computes bottom left and top right coordinates (to fill the whole area under the object correctly) 
	/// </summary>
	/// <param name="start">The object start position</param>
	/// <param name="size">Object's size</param>
	/// <returns>(BottomLeftCoordinates, TopRightCoordinates)</returns>
	private static (Vector3Int, Vector3Int) CompAreaBordersCoords(Vector3Int start, Vector3Int size)
	{
		Vector3Int bottomLeft = new Vector3Int(
		start.x - size.x / 2,
		start.y - size.y / 2,
		start.z
	);

		Vector3Int topRight = new Vector3Int(
			bottomLeft.x + size.x,
			bottomLeft.y + size.y,
			start.z
		);
		return (bottomLeft, topRight);
	}

	public static void TakeArea(Vector3 pos, Vector3Int size, TileBase tile)
	{
		size = Grid_.LocalToCell(size);
		var bottomLeft = new Vector3Int();
		var topRight = new Vector3Int();
		Vector3Int start = Grid_.WorldToCell(pos);
		(bottomLeft, topRight) = CompAreaBordersCoords(start, size);
		//Debug.Log(start);
		//Debug.Log(bottomLeft);
		//Debug.Log(topRight);
		Tilemap_.BoxFill(start, tile, bottomLeft.x, bottomLeft.y, topRight.x, topRight.y);
	}

	/// <summary>
	/// Maps coordinates to the grid
	/// </summary>
	/// <param name="position">Real coordinates (will be transformed to the grid coordinates)</param>
	/// <returns></returns>
	public Vector3 MapCoordToGrid(Vector3 position)
	{
		Vector3Int cellPos = Grid_.WorldToCell(position);
		position = Grid_.GetCellCenterWorld(cellPos);
		return position;
	}
	// ________________________________________________________________


	// Actions_________________________________________________________
	public void SpawnBuilding(Building building, Team team)
	{
		_buildingAreaCenter = team.MainBuilding_.transform.position;
		_buildingRadius = team.MainBuilding_.BuildingRadius;
		bool was_bought = false;
		if (CurrBuilding != null && !CurrBuilding.Placed)
		{ UIManager.Instance.UpdateWarningPanel("Place or delete current building first"); return; }

		was_bought = Player.Instance.Shop.TryBuyItem(building.Name, Player.Instance.MainBuilding_.Wallet_);

		if (!was_bought) { return; }
		Vector3 spawnPos = MapCoordToGrid(GetMouseWorldPos());
		GameObject obj = Instantiate(building.gameObject, spawnPos, building.transform.rotation);
		CurrBuilding = obj.GetComponent<Building>();
		((ITeamMember)CurrBuilding.GetComponent<Building>()).SetTeam(team);
		obj.AddComponent<Movable>();
		_allowBuilding = true;
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
	// ________________________________________________________________
}
