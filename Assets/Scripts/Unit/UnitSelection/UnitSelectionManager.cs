using System.Collections.Generic;
using UnityEngine;


// Z tutorialu
public class UnitSelectionManager : MonoBehaviour, IListener
{
	public static UnitSelectionManager Instance { get; set; }

	public List<GameObject> UnitsSelected { get; private set; } = new List<GameObject>();
	public List<GameObject> AllUnits { get; private set; } = new List<GameObject>();

	public GameObject GroundMarker;

	

	public bool Ready { get; private set; } = false;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }


		Ready = true;
	}

	private void Start()
	{
		StartCoroutine(((IListener)this).StartListening());
	}

	private void Update()
	{
		if (!MainController.Instance.Ready) return;

	}


	// Selection__________________________________________________
	/// <summary>
	/// If we are clicking at a unit then we select it (or deselect if alreadt selected). 
	/// Allows To select more units with LeftShift.
	/// </summary>
	private void TrySelectUnits()
	{
		GameObject comp = MouseController.Instance.GetObjOnMousePos();

		if (comp)
		{
			Unit unit = comp.GetComponent<Unit>();

			// If we are hitting a unit
			if (unit)
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{ MultiSelect(unit.gameObject); }
				else
				{ SelectByClicking(unit.gameObject); }
			}
		}
		else if (!Input.GetKey(KeyCode.LeftShift))
		{ DeselectAll(); }
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

		GroundMarker.SetActive(false);
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
	}
	// ___________________________________________________________


	// Movement___________________________________________________
	/// <summary>
	/// If units are selected then they will get a new position to which they must move.
	/// </summary>
	private void TrySetNextPos()
	{
		Vector3 worldPos = MouseController.GetMouseWorldPos();

		if (Vector3.zero != worldPos)
		{
			GroundMarker.transform.position = worldPos;
			GroundMarker.SetActive(false);
			GroundMarker.SetActive(true);
		}
	}

	public void MouseHitMapAction(int button)
	{
		if (button == 0) TrySelectUnits();
		if (button == 1 && UnitsSelected.Count > 0)
		{ TrySetNextPos(); }
	}

	// ___________________________________________________________
}