using System.Collections.Generic;
using UnityEngine;
using Map = MapSpace.Map;
using MNames = MapSpace.Map.MapNames;


[RequireComponent(typeof(Unit))]
public class UnitMovement : MonoBehaviour, IListener
{
	bool _isMoving = false;
	bool _findNextStepPos = false;
	float _speed = 20f;
	Vector2Int _targetPos;
	Vector2Int _stepPos;
	Vector3 _dir = Vector3.forward;


	private void Start()
	{
		var pos = Map.WorldToMap(transform.position);

		if (!TrySetUnitOnMap(pos))
		{
			pos = Map.FindNearestCell(pos, Map.CellType.Empty,
				(nxtCellPos, targetCellT, mapName, _) => { return Map.GetCellType(mapName, nxtCellPos) == targetCellT; },
				MNames.EnvMap, (dirs) => dirs);
			transform.position = Map.MapToWorld(pos);
			Debug.Log(pos);
		}
		StartCoroutine(((IListener)this).StartListening());
	}

	private void Update()
	{
		if (!MainController.Instance.Ready) return;

		if (_findNextStepPos)
		{
			_stepPos = FindNextStepPos();
			TurnTo(_stepPos);
			TrySetUnitOnMap(_stepPos);
			Map.CleanCell(MNames.EnvMap, Map.WorldToMap(transform.position));

			if (Map.IsOutOfMap(_stepPos))
			{
				_isMoving = false;
				_findNextStepPos = false;
				return;
			}
			_isMoving = true;
			_findNextStepPos = false;
		}

		if (_isMoving)
		{ MoveTo(_stepPos); }
	}

	private bool TrySetUnitOnMap(Vector2Int mapPos)
	{
		var unit = GetComponent<Unit>();
		if (Map.TrySetTeamCell(MNames.EnvMap, mapPos, unit.GetUnitCellID(), unit.GetTeamID())) 
			return true;
		return false;
	}

	private void TurnTo(Vector2Int mapPos)
	{
		transform.LookAt(Map.MapToWorld(mapPos));
		_dir = Map.MapToWorld(_stepPos) - transform.position;
		_dir.Normalize();
	}

	private Vector2Int FindNextStepPos()    // !!! Careful with map access from multiple units (first - map update, second - move)
	{
		// Sorts directions so that directions with better heuristic would be first
		List<Vector2Int> DirHeuristicSort(List<Vector2Int> dirs) // Assume dirs will be very small (8 items)
		{
			var mapPos = Map.WorldToMap(transform.position);
			List<Vector2Int> sortDirs = new List<Vector2Int>() { dirs[0] };

			for (int j = 1; j < dirs.Count; j++)
			{
				for (int i = 0; i < sortDirs.Count; i++)
				{
					if (Vector2Int.Distance(mapPos + sortDirs[i], _targetPos) > 
						Vector2Int.Distance(mapPos + dirs[j], _targetPos))
					{ sortDirs.Insert(i, dirs[j]); break; }
					else if (i == sortDirs.Count - 1)
					{ sortDirs.Add(dirs[j]); break; }
				}
			}
			return sortDirs;
		}

		return Map.FindNearestCell(Map.WorldToMap(transform.position), Map.CellType.Empty,
			(nxtCellPos, targetCellT, _, ignoreTypes) => { return Map.CellInAllMapsIs(targetCellT, nxtCellPos, ignoreTypes); },
			MNames.Invalid, DirHeuristicSort);
	}

	private void MoveTo(Vector2Int nxtPos)
	{
		transform.position = Vector3.MoveTowards(transform.position, Map.MapToWorld(nxtPos), _speed * Time.deltaTime);
		var mapPos = Map.WorldToMap(transform.position);

		if (Vector2Int.Distance(mapPos, nxtPos) < 0.1f)
		{
			_isMoving = false;

			if (_targetPos == mapPos)
			{			
				_findNextStepPos = false;
			}
			else
				_findNextStepPos = true;
		}
		Debug.DrawLine(transform.position, Map.MapToWorld(nxtPos), Color.red);
	}

	private void SetTargetPos(Vector3 pos)
	{
		_targetPos = Map.WorldToMap(pos);
		_isMoving = false;
		_findNextStepPos = true;
	}

	private bool CellIsEmpty(Vector2Int pos)
	{
		return Map.CellInAllMapsIs(Map.CellType.Empty, pos);
	}

	private void FindTargetPosition()
	{
		var pos = MouseController.GetMouseWorldPos();

		if (UnitSelectionManager.Instance.UnitsSelected.Count > 0 &&
			UnitSelectionManager.Instance.UnitsSelected.Contains(gameObject) &&
			Vector3.zero != pos)
		{
			if (Map.IsOutOfMap(pos) || 
				!CellIsEmpty(Map.WorldToMap(pos)))
			{ return; }

			if (UnitSelectionManager.Instance.UnitsSelected.Count > 1)
			{ UnitGroupMovement.MoveMe(this); }
			else SetTargetPos(pos);
		}
	}

	public void SetTargetPositionInGroup(Vector3 targetPos)
	{
		SetTargetPos(targetPos);
	}

	public void MouseHitMapAction(int button)
	{
		if (1 == button)
		{ FindTargetPosition(); }
	}


	//private void OnDrawGizmos()
	//{
	//	var color = Color.indianRed;
	//	Gizmos.color = color;
	//	Gizmos.DrawCube(Map.MapToWorld(new Vector2Int(5, 10)), new Vector3(1,1,1));
	// Factual position is _targetPoos +1 both to x and z
	//}
}


// We will implement dynamic pathfinding as we need to avoid other units, when several of them can move in the same time
