using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warehouse : Building, ILootTaker
{
	public Dictionary<LootType, int> LootCount { get; protected set; } = new Dictionary<LootType, int>() { { LootType.Wood, 0 } };
	public List<string> _containment { get; protected set; } = new List<string>() { "Tree", "Money" };
	public override string Name => "Warehouse0";


	private void Start()
	{
		_panel = UIManager.Instance.GetPanelWithTag(PubNames.WarehousePanelTag);
	}

	void Update()
	{
		UpdatePanelInfo();
	}


	// Interaction____________________________________________
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == PubNames.UnitTag)
		{
			var unit = collision.gameObject.GetComponent<Unit>();
			TakeLoot(((ILootGiver)unit).GiveAllLoot(unit.LootBag));
		}
	}

	public void TakeLoot(List<Loot> loot)
	{
		for (int i = 0; i < loot.Count; i++)
		{
			if (LootCount.ContainsKey(loot[i].Type))
			{ LootCount[loot[i].Type]++; }
			else
			{ LootCount.Add(loot[i].Type, 1); }
		}
	}
	// _______________________________________________________


	// Visual_________________________________________________
	public override void ShowPanel()
	{
		base.ShowPanel();
		UpdatePanelInfo();
	}

	public override void UpdatePanelInfo()
	{
		string text = "Containment:\n";
		foreach (var item in LootCount)
		{
			text += _containment[(int)item.Key] + " : " + item.Value.ToString() + "\n";
		}
		_panel.GetComponent<Text>().text = text;
	}
	// _______________________________________________________
}
