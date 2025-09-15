using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitSelectionManager : MonoBehaviour
{
	public static UnitSelectionManager Instance { get; set; }

	public List<Unit> UnitsSelected { get; private set; } = new List<Unit>();
	public List<Unit> AllUnits { get; private set; } = new List<Unit>();

	private Camera _cam;

	[SerializeField] LayerMask _clickable;
	[SerializeField] LayerMask _ground;
	[SerializeField] GameObject _groundMarker;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	private void Start()
	{ _cam = Camera.main; }


	private void Update()
	{
		RaycastHit hit;
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

			// If we are hitting a clickble object
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, _clickable))
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{ MultiSelect(hit.collider.gameObject.GetComponent<Unit>()); }
				else
				{ SelectByClicking(hit.collider.gameObject.GetComponent<Unit>()); }
			}
			else  // Deselect all the units
			{
				if (!Input.GetKey(KeyCode.LeftShift))
				{ DeselectAll(); }
			}
		}
		else if (Input.GetMouseButtonDown(1) && UnitsSelected.Count > 0)
		{
			Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

			// If we are hitting a ground with right button
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground))
			{
				_groundMarker.transform.position = new Vector3(hit.point.x, .001f, hit.point.z);
				_groundMarker.SetActive(false);
				_groundMarker.SetActive(true);
			}
		}
	}

	public void RemoveUnit(Unit unit)
	{
		AllUnits.Remove(unit);
	}

	public void AddUnit(Unit unit)
	{
		AllUnits.Add(unit);
	}

	private void MultiSelect(Unit unit)
	{
		if (!UnitsSelected.Contains(unit))
		{
			UnitsSelected.Add(unit);
			SelectUnit(unit, true);
		}
		else
		{
			SelectUnit(unit, false);
			UnitsSelected.Remove(unit);
		}
	}

	private void SelectByClicking(Unit unit)
	{
		DeselectAll();

		UnitsSelected.Add(unit);
		SelectUnit(unit, true);
	}
	public void DeselectAll()
	{
		foreach (var unit in UnitsSelected)
		{ SelectUnit(unit, false); }

		_groundMarker.SetActive(false);
		UnitsSelected.Clear();
	}

	private void EnableUnitMovement(Unit unit, bool canMove)
	{ unit.GetComponent<UnitMovement>().enabled = canMove; }

	private void TriggerSelectionIndicator(Unit unit, bool isVisible)
	{ unit.transform.GetChild(0).gameObject.SetActive(isVisible); }

	internal void DragSelect(Unit unit)
	{
		if (!UnitsSelected.Contains(unit))
		{
			UnitsSelected.Add(unit);
			SelectUnit(unit, true);
		}
	}

	private void SelectUnit(Unit unit, bool isSelected)
	{
		TriggerSelectionIndicator(unit, isSelected);
		EnableUnitMovement(unit, isSelected);
	}
}