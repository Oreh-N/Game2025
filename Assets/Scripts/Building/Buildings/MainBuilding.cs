using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : Building, ILootTaker
{
	public Inventory LootCounter { get; set; } = new Inventory() { { LootType.Gold, 0 } };

	public int BuildingRadius { get; protected set; } = 50;
	public override string Name => "MainBuilding";
	public Wallet Wallet_ { get; protected set; } = new Wallet(500);


	private void Awake()
	{
		Placed = true;
	}

	private void Start()
	{
		_panel = UIManager.Instance.GetPanelWithTag(PubNames.MainBuildingPanelTag);
		Color buildColor = Team_.TeamColor;
		buildColor.a = 0.5f;
		GetComponentInChildren<Renderer>().material.color = buildColor;
	}

	private new void Update()
	{
		base.Update();
	}

	public void UpgradeBuildingArea()
	{
		BuildingRadius += (int)(BuildingRadius * 0.3f);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == PubNames.UnitTag)
		{
			var unit = collision.gameObject.GetComponent<Unit>();
			Inventory givenLoot = ((ILootGiver)unit).GiveAllLoot();
			((ILootTaker)this).TakeSpecificLoot(givenLoot, new List<LootType>() { LootType.Gold });
			Wallet_.Earn(LootCounter[LootType.Gold]);	//each time more money, even if already given
		}
	}


	// Visual_________________________________________________
	public override void UpdatePanelInfo()
	{
		Text text = _panel.GetComponentInChildren<Text>();
		text.text = $"{Name}\nTeam: {Team_.TeamName}\nHealth: {_health}\n";
		foreach (var item in LootCounter)
		{
			text.text += Loot.LootNames[(int)item.Key] + " : " + item.Value.ToString() + "\n";
		}
		UIManager.Instance.UpdateMoneyPanel(Wallet_.Money);
	}
	// ________________________________________________________
}
