using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
	public static UnitSelectionManager Instance {  get; set; }

	public List<GameObject> unitsSelected = new List<GameObject>();
	public List<GameObject> allUnits = new List<GameObject>();

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else 
		{ Instance = this; }
	}




}


