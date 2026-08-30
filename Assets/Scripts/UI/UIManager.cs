using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// Use UI to display information to the player
/// </summary>
public class UIManager : MonoBehaviour
{
	// Should be in the same order panels placed in AllPanels
	public enum PanelNames { MoneyP, WarningP, WoodP, UnitP, MainBuildingP, WarehouseP, SpawnerP, OptionsP, BuildingP }
	public static UIManager Instance;
	UIManagerData data = new UIManagerData();


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

		Setup();
		data.DefaultCursor = Prefabs.DefaultCursor;
		data.DeclineCursor = Prefabs.DeclineCursor;
	}
	private void Start()
	{
		if (GetPanel(PanelNames.WarningP))
		{ GetPanel(PanelNames.WarningP).SetActive(false); }
		else
		{ Debug.Log("Warning panel not found!"); }
		data.Ready = true;
	}

	private void Update()
	{
		if (!MainController.Instance.Ready) return;
		if (!data.FollowedTeam) data.FollowedTeam = Player.Instance;
		UpdateTopPanel();


	}

	void UpdateTopPanel()
	{
		UpdatePanel(PanelNames.MoneyP, data.FollowedTeam.GetInventory()[LootType.Gold].ToString());
		UpdatePanel(PanelNames.WoodP, data.FollowedTeam.GetInventory()[LootType.Wood].ToString());
		
	}

	void Setup()
	{
		// The amount of prefixes MUST BE equel to AllPanels count, at least empty string should be there
		AddPanel(PubNames.MoneyPanelName, "Gold: ");
		AddPanel(PubNames.WarningPanelName, "");
		AddPanel(PubNames.WoodPanelName, "Wood: ");
		AddPanel(PubNames.UnitPanelName, "");
		AddPanel(PubNames.MainBuildingPanelName, "");
		AddPanel(PubNames.WarehousePanelName, "");
		AddPanel(PubNames.SpawnerPanelName, "");
		AddPanel(PubNames.OptionsPanelName, "");
		AddPanel(PubNames.BuildingPanelName, "");

		foreach (var p in data.AllPanels) { p.SetActive(false); }
		data.AlwaysActivePanels = new List<PanelNames>() 
		{PanelNames.MoneyP, PanelNames.WoodP, PanelNames.OptionsP};
		TurnOnInitPanels();
		SetupButtons();
	}

	void SetupButtons()
	{
		Prefabs.AddChildrenFromFolder(data.Buttons, "BuildingButts");
		AssignBuildingButtons(data.Buttons);
		List<GameObject> optButts = new List<GameObject>();
		Prefabs.AddChildrenFromFolder(optButts, PubNames.OptionsPanelName);
		data.Buttons.AddRange(optButts);
		AssignOptionButtons(optButts);
	}

	private void AssignOptionButtons(List<GameObject> optButts)
	{
		optButts[0].GetComponent<Button>().onClick.AddListener(() => EnableDisablePanel(PanelNames.BuildingP));
		optButts[1].GetComponent<Button>().onClick.AddListener(() => EnableDisablePanel(PanelNames.UnitP));
	}

	void AssignBuildingButtons(List<GameObject> buttons)
	{
		foreach (var button in buttons)
		{
			var prefab = Prefabs.NameToPrefab(button.name);
			button.GetComponent<Button>().onClick.AddListener(() => MapController.Instance.SpawnMovableBuild
			(prefab, Player.Instance.GetID()));
		}
	}

	private void TurnOnInitPanels()
	{
		foreach (var pName in data.AlwaysActivePanels)
		{ GetPanel(pName).SetActive(true); }
		GetPanel(PanelNames.BuildingP).SetActive(true);
	}

	void AddPanel(string name, string prefix)
	{
		data.AllPanels.Add(GameObject.Find(name));
		data.Prefixes.Add(prefix);
	}

	public bool Ready() { return data.Ready; }

	// Actions_________________________________________

	public void HideAllPanels()
	{
		for (int i = 0; i < data.AllPanels.Count; i++)
		{
			if (!data.AlwaysActivePanels.Contains((PanelNames)i))
				data.AllPanels[i].SetActive(false); 
		}
	}

	public void UpdatePanel(PanelNames name, string newText)
	{
		var panel = GetPanel(name);
		if (panel == null)
		{ Debug.Log($"{name} panel is null"); return; }
		panel.SetActive(true);
		panel.GetComponent<Text>().text = data.Prefixes[(int)name] + newText;
	}

	// This pannel has unique formatting
	public void UpdateUnitPanel(string unitName, Inventory unitInv)
	{
		UpdatePanel(PanelNames.UnitP, $"Name: {unitName}\n\n\n\nBag: {unitInv}");
	}

	public Team GetFollowedTeam() { return data.FollowedTeam; }

	public void EnableDisablePanel(UIManager.PanelNames panelName)
	{
		var panel = GetPanel(panelName);
		HideAllPanels();

		if (panel.activeSelf)
			panel.SetActive(false);
		else panel.SetActive(true);
	}


	public void ChangeCursor(bool is_default)
	{
		if (is_default)
			Cursor.SetCursor(data.DefaultCursor, new Vector2(0, 0), CursorMode.Auto);
		else
			Cursor.SetCursor(data.DeclineCursor, new Vector2(0, 0), CursorMode.Auto);
		data.Is_default_cursor = is_default;
	}
	// ________________________________________________


	// Database________________________________________
	public GameObject GetPanelWithTag(string tag)
	{
		foreach (var panel in data.AllPanels)
		{
			if (panel && panel.tag == tag)
			{ return panel; }
		}
		return null;
	}

	public GameObject GetPanel(PanelNames name) 
	{ return data.AllPanels[(int)name]; }

	// ________________________________________________
}
