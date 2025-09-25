using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Team
{
	public static Player Instance;

	public Dictionary<LootType, int> LootCount = new Dictionary<LootType, int>() { { LootType.Wood, 0} };
	public IInteractable CurrInteractObject { get; protected set; }
	public Wallet Wallet_ { get; private set; } = new Wallet(500);
	public Shop Shop { get; private set; } = new Shop();

	[SerializeField] GameObject MoneyPanel;
	[SerializeField] GameObject TreePanel;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	void Update()
	{
		RecalculateLoot();
		UpdatePanels();
	}


	// Actions_______________________________________________________
	public void SpawnObject(GameObject obj)
	{
		CurrInteractObject.Spawn(obj);
	}

	public void InteractWithObject()
	{
		CurrInteractObject.Interact();
	}

	public void ChangeInteractableObject(IInteractable obj)
	{
		CurrInteractObject = obj;
	}
	// _______________________________________________________________


	// Visual_________________________________________________________
	private void UpdatePanels()
	{
		MoneyPanel.GetComponent<Text>().text = $"Money: {Wallet_.Money}";
		if (LootCount.ContainsKey(LootType.Wood))
		{
			TreePanel.GetComponent<Text>().text = $"Tree: {LootCount[LootType.Wood]}"; 
		}
	}
	// _______________________________________________________________


	// Database_______________________________________________________
	public void RegisterBuilding(Building building)
	{ Buildings.Add(building); }

	public void RemoveBuilding(Building building)
	{ Buildings.Remove(building); }

	private void RecalculateLoot()
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
