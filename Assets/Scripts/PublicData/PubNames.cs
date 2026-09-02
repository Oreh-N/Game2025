using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PubNames
{
	// Layers
	public const string ObstaclesLayer = "Obstacles";
	public const string ClickableLayer = "Clickable";
	public const string BuildingLayer = "Buildings";
	public const string GroundLayer = "Ground";
	public const string UnitsLayer = "Units";


	// Tags
	public const string MainBuildingPanelTag = "MainBuildingPanel";
	public const string WarehousePanelTag = "Warehouse0Panel";
	public const string SpawnerPanelTag = "Spawner0Panel";
	public const string MainBuildingTag = "MainBuilding";
	public const string UpgradePanelTag = "UpgradePanel";
	public const string MoneyPanelTag = "MoneyPanel";
	public const string WoodPanelTag = "WoodPanel";
	public const string WarningPanelTag = "WarningPanel";
	public const string UnitPanelTag = "UnitPanel";
	public const string TopPanelTag = "TopPanel";
	public const string TreeTag = "Tree";
	public const string UnitTag = "Unit";

	// Panels names
	public const string MainBuildingPanelName = "MainBuildingPanel";
	public const string OptionsPanelName = "OptionsPanel";
	public const string UnitPanelName = "SelectedUnitPanel";
	public const string BuildingPanelName = "BuildingPanel";
	public const string SpawnerPanelName = "SpawnerPanel";
	public const string WarehousePanelName = "WarehousePanel";
	public const string MoneyPanelName = "MoneyPanel";
	public const string WoodPanelName = "WoodPanel";
	public const string WarningPanelName = "WarningPanel";
	public const string DefaultPanelName = "DefaultPanel";

	// Folders
	public const string OptButtsF = "OptionsButts";
	public const string BuildButtsF = "BuildingButts";



	public static void TakeChildrenFromFolder(List<GameObject> addToList, string folder)
	{
		var buttsFolder = GameObject.Find(folder);
		var count = buttsFolder.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			var button = buttsFolder.transform.GetChild(i);
			addToList.Add(button.gameObject);
		}
	}
}
