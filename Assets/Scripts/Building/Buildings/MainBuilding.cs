using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainBuilding : Building
{
    public int BuildingRadius { get; protected set; } = 50;
	public override string Name => "MainBuilding";


	private void Awake()
	{
		Placed = true;
	}

	private void Start()
	{
		_panel = UIManager.Instance.GetPanelWithTag(PubNames.MainBuildingPanelTag);
		Color buildColor = TeamColor;
		buildColor.a = 0.5f;
		GetComponentInChildren<Renderer>().material.color = buildColor;
	}

	private void Update()
	{
		UpdatePanelInfo();
	}

	public void UpgradeBuildingArea()
	{
		BuildingRadius += (int)(BuildingRadius * 0.3f);
	}



	// Visual_________________________________________________
	public override void ShowPanel()
	{
		base.ShowPanel();
		UpdatePanelInfo();
	}

	private new void UpdatePanelInfo()
	{
		Text text = _panel.GetComponentInChildren<Text>();
		text.text = $"{Name}\nTeam: {TeamName}\nHealth: {_health}";
	}
	// ________________________________________________________
}
