using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitSelectionManager : MonoBehaviour
{
	public static UnitSelectionManager Instance { get; set; }

	public List<GameObject> unitsSelected = new List<GameObject>();
	public List<GameObject> allUnits = new List<GameObject>();

	private Camera mainCam;

	private LayerMask npc;
	public LayerMask ground;
	public GameObject groundMarker;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	private void Start()
	{ 
		mainCam = Camera.main;
		npc = LayerMask.NameToLayer("NPC");
	}


	private void Update()
	{
		RaycastHit hit;
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

			// If we are hitting a clickble object
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, npc))
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{ MultiSelect(hit.collider.gameObject); }
				else
				{ SelectByClicking(hit.collider.gameObject); }
			}
			else  // Deselect all the units
			{
				if (!Input.GetKey(KeyCode.LeftShift))
				{ DeselectAll(); }
			}
		}
		else if (Input.GetMouseButtonDown(1) && unitsSelected.Count > 0)
		{
			Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);

			// If we are hitting a ground with right button
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
			{
				groundMarker.transform.position = new Vector3(hit.point.x, .001f, hit.point.z);
				groundMarker.SetActive(false);
				groundMarker.SetActive(true);
			}
		}
	}

	private void MultiSelect(GameObject unit)
	{
		if (!unitsSelected.Contains(unit))
		{
			unitsSelected.Add(unit);
			SelectUnit(unit, true);
		}
		else
		{
			SelectUnit(unit, false);
			unitsSelected.Remove(unit);
		}
	}

	private void SelectByClicking(GameObject unit)
	{
		DeselectAll();

		unitsSelected.Add(unit);
		SelectUnit(unit, true);
	}
	public void DeselectAll()
	{
		foreach (var unit in unitsSelected)
		{ SelectUnit(unit, false); }

		groundMarker.SetActive(false);
		unitsSelected.Clear();
	}

	private void EnableUnitMovement(GameObject unit, bool canMove)
	{ unit.GetComponent<UnitMovement>().enabled = canMove; }

	private void TriggerSelectionIndicator(GameObject unit, bool isVisible)
	{ unit.transform.GetChild(0).gameObject.SetActive(isVisible); }

	internal void DragSelect(GameObject unit)
	{
		if (!unitsSelected.Contains(unit))
		{
			unitsSelected.Add(unit);
			SelectUnit(unit, true);
		}
	}

	private void SelectUnit(GameObject unit, bool isSelected)
	{
		TriggerSelectionIndicator(unit, isSelected);
		EnableUnitMovement(unit, isSelected);
	}
}