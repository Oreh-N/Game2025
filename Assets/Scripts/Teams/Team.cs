using MapSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;


// Visuals should be separated from logic
/// <summary>
/// Centralize data about each specific team, and contains method to control it (mining, attack)
/// </summary>
public abstract class Team : MonoBehaviour, ILootContainer
{
	public TeamData data { get; protected set; } = new TeamData();


	public void Start()
	{
	}

	public void Update()
	{
		if (!MainController.Instance.Ready) return;

		if (data.IsDefeated)
		{ Debug.Log($"Team {data.TeamName} was defeated"); }

		RecalculateLoot();
	}

	public bool Ready()
	{ return data.Ready; }

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
			UIManager.Instance.UpdatePanel(UIManager.PanelNames.WarningP, "Not enough gold");
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

	public Team CreateBase()
	{
		var mb = Prefabs.MainBuildPref;
		var b = Creator.CreateBuilding(mb, data.BaseCenter).GetComponent<Building>();
		MapController.Instance.PlaceBuilding(b, this);

		if (Prefabs.WorkerPref == null) Debug.Log("Didn't find unit");
		int init_unit_count = 3;

		for (int i = 0; i < init_unit_count; i++)
		{ 
			var unit = Creator.CreateUnit(Prefabs.WorkerPref, 
				data.BaseCenter - new Vector3(15 + 5 * i, 0, 15 + 5 * i)).GetComponent<Unit>();
			if (unit) unit.Setup(data.ID);
		}
		return this;
		
	}

	// _______________________________________________________________


	// DATA_TRANSFERRING_______________________________________________________________
	/// <summary>
	/// Setup team. If there are troubles (base center is out of map, etc.) then method will return false, otherwise true.
	/// </summary>
	/// <param name="BasePos"></param>
	/// <param name="teamColor"></param>
	/// <param name="teamName"></param>
	/// <returns></returns>
	public virtual Team Setup(Vector2Int BasePos, Color teamColor, string teamName)
	{
		data.ID = TeamData.FreeID;
		TeamData.FreeID++;

		if (Map.IsOutOfMap(BasePos))
		{
			Destroy(this);
			Debug.Log("The base is out of map. Setup will be ignored.");
			return null;
		}
		data.BaseCenter = Map.MapToWorld(BasePos);
		data.TeamColor = teamColor;
		data.TeamName = teamName;
		return this;
	}

	private T GetClosestTeamObj<T>(Vector3 pos, List<T> objList) where T : Component
	{
		if (objList.Count < 1) return null; 

		T obj = objList[0];

		foreach (var o in objList)
		{
			if (Vector3.Distance(obj.transform.position, pos) >
				Vector3.Distance(o.transform.position, pos))
			{ obj = o; }
		}
		return obj;
	}

	public Building GetClosestTeamBuild(Vector3 pos)
	{ return GetClosestTeamObj<Building>(pos, data.Buildings); }

	public Unit GetClosestTeamUnit(Vector3 pos)
	{ return GetClosestTeamObj<Unit>(pos, data.Units); }

	public void ChangeInteractableObject(IInteractable obj) { data.CurrInteractObject = obj; }

	public string GetName() { return data.TeamName; }

	public int GetID() { return data.ID; }

	public Color GetColor() { return data.TeamColor; }

	public void RegisterBuilding(Building building) 
	{ data.Buildings.Add(building); Debug.Log($"Building {building.GetType()} added"); }

	public void RemoveBuilding(Building building) 
	{ data.Buildings.Remove(building); Debug.Log($"Building {building.GetType()} removed"); }


	public void RegisterUnit(Unit unit) 
	{ data.Units.Add(unit); Debug.Log($"Unit {unit.GetType()} added"); }

	public void RemoveUnit(Unit unit) 
	{ data.Units.Remove(unit); Debug.Log($"Unit {unit.GetType()} removed"); }

	public Vector3 GetCenter() { return data.BaseCenter; }

	public float GetBuildingRadius() { return data.BuildingRadius; }

	public Inventory GetInventory() { return data.LootCounter; }
	// _________________________________________________________________________________
}
