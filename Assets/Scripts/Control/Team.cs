using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Team : MonoBehaviour, ILootContainer
{
	public TeamData data { get; protected set; } = new TeamData();


	public void Start()
	{
		var building = MainController.Instance.MainBuildingPrefab;

		BuildingManager.Instance.SpawnObjOnPos
		(building, this, data.BaseLocation);

	}

	public void Update()
	{
		if (data.IsDefeated)
		{ Debug.Log($"Team {data.TeamName} was defeated"); }

		RecalculateLoot();
	}

	public void InteractWithObject()
	{ data.CurrInteractObject.Interact(); }


	//public void SpawnBuilding(Building building)
	//{ BuildingManager.Instance.SpawnMovableBuild(building, this); }

	public bool Interacting() { return data.CurrInteractObject != null; }

	// Database_______________________________________________________

	public void RecalculateLoot()
	{
		data.LootCounter.Clear();

		foreach (var build in data.Buildings)
		{
			if (build is not ILootContainer)
			{ continue; }

			Inventory inv = ((ILootContainer)build).GetInventory();
			foreach (var loot in inv)
			{
				if (data.LootCounter.ContainsKey(loot.Key))
				{ data.LootCounter[loot.Key] += loot.Value; }
				else
				{ data.LootCounter.Add(loot.Key, loot.Value); }

			}

		}
	}

	// _______________________________________________________________


	// DATA_TRANSFERRING_______________________________________________________________

	public void SetTeam(Color teamColor, string teamName)
	{
		data.TeamColor = teamColor;
		data.TeamName = teamName;
	}

	public void ChangeInteractableObject(IInteractable obj) { data.CurrInteractObject = obj; }

	public string GetName() { return data.TeamName; }

	public int GetID() { return data.ID; }

	public Color GetColor() { return data.TeamColor; }

	public void RegisterBuilding(Building building) { data.Buildings.Add(building); }

	public void RemoveBuilding(Building building) { data.Buildings.Remove(building); }

	public Vector3 GetCenter() { return data.BaseLocation; }

	public int GetBuildingRadius() { return data.BuildingRadius; }

	public Inventory GetInventory() { return data.LootCounter; }
	// _________________________________________________________________________________
}
