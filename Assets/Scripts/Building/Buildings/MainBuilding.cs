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
		Data.IsPlaced = true;
		Data.Name = "MainBuilding";
	}

	private new void Start()
	{
		base.Start();
		HealthSys.SetHealth(1000);
		ColorBuilding();
	}

	private new void Update()
	{
		base.Update();
	}

	public override void ColorBuilding()
	{
		Team t = BuildingManager.GetTeam(Data.TeamID);
		if (!t) return;
		Color buildColor = t.GetColor();
		buildColor.a = 0.5f;
		GetComponentInChildren<Renderer>().material.color = buildColor;
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == PubNames.UnitTag)
		{
			var unit = collision.gameObject.GetComponent<Unit>();
			BuildingManager.Instance.MoveLoot(unit, this, _content);
		}
	}


	// Visual_________________________________________________
	public override void UpdatePanelInfo()
	{
		string text = "";
		text = $"{Data.Name}\nTeam: {UnitManager.GetTeamName(Data.TeamID)}\nHealth: {HealthSys.GetHealth()}\n";
		foreach (var item in LootCounter)
		{
			text += Loot.LootNames[(int)item.Key] + " : " + item.Value.ToString() + "\n";
		}
		BuildingManager.Instance.UpdatePanelText(text, Data.PanelID);
	}

	public Inventory GetInventory()
	{
		return LootCounter;
	}
	// ________________________________________________________
}
