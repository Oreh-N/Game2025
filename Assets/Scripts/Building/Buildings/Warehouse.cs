using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Warehouse : Building, ILootTaker
{
	public List<LootType> _containment { get; protected set; } = new List<LootType>() { LootType.Wood };
	public Inventory LootCounter { get; set; } = new Inventory() { { LootType.Wood, 0 } };
	public override string Name => "Warehouse0";


	private void Start()
	{
		_panel = UIManager.Instance.GetPanelWithTag(PubNames.WarehousePanelTag);
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
			Inventory givenLoot = ((ILootGiver)unit).GiveAllLoot();
			((ILootTaker)this).TakeSpecificLoot(givenLoot, _containment);	//takes everything, shows only wood
			// Also clicks on building through UI element
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
		_panel.GetComponent<Text>().text = text;
	}
	// _______________________________________________________
}
