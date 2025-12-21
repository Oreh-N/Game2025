using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class MapData
{
	public static int[] MapSize = new int[2] {1000, 1000};
	public Vector3 CellSize = new Vector3(1, 0.001f, 1);
	public Map.CellType[,] Map = new Map.CellType[MapSize[0], MapSize[1]];
	public Vector3 MapStart = new Vector3(0, 0, 0);	// Shifted so that cells wouldn't go out of the map


}
