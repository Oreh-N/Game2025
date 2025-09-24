using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Team
{
	public static Player Instance;
	public Dictionary<LootType, int> LootCount = new Dictionary<LootType, int>() { { LootType.Tree, 0} };
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
		if (LootCount.ContainsKey(LootType.Tree))
		{
			TreePanel.GetComponent<Text>().text = $"Tree: {LootCount[LootType.Tree]}"; 
		}
	}
	// _______________________________________________________________


	// Database_______________________________________________________
	private void RecalculateLoot()
	{
		//foreach (var item in LootCount)
		//{ LootCount[item.Key] = 0; }

		foreach (var build in Buildings)
		{
			if (build is not Warehouse)
			{ continue; }

			foreach (var loot in ((Warehouse)build).LootCount)
			{
				if (LootCount.ContainsKey(loot.Key))
				{ LootCount[loot.Key] += loot.Value; }
				else
				{ LootCount.Add(loot.Key, loot.Value); Debug.Log(loot.Value); }
				
			}

		}
	}
	// _______________________________________________________________
}
