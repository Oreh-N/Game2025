using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Team
{
	public static Player Instance;

	public IInteractable CurrInteractObject { get; protected set; }

	[SerializeField] GameObject MoneyPanel;
	[SerializeField] GameObject TreePanel;


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
		UpdatePanels();
	}


	// UI_Interaction_______________________________________________________
	public void SpawnObject(GameObject obj)
	{
		CurrInteractObject.Spawn(obj);
	}

	public void SpawnBuilding(Building building)
	{
		BuildingManager.Instance.SpawnBuilding(building, this);
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
}
