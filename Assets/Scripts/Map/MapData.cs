using UnityEngine;
using static MapSpace.Map;

namespace MapSpace
{
	public class MapData
	{
		public static readonly Vector2Int MapSize = new Vector2Int( 999, 999 );
		public static CellType[][,] Layers = new CellType[1][,];

		public Vector3 CellSize = new Vector3(1, 0.001f, 1);
		public Vector3 MapStart = new Vector3(0.5f, 0, 0.5f);   // Shifted so that cells wouldn't go out of the map
		public static bool Ready = false;


	}
}
