using MapSpace.MapLayers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Map = MapSpace.Map;
using MNames = MapSpace.MapLayers.Maps.MapNames;


[RequireComponent(typeof(Unit))]
public class UnitMovement : MonoBehaviour
{
	[SerializeField] LayerMask _ground;
	bool _isMoving = false;
	bool _findNextStepPos = false;
	float _speed = 20f;
	Vector2Int _targetPos;
	Vector2Int _prevPos;
	Vector2Int _nxtPos;


	private void Start()
	{
		_prevPos = Map.WorldToMap(transform.position);

		if (!Map.TrySetCell(_prevPos, Map.CellType.Unit, MNames.UnitMap))
		{
			_prevPos = Map.FindNearestCell(_prevPos, Map.CellType.Empty, 
				(nxtCellPos, targetCellT, mapName, _) => { return Map.GetCellType(nxtCellPos, mapName) == targetCellT; },
				MNames.UnitMap, (dirs) => dirs);
			transform.position = Map.MapToWorld(_prevPos);
		}

	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{ FindTargetPosition(); }

		if (_findNextStepPos)
		{
			_nxtPos = FindNextStepPos();
			_isMoving = true;
			_findNextStepPos = false;
			Debug.Log($"Next position is: {_nxtPos}");
		}

		if (_isMoving)
		{ MoveTo(_nxtPos); }
	}

	private Vector2Int FindNextStepPos()    // !!! Careful with map access from multiple units (first - map update, second - move)
	{

		// Sorts directions so that directions with better heuristic would be first
		List<Vector2Int> DirHeuristicSort(List<Vector2Int> dirs) // Assume dirs will be very small (4 items)
		{
			List<Vector2Int> sortDirs = new List<Vector2Int>() { dirs[0] };

			for (int j = 1; j < dirs.Count; j++)
			{
				for (int i = 0; i < sortDirs.Count; i++)
				{ 
					if (Vector2Int.Distance(sortDirs[i], _targetPos) > Vector2Int.Distance(dirs[j], _targetPos))
					{ sortDirs.Insert(i, dirs[j]); break; }
					else if (i == sortDirs.Count - 1)
					{ sortDirs.Add(dirs[j]); break; }
				}
			}
			return sortDirs;
		}
		return Map.FindNearestCell(_nxtPos, Map.CellType.Empty, 
			(nxtCellPos, targetCellT, _, ignoreTypes) => { return Maps.CellInAllMapsIs(targetCellT, nxtCellPos, ignoreTypes); }, 
			MNames.Invalid,	DirHeuristicSort,
			new List<Map.CellType> {Map.CellType.BuildArea, Map.CellType.Road });
	}

	private void MoveTo(Vector2Int nxtPos)
	{
		transform.position = Vector3.MoveTowards(transform.position, Map.MapToWorld(nxtPos), _speed * Time.deltaTime);

		if (Vector2Int.Distance(new Vector2Int((int)transform.position.x, (int)transform.position.z), _nxtPos) < 1)
		{
			_isMoving = false;
			_findNextStepPos = true;
		}
		Debug.DrawLine(transform.position, Map.MapToWorld(_nxtPos), Color.red);
	}

	private void FindTargetPosition()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground) &&
			UnitSelectionManager.Instance.UnitsSelected.Contains(gameObject))
		{
			_targetPos = Map.WorldToMap(hit.point);
			_isMoving = false;
			_findNextStepPos = true;
		}
	}
}


// We will implement dynamic pathfinding as we need to avoid other units, when several of them can move in the same time
