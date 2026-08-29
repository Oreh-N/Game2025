using MapSpace;
using UnityEngine;

public class MouseController : MonoBehaviour
{
	public static MouseController Instance { get; private set; }


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			LeftClick();

		}
	}

	void LeftClick()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (MainController.groundPlane.Raycast(ray, out float distance))
		{
			var pos = ray.GetPoint(distance);
			if (Map.IsOutOfMap(pos)) return;
			Component obj = GetObjOnPos(pos);
			
			if (obj && obj is IInteractable) ((IInteractable)obj).MouseDownAct();
		}

	}

	/// <summary>
	/// Get position of the mouse cursor on the world landscape
	/// </summary>
	/// <returns></returns>
	public static Vector3 GetMouseWorldPos()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (MainController.groundPlane.Raycast(ray, out float distance))
		{ return ray.GetPoint(distance); }
		return Vector3.zero;
	}

	private Component GetObjOnPos(Vector3 pos)
	{
		var mapPos = Map.WorldToMap(pos);
		if (Map.GetCellType(Map.MapNames.EnvMap, mapPos) == Map.CellType.Empty)
		{
			mapPos = Map.FindNearestCell(mapPos, 4, Map.MapNames.EnvMap);
			if (Map.GetCellType(Map.MapNames.EnvMap, mapPos) == Map.CellType.Empty)
			{ return null; }
		}
		var cellT = Map.GetBasicCellInMap(Map.MapNames.EnvMap, mapPos);
		int teamID = Map.GetCellTeamID(Map.MapNames.EnvMap, mapPos);
		var team = MainController.Instance.GetTeam(teamID);
		if (Map.IsBuilding(mapPos)) 
			return team.GetClosestTeamBuild(pos);
		if (Map.IsUnit(mapPos))
			return team.GetClosestTeamUnit(pos);
		return null;
	}
}

