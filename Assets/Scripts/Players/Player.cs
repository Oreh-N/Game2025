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

		BaseLocation = new Vector3(52, 0, 52);
		SetTeam(new Color(0.7f, 0.4f, 0.9f), "Nuts");
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

		if (LootCounter.ContainsKey(LootType.Wood))
		{ wood_count = LootCounter[LootType.Wood]; }

		UIManager.Instance.UpdateWoodPanel(wood_count);

	}

	public void SpawnObject(GameObject obj)
	{
		if (CurrInteractObject is Spawner)
		((Spawner)CurrInteractObject).Spawn(obj);
	}

	// _______________________________________________________________


}
