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
		data.Name = "Warehouse0";
		data.Size = new Vector2Int(7, 7);
		data.CellType = MapSpace.Map.CellType.Warehouse;
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



	public Inventory GetInventory()
	{
		return LootCounter;
	}
}
