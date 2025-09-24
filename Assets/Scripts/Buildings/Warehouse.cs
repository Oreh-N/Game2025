using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warehouse : Building
{
	public Dictionary<LootType, int> LootCount { get; protected set; } = new Dictionary<LootType, int>() { { LootType.Tree, 0 } };
	public List<string> _containment { get; protected set; } = new List<string>() { "Tree", "Money" };


	private void Start()
	{
		Panel = UIManager.Instance.GetPanelWithTag(PubNames.WarehousePanelTag);
	}


	// Interaction____________________________________________
	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == PubNames.UnitTag)
		{
			var unit = other.gameObject.GetComponent<Unit>();
			TakeLoot(unit.GiveAllLoot());
		}
	}

	private void TakeLoot(List<Loot> loot)
	{
		for (int i = 0; i < loot.Count; i++)
		{
			if (LootCount.ContainsKey(loot[i].Type))
			{ LootCount[loot[i].Type]++; }
			else
			{ LootCount.Add(loot[i].Type, 1); }
		}
		UpdatePanelInfo();
	}
	// _______________________________________________________


	// Visual_________________________________________________
	private void UpdatePanelInfo()
	{
		string text = "Containment:\n";
		foreach (var item in LootCount)
		{
			text += _containment[(int)item.Key] + " : " + item.Value.ToString() + "\n";
		}
		Panel.GetComponent<Text>().text = text;
	}
	// _______________________________________________________
}
