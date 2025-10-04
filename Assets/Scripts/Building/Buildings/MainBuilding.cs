using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : Building, ILootTaker
{
	public Inventory LootCounter { get; set; } = new Inventory() { { LootType.Gold, 500 } };
	List<LootType> _content = new List<LootType>() { LootType.Gold };
	public int BuildingRadius { get; protected set; } = 50;
	public override string Name => "MainBuilding";


	private new void Awake()
	{
		base.Awake();
		_health = 1000;
		Placed = true;
	}

	private new void Start()
	{
		base.Start();
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
		GameObject panel = GameObject.FindGameObjectWithTag(PubNames.UpgradePanelTag);
		var text = panel.GetComponent<Text>();

		int upgradePrice;
		if (int.TryParse(text.text, out upgradePrice) && Pay(upgradePrice))
		{
			text.text = ((int)(upgradePrice*1.5)).ToString();
			BuildingRadius += (int)(BuildingRadius * 0.3f); 
			UIManager.Instance.UpdateWarningPanel("Building area has increased by 30%");
		}
		else
		{ UIManager.Instance.UpdateWarningPanel("Not enough gold"); }
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == PubNames.UnitTag)
		{
			var unit = collision.gameObject.GetComponent<Unit>();
			ILootContainer.MoveSpecificLoot(unit.LootCounter, LootCounter, _content);
		}
	}
	/// <summary>
	/// Pays from the team's gold reserves
	/// </summary>
	/// <param name="price"></param>
	/// <returns>true if the payment was successful, otherwise returns false</returns>
	public bool Pay(int price)
	{
		if (LootCounter.ContainsKey(LootType.Gold) && LootCounter[LootType.Gold] >= price)
		{
			LootCounter[LootType.Gold] -= price;
			return true;
		}
		else
		{
			UIManager.Instance.UpdateWarningPanel("Not enough gold");
			return false;
		}
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
