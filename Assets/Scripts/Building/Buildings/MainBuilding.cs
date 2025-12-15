using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : Building, ILootContainer
{
	public Inventory LootCounter { get; set; } = new Inventory() { { LootType.Gold, 500 } };
	List<LootType> _content = new List<LootType>() { LootType.Gold };
	public int BuildingRadius { get; protected set; } = 50;


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

	public void UpgradeBuildingArea()
	{
		GameObject panel = GameObject.FindGameObjectWithTag(PubNames.UpgradePanelTag);
		var text = panel.GetComponent<Text>();

		int upgradePrice;
		if (int.TryParse(text.text, out upgradePrice) && Pay(upgradePrice))
		{
			text.text = ((int)(upgradePrice*1.5)).ToString();
			BuildingRadius += (int)(BuildingRadius * 0.3f);
			BuildingManager.ShowMessage("Building area has increased by 30%");
		}
		else
		{ BuildingManager.ShowMessage("Not enough gold"); }
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (collision.collider.tag == PubNames.UnitTag)
		{
			var unit = collision.gameObject.GetComponent<Unit>();
			BuildingManager.Instance.MoveLoot(unit, this, _content);
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
