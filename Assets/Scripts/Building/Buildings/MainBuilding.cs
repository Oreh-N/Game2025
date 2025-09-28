using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : Building, ILootTaker
{
	public Inventory LootCounter { get; set; } = new Inventory() { { LootType.Gold, 500 } };

	public int BuildingRadius { get; protected set; } = 50;
	public override string Name => "MainBuilding";


	private void Awake()
	{
		_health = 1000;
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
		}
	}

	public void Pay(int price)
	{
		if (LootCounter.ContainsKey(LootType.Gold) && LootCounter[LootType.Gold] >= price)
		{ LootCounter[LootType.Gold] -= price; }
		else { UIManager.Instance.UpdateWarningPanel("Not enough gold"); }
	}

	public void Earn(int money) 
	{
		if (LootCounter.ContainsKey(LootType.Gold))
		{ LootCounter[LootType.Gold] += money; }
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
		UIManager.Instance.UpdateMoneyPanel(LootCounter[LootType.Gold]);
	}
	// ________________________________________________________
}
