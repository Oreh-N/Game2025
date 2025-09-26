using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBuilding : Building
{
	public override string Name => "MainBuilding";
    public int BuildingRadius { get; protected set; } = 50;

	private void Awake()
	{
		TeamName = "Nuts";
	}

	public void UpgradeBuildingArea()
	{
		BuildingRadius += (int)(BuildingRadius * 0.3f);
	}
}
