using MapSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class UnitGroupMovement
{
	static List<UnitMovement> _units = new List<UnitMovement>();
	static int _movingUnitsCount = 0;

	public static void MoveMe(UnitMovement unit)
	{
		if (_movingUnitsCount == 0)
			_movingUnitsCount = UnitSelectionManager.Instance.UnitsSelected.Count;

		_units.Add(unit);

		if (_units.Count == _movingUnitsCount)
		{
			FindTargets();
			_units.Clear();
			_movingUnitsCount = 0;
		}
	}

	private static void FindTargets()
	{
		Vector3 pos = MouseController.GetMouseWorldPos();

		if (Vector3.zero != pos && !Map.IsOutOfMap(pos))
		{
			int distBetween = 5;
			var preferedDir = new Vector3(-1, 0, -0.5f);
			// units will be assambled on square area
			int squareSide = ((int)Math.Sqrt(_movingUnitsCount) + 1) * distBetween;
			// The position of the mouse will be at the forward center
			// *1.5 to be bigger than rombus diameter

			var points = ComputeSquarePoints(pos, (int)(squareSide * 1.5), preferedDir);
			var units_per_row = (int)Math.Sqrt(_movingUnitsCount) + 1;
			var dirX = (points[1] - points[0]).normalized;
			var dirZ = (points[2] - points[0]).normalized;
			Vector3 unitPos = points[0];

			for (int z = 0; z < units_per_row; z++)
				for (int x = 0; x < units_per_row; x++)
				{
					int idx = z * units_per_row + x;
					if (idx > _units.Count) continue;
					_units[z + x].SetTargetPositionInGroup
						(unitPos 
						+ dirX * distBetween * x 
						+ dirZ * distBetween * z);
				}
			
		}
	}

	static Vector3[] ComputeSquarePoints(Vector3 anchor, float squareSize, Vector3 direction)
	{
		Vector3 forward = direction.normalized;
		Vector3 right = Vector3.Cross(Vector3.up, forward);

		float halfWidth = squareSize / 2f;

		Vector3 nearLeft = anchor - right * halfWidth;
		Vector3 nearRight = anchor + right * halfWidth;
		Vector3 farLeft = anchor - right * halfWidth + forward * squareSize;
		Vector3 farRight = anchor + right * halfWidth + forward * squareSize;

		return new[] { nearLeft, nearRight, farLeft, farRight };
	}

}

// Map:
//	|---|---| Right	(X)
//	| 2 | 1 |
//	|___|___|
//	| 3 | 4 |
//	|___|___|
// Left		Camera and (0,0,0)
// (Z)
//
//
//