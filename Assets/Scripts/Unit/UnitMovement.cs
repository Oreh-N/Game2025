using MapSpace.MapLayers;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using Map = MapSpace.Map;
using MNames = MapSpace.MapLayers.Maps.MapNames;


[RequireComponent(typeof(Unit))]
public class UnitMovement : MonoBehaviour
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

		if (!Map.TrySetCell(pos, Map.CellType.Unit, MNames.UnitMap))
		{
			pos = Map.FindNearestCell(pos, Map.CellType.Empty,
				(nxtCellPos, targetCellT, mapName, _) => { return Map.GetCellType(nxtCellPos, mapName) == targetCellT; },
				MNames.UnitMap, (dirs) => dirs);
			transform.position = Map.MapToWorld(pos);
		}
	}

	private void Update()
	{
		if (!MainController.Instance.Ready) return;

		if (Input.GetMouseButtonDown(1))
		{ FindTargetPosition(); }

		if (_findNextStepPos)
		{
			_stepPos = FindNextStepPos();
			TurnTo(_stepPos);
			Maps.TrySetCell(MNames.UnitMap, _stepPos, Map.CellType.Unit);
			Maps.CleanCell(MNames.UnitMap, Map.WorldToMap(transform.position));

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
			(nxtCellPos, targetCellT, _, ignoreTypes) => { return Maps.CellInAllMapsIs(targetCellT, nxtCellPos, ignoreTypes); },
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
		return Maps.CellInAllMapsIs(Map.CellType.Empty, pos);
	}

	private void FindTargetPosition()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (UnitSelectionManager.Instance.UnitsSelected.Count > 0 &&
			UnitSelectionManager.Instance.UnitsSelected.Contains(gameObject) &&
			MainController.groundPlane.Raycast(ray, out float distance))
		{
			var pos = ray.GetPoint(distance);
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


	//private void OnDrawGizmos()
	//{
	//	var color = Color.indianRed;
	//	Gizmos.color = color;
	//	Gizmos.DrawCube(Map.MapToWorld(new Vector2Int(5, 10)), new Vector3(1,1,1));
	// Factual position is _targetPoos +1 both to x and z
	//}
}


// We will implement dynamic pathfinding as we need to avoid other units, when several of them can move in the same time
