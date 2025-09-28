using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Team : MonoBehaviour, ILootContainer
{
	// Sources
	public Inventory LootCounter { get; set; } = new Inventory { { LootType.Wood, 0 } };
	public IInteractable CurrInteractObject { get; protected set; }
	public Shop Shop_ { get; protected set; } = new Shop();
	//__________________
	// Members/Buildings
	public List<Building> Buildings { get; protected set; } = new List<Building>();
	public List<IAlive> Members { get; protected set; } = new List<IAlive>();
	public MainBuilding MainBuilding_ { get; protected set; }
	//___________________
	// Team identifier
	public Vector3 BaseLocation { get; protected set; }
	public string TeamName { get; protected set; }
    public Color TeamColor { get; protected set; }
	//___________________


	public void Start()
	{
		var build = Instantiate(MainController.Instance.MainBuildingPrefab, BaseLocation, Quaternion.identity);
		MainBuilding_ = build.GetComponent<MainBuilding>();
		((ITeamMember)MainBuilding_).SetTeam(this);
	}

	public void Update()
	{
		if (MainBuilding_ == null)
		{ Debug.Log($"Team {TeamName} was defeated"); }

		RecalculateLoot();
	}

	public void SetTeam(Color teamColor, string teamName)
	{
		TeamColor = teamColor;
		TeamName = teamName;
	}

	public void InteractWithObject()
	{ CurrInteractObject.Interact(); }

	public void ChangeInteractableObject(IInteractable obj)
	{ CurrInteractObject = obj; }

	public void SpawnBuilding(Building building)
	{ BuildingManager.Instance.SpawnBuilding(building, this); }


	// Database_______________________________________________________
	public void RegisterBuilding(Building building)
	{ Buildings.Add(building); }

	public void RemoveBuilding(Building building)
	{ Buildings.Remove(building); }

	public void RecalculateLoot()
	{
		LootCounter.Clear();

		foreach (var build in Buildings)
		{
			if (build is not ILootTaker)
			{ continue; }

			foreach (var loot in ((ILootTaker)build).LootCounter)
			{
				if (LootCounter.ContainsKey(loot.Key))
				{ LootCounter[loot.Key] += loot.Value; }
				else
				{ LootCounter.Add(loot.Key, loot.Value); }

			}

		}
	}
	// _______________________________________________________________
}
