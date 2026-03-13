using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapControllerData :  MonoBehaviour
{
	public Tilemap Tilemap_ = FindFirstObjectByType<Tilemap>();
	public Grid MapGrid = FindFirstObjectByType<Grid>();

	public bool AllowBuilding = false;
	public Building CurrBuilding;
	public TileBase BusyTile;

}
