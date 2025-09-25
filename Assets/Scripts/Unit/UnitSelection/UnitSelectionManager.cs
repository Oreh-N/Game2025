using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


// Z tutorialu
public class UnitSelectionManager : MonoBehaviour
{
	public static UnitSelectionManager Instance { get; set; }

	public List<GameObject> UnitsSelected { get; private set; } = new List<GameObject>();
	public List<GameObject> AllUnits { get; private set; } = new List<GameObject>();

	[SerializeField] GameObject _groundMarker;

	LayerMask _ground;
	LayerMask _units;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

		_ground = LayerMask.GetMask(PubNames.GroundLayer);
		_units = LayerMask.GetMask(PubNames.UnitsLayer);
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{ TrySelectUnits(); }
		else if (Input.GetMouseButtonDown(1) && UnitsSelected.Count > 0)
		{ TrySetNextPos(); }
	}


	// Selection__________________________________________________
	/// <summary>
	/// If we are clicking at a unit then we select it (or deselect if alreadt selected). 
	/// Allows To select more units with LeftShift.
	/// </summary>
	private void TrySelectUnits()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		// If we are hitting a unit
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, _units))
		{
			if (Input.GetKey(KeyCode.LeftShift))
			{ MultiSelect(hit.collider.gameObject); }
			else
			{ SelectByClicking(hit.collider.gameObject); }
		}
		else  // Deselect all units
		{
			if (!Input.GetKey(KeyCode.LeftShift))
			{ DeselectAll(); }
		}
	}

	public void RemoveUnit(GameObject unit)
	{
		if (AllUnits.Contains(unit))
		{ AllUnits.Remove(unit); }
	}

	public void AddUnit(GameObject unit)
	{ 
		if (!AllUnits.Contains(unit))
		AllUnits.Add(unit); 
	}

	private void MultiSelect(GameObject unit)
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

	private void SelectByClicking(GameObject unit)
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

	private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
	{ unit.transform.GetChild(0).gameObject.SetActive(isVisible); }

	internal void DragSelect(GameObject unit)
	{
		if (!UnitsSelected.Contains(unit))
		{
			UnitsSelected.Add(unit);
			SelectUnit(unit, true);
		}
	}

	/// <summary>
	/// Selects the unit
	/// </summary>
	/// <param name="unit"></param>
	/// <param name="isSelected"></param>
	private void SelectUnit(GameObject unit, bool isSelected)
	{
		TriggerSelectionIndicator(unit, isSelected);
		EnableUnitMovement(unit, isSelected);
	}
	// ___________________________________________________________


	// Movement___________________________________________________
	/// <summary>
	/// If units are selected then they will get a new position to which they must move.
	/// </summary>
	private void TrySetNextPos()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		RaycastHit hit;
		// If we are hitting a ground with right button
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, _ground))
		{
			_groundMarker.transform.position = new Vector3(hit.point.x, .1f, hit.point.z);
			_groundMarker.SetActive(false);
			_groundMarker.SetActive(true);
		}
	}

	private void EnableUnitMovement(GameObject unit, bool canMove)
	{ unit.GetComponent<UnitMovement>().enabled = canMove; }
	// ___________________________________________________________
}