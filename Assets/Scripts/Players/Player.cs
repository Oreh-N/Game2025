using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Team
{
	public static Player Instance;
	public Wallet _wallet { get; private set; } = new Wallet(500);
	public Shop Shop { get; private set; } = new Shop();
	[SerializeField] GameObject MoneyPanel;
	[SerializeField] GameObject TreePanel;

	public IInteractable CurrInteractObject { get; protected set; }
	public Dictionary<LootType, int> LootCount = new Dictionary<LootType, int>();


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

	}

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

	void Start()
	{
	}


	void Update()
	{
		//UIManager.Instance.UpdateMoneyPanel(_wallet.Money);
		RecalculateLoot();
		UpdatePanels();
	}

	private void UpdatePanels()
	{
		MoneyPanel.GetComponent<Text>().text = $"Money: {_wallet.Money}";
		if (LootCount.ContainsKey(LootType.Tree))
		{
			TreePanel.GetComponent<Text>().text = $"Tree: {LootCount[LootType.Tree]}"; 
		}
	}

	private void RecalculateLoot()
	{
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
}
