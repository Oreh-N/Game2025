using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using Unity.VisualScripting;


public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    LayerMask _obstacles;
    [SerializeField] TileBase _busyTile;
	[SerializeField] TileBase _freeTile;
	public Grid Grid_ { get; private set; }
    static Tilemap _tilemap;
    bool _allowBuilding = false;
    Building _currBuilding;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

        Grid_ = FindObjectOfType<Grid>();
        _tilemap = FindObjectOfType<Tilemap>();
		_obstacles = LayerMask.GetMask(PubNames.BuildingLayer);
	}


    void Update()
    {
        if (!_allowBuilding || _currBuilding == null) return;

        if (CanBePlaced(_currBuilding))
        {
            Debug.Log("Building's size");
            Debug.Log(_currBuilding.Size);
            TakeArea(Grid_.WorldToCell(_currBuilding.transform.position), _currBuilding.Size, _freeTile); }
        else 
        { TakeArea(Grid_.WorldToCell(_currBuilding.transform.position), _currBuilding.Size, _busyTile); }

        if (Input.GetMouseButtonDown(0))
        {
            if (CanBePlaced(_currBuilding))
            {
                _currBuilding.Construct();
                Vector3Int start = Grid_.WorldToCell(_currBuilding.transform.position);
                _allowBuilding = false;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape)) Destroy(_currBuilding.gameObject);
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

    private bool CanBePlaced(Building building)
    {
        BoundsInt area = new BoundsInt();
        area.position = Grid_.WorldToCell(building.transform.position);
        area.size = building.Size;
        TileBase[] baseArr = GetTilesBlock(area, _tilemap);
		foreach (var b in baseArr)
		{
			if (b == _busyTile) return false;
		}
        return true;

	}

    public void TakeArea(Vector3Int start, Vector3Int size, TileBase tile)
    {
        _tilemap.BoxFill(start, tile, start.x, start.y, start.x + size.x + 1, start.y + size.y + 1);
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] arr = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

		foreach (var vector in area.allPositionsWithin)
		{
            Vector3Int pos = new Vector3Int(vector.x, vector.y, z: 0);
            arr[counter] = _tilemap.GetTile(pos);
            counter++;
        }
        return arr;
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

    public void SpawnBuilding(Building building)
    {
        if (_currBuilding != null && !_currBuilding.Placed) 
        { Debug.Log("Place or delete current building first"); return; }
		Vector3 spawnPos = MapCoordToGrid(Vector3.zero);
        GameObject obj = Instantiate(building.gameObject, spawnPos, Quaternion.identity);
        _currBuilding = obj.GetComponent<Building>();
        obj.AddComponent<Movable>();
        _allowBuilding = true;
    }

}
