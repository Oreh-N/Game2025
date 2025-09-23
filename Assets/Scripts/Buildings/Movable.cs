using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
	private void Update()
	{
		//BuildingManager.FreeArea(BuildingManager.Grid_.WorldToCell(BuildingManager.Instance.CurrBuilding.transform.position), BuildingManager.Instance.CurrBuilding.Size);
		Vector3 new_pos = BuildingManager.GetMouseWorldPos();
		transform.position = BuildingManager.Instance.MapCoordToGrid(new_pos);
	}
}
