using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitMovement : MonoBehaviour
{
	[SerializeField] LayerMask _ground;
	bool _isMoving = false;
	float _speed = 10f;
	Vector3 _nxtPos;
	List<Vector3> _milestones = new List<Vector3>();
	//The idea is to split direct way to the next position into several
	//next position that are closer and if some of them are in the
	//obstacle we will recalculate the way for them (maybe find shortest one)
	//then we will go step by step to the final destination


	private void Update()
	{
		if (Input.GetMouseButtonDown(1))
		{ TryGetNextPosition(); }

		if (_isMoving)
		{ Move(); }
	}

	private void Move()
	{
		transform.LookAt(_nxtPos);
		transform.position = Vector3.MoveTowards(transform.position, _nxtPos, _speed * Time.deltaTime);

		if ((int)transform.position.x == (int)_nxtPos.x && (int)transform.position.z == (int)_nxtPos.z)
		{ 
			_isMoving = false;
		}
		Debug.DrawLine(transform.position, _nxtPos, Color.red);
	}

	private void TryGetNextPosition()
	{
		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground) &&
			UnitSelectionManager.Instance.UnitsSelected.Contains(gameObject))
		{
			_nxtPos = hit.point;
			_isMoving = true;
		}
	}
}
