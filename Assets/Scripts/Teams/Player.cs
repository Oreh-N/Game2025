using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Team
{
	public static Player Instance;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	private new void Start()
	{
		base.Start();
	}

	new void Update()
	{
		base.Update();
		UpdateWoodCount();
	}


	// UI_Interaction_______________________________________________________
	public void UpdateWoodCount()
	{
		int wood_count = 0;

		if (data.LootCounter.ContainsKey(LootType.Wood))
		{ wood_count = data.LootCounter[LootType.Wood]; }

		UIManager.Instance.UpdateWoodPanel(wood_count);

	}

	public void SpawnObject(GameObject obj)
	{
		//if (data.CurrInteractObject is Spawner && 
		//	data.Shop_.TryBuyItem(obj.GetComponent<Unit>().data.Name, data.MainBuilding_))
		//((Spawner)data.CurrInteractObject).Spawn(obj);
	}

	public void UpdateMainBuilding()
	{
		//BuildingManager.UpdateBuildArea(data.MainBuilding_);
		//data.MainBuilding_.UpgradeBuildingArea();
	}
	// _______________________________________________________________


	public string GetTeamName()
	{
		return data.TeamName;
	}
}
