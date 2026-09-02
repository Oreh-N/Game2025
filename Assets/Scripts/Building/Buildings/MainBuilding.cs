using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : Building, ILootContainer
{
	public Inventory LootCounter { get; set; } = new Inventory() { { LootType.Gold, 500 } };
	List<LootType> _content = new List<LootType>() { LootType.Gold };


	private new void Awake()
	{
		base.Awake();
		data.IsPlaced = true;
		data.Name = "MainBuilding";
		data.Size = new Vector2Int(13, 13);
		data.CellType = MapSpace.Map.CellType.MainBuild;
		data.PanelName = UIManager.PanelNames.MainBuildingP;
	}

	private new void Start()
	{
		base.Start();
		HealthSys.SetHealth(1000);
	}

	private new void Update()
	{
		base.Update();
	}


	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == PubNames.UnitTag)
		{
			var unit = collision.gameObject.GetComponent<Unit>();
			BuildingManager.Instance.MoveLoot(unit, this, _content);
		}
	}




	public Inventory GetInventory()
	{
		return LootCounter;
	}
}
