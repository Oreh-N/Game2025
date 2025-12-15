using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warehouse : Building, ILootContainer
{
	public List<LootType> _content { get; protected set; } = new List<LootType>() { LootType.Wood };
	public Inventory LootCounter { get; set; } = new Inventory() { { LootType.Wood, 0 } };


	private new void Start()
	{
		base.Start();
		//data.Panel = UIManager.Instance.GetPanelWithTag(PubNames.WarehousePanelTag);
		Data.Name = "Warehouse0";
	}

	private new void Update()
	{
		base.Update();
	}


	// Interaction____________________________________________
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == PubNames.UnitTag)
		{
			var unit = collision.gameObject.GetComponent<Unit>();
			BuildingManager.Instance.MoveLoot(unit, this, _content);
		}
	}
	// _______________________________________________________


	// Visual_________________________________________________
	public override void UpdatePanelInfo()
	{
		string text = "Containment:\n";
		foreach (var item in LootCounter)
		{
			text += Loot.LootNames[(int)item.Key] + " : " + item.Value.ToString() + "\n";
		}
		BuildingManager.ShowMessage(text);
	}

	public Inventory GetInventory()
	{
		return LootCounter;
	}
	// _______________________________________________________
}
