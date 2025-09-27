using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Team : MonoBehaviour, ITeamMember
{
	Color ITeamMember.TeamColor { get => TeamColor; set => TeamColor = value; }
	string ITeamMember.TeamName { get => TeamName; set => TeamName = value; }
	// Sources
	public Dictionary<LootType, int> LootCount { get; protected set; } 
	 = new Dictionary<LootType, int>() { { LootType.Wood, 0 } };
	public Shop Shop { get; protected set; } = new Shop();
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
		MainBuilding_.SetTeam(TeamColor, TeamName);
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

	// Database_______________________________________________________
	public void RegisterBuilding(Building building)
	{ Buildings.Add(building); }

	public void RemoveBuilding(Building building)
	{ Buildings.Remove(building); }

	public void RecalculateLoot()
	{
		LootCount.Clear();

		foreach (var build in Buildings)
		{
			if (build is not Warehouse)
			{ continue; }

			foreach (var loot in ((Warehouse)build).LootCount)
			{
				if (LootCount.ContainsKey(loot.Key))
				{ LootCount[loot.Key] += loot.Value; }
				else
				{ LootCount.Add(loot.Key, loot.Value); }

			}

		}
	}
	// _______________________________________________________________
}
