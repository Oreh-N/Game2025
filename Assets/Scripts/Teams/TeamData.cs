using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TeamData
{
	// Sources
	public Inventory LootCounter = new Inventory { { LootType.Wood, 0 } };
	public IInteractable CurrInteractObject;
	public Shop Shop_ = new Shop();
	//__________________
	// Members/Buildings
	public List<Building> Buildings = new List<Building>();
	public List<Unit> Members = new List<Unit>();
	//___________________
	// Team identifier
	public Vector3 BaseCenter = new Vector3(0,0,0);
	public float BuildingRadius = 50;
	public string TeamName = "";
	public Color TeamColor = Color.white;
	public int ID;
	//___________________
	public bool IsDefeated;
}