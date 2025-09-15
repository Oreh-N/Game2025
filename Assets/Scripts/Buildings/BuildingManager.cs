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
    public Vector3 _currBuildHeight {  get; private set; }


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

        _grid.GetComponentInChildren<Tilemap>();
    }

    public static Vector3 GetMouseWorldPos()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit)) 
            return hit.point;
        return Vector3.zero;
    }

    public Vector3 MapCoordToGrid(Vector3 position)
    {
        Vector3Int cellPos = _grid.WorldToCell(position);
        position = _grid.GetCellCenterWorld(cellPos);
        return position;
    }

    public void InitializeWithObject(Building building)
    {
        _allowBuilding = true;
        Vector3 position = MapCoordToGrid(Vector3.zero);
        GameObject obj = Instantiate(building.gameObject, position, Quaternion.identity);
        _currBuilding = obj.GetComponent<Building>();
        obj.AddComponent<Movable>();
       _currBuildHeight = new Vector3(0, building.transform.position.y, 0);
        obj.transform.position += _currBuildHeight;
    }

}
