using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


// Visuals should be separated from logic
/// <summary>
/// Centralize data about each specific team, and contains method to control it (mining, attack)
/// </summary>
public abstract class Team : MonoBehaviour, ILootContainer
{
	public TeamData data { get; protected set; } = new TeamData();


	public void Start()
	{
		var mb = BuildingPrefabs.MainBuildPref;
		var b = Instantiate(mb).GetComponent<Building>(); // b.SetTeam(data.ID);
		MapController.Instance.PlaceBuilding(b);
		if (UnitPrefabs.WorkerPref == null) Debug.Log("Didn't find unit");
		int init_unit_count = 3;
		for (int i = 0; i < init_unit_count; i++)
		{ Instantiate(UnitPrefabs.WorkerPref, data.BaseCenter - new Vector3(15 + 5 * i, 0, 15 + 5 * i), Quaternion.identity); }
		
		//BuildingManager.ColorCurrBuilding(obj.GetComponent<MainBuilding>(), data.TeamColor);
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
	public void Lose()
	{ data.IsDefeated = true; }

	public void UpgradeBuildingArea()
	{
		GameObject panel = GameObject.FindGameObjectWithTag(PubNames.UpgradePanelTag);
		var text = panel.GetComponent<Text>();

		int upgradePrice;
		if (int.TryParse(text.text, out upgradePrice) && Pay(upgradePrice))
		{
			text.text = ((int)(upgradePrice * 1.5)).ToString();
			data.BuildingRadius += data.BuildingRadius * 0.3f;
			BuildingManager.ShowMessage("Building area has increased by 30%");
		}
		else
		{ BuildingManager.ShowMessage("Not enough gold"); }
	}

	/// <summary>
	/// Pays from the team's gold reserves
	/// </summary>
	/// <param name="price"></param>
	/// <returns>true if the payment was successful, otherwise returns false</returns>
	public bool Pay(int price)
	{
		if (data.LootCounter.ContainsKey(LootType.Gold) && data.LootCounter[LootType.Gold] >= price)
		{
			data.LootCounter[LootType.Gold] -= price;
			return true;
		}
		else
		{
			UIManager.Instance.UpdateWarningPanel("Not enough gold");
			return false;
		}
	}

	public void Earn(int money)
	{
		if (data.LootCounter.ContainsKey(LootType.Gold))
		{ data.LootCounter[LootType.Gold] += money; }
	}

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

	public void SetTeam(Color teamColor, string teamName, int id)
	{
		data.TeamColor = teamColor;
		data.TeamName = teamName;
		data.ID = id;
	}

	public void ChangeInteractableObject(IInteractable obj) { data.CurrInteractObject = obj; }

	public string GetName() { return data.TeamName; }

	public int GetID() { return data.ID; }

	public Color GetColor() { return data.TeamColor; }

	public void RegisterBuilding(Building building) { data.Buildings.Add(building); Debug.Log("Building added"); }

	public void RemoveBuilding(Building building) { data.Buildings.Remove(building); }

	public Vector3 GetCenter() { return data.BaseCenter; }

	public float GetBuildingRadius() { return data.BuildingRadius; }

	public Inventory GetInventory() { return data.LootCounter; }
	// _________________________________________________________________________________
}
