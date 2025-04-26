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

	private Camera cam;

	public LayerMask clickable;
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
		cam = Camera.main;
	}


	private void Update()
	{

		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			// If we are hitting a clickble object
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, clickable))
			{
				if (Input.GetKey(KeyCode.LeftControl))
				{ MiltiSelect(hit.collider.gameObject); }
				else
				{ SelectByClicking(hit.collider.gameObject); }
			}
			else  // Deselect all the units
			{
				if (!Input.GetKey(KeyCode.LeftControl))
				{ DeselectAll(); }
			}
		}


		if (Input.GetMouseButtonDown(1))
		{
			RaycastHit hit;
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);

			// If we are hitting a ground with right button
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
			{
				groundMarker.transform.position = hit.point;
				groundMarker.SetActive(false);
				groundMarker.SetActive(true);
			}
		}
	}

	private void MiltiSelect(GameObject unit)
	{
		if (!unitsSelected.Contains(unit))
		{
			unitsSelected.Add(unit);
			EnableUnitMovement(unit, true);
		}
		else
		{
			EnableUnitMovement(unit, false);
			unitsSelected.Remove(unit);
		}
	}

	private void SelectByClicking(GameObject unit)
	{
		DeselectAll();

		unitsSelected.Add(unit);
		EnableUnitMovement(unit, true);
	}
	private void DeselectAll()
	{
		foreach (var unit in unitsSelected)
		{ EnableUnitMovement(unit, false); }

		unitsSelected.Clear();
	}

	private void EnableUnitMovement(GameObject unit, bool canMove)
	{
		unit.GetComponent<UnitMovement>().enabled = canMove;


	}

}