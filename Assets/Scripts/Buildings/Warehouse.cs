using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warehouse : Building
{
	List<string> _containment = new List<string>() { "Tree" };
	public Dictionary<LootType, int> LootCount { get; protected set; } = new Dictionary<LootType, int>() { { LootType.Tree, 0 } };


	private void Awake()
	{
		Panel = UIManager.Instance.GetPanelWithTag(PubNames.WarehousePanelTag);
	}

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

	private void UpdatePanelInfo()
	{
		string text = "Containment:\n";
		foreach (var item in LootCount)
		{
			text += _containment[(int)item.Key] + " : " + item.Value.ToString() + "\n";
		}
		Panel.GetComponent<Text>().text = text;
	}
}
