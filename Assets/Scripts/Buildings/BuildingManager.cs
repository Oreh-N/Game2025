using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager Instance;
    [SerializeField] LayerMask _obstacles;
    [SerializeField] Grid _grid;
    [SerializeField] Tilemap _tilemap;
    [SerializeField] TileBase _tile;
    
    bool _allowBuilding = false;
    Building _currBuilding;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

	}


    void Update()
    {
        if (!_allowBuilding || _currBuilding == null) return;

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

    /// <summary>
    /// Maps coordinates to the grid
    /// </summary>
    /// <param name="position">Real coordinates (will be transformed to the grid coordinates)</param>
    /// <returns></returns>
    public Vector3 MapCoordToGrid(Vector3 position)
    {
        Vector3Int cellPos = _grid.WorldToCell(position);
        position = _grid.GetCellCenterWorld(cellPos);
        return position;
    }

    public void SpawnBuilding(Building building)
    {
        _allowBuilding = true;
        Vector3 spawnPos = MapCoordToGrid(Vector3.zero) + new Vector3(0, building.transform.position.y, 0);
        GameObject obj = Instantiate(building.gameObject, spawnPos, Quaternion.identity);
        _currBuilding = obj.GetComponent<Building>();
        obj.AddComponent<Movable>();
    }

}
