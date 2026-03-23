using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapControllerData
{
	public Tilemap Tilemap_;
	public Grid MapGrid;

	public bool AllowBuilding = false;
	public Building CurrBuilding;
	public TileBase BusyTile;

}
