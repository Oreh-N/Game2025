using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


/// <summary>
/// Use UI to display information to the player
/// </summary>
public class UIManager : MonoBehaviour, IMouseListener
{
	// Should be in the same order panels placed in AllPanels
	public enum PanelNames { MoneyP, WarningP, WoodP, UnitP, MainBuildingP, WarehouseP, SpawnerP, OptionsP, BuildingP, DefaultP }
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
		StartCoroutine(((IMouseListener)this).StartListening());
		data.Ready = true;
	}

	private void Update()
	{
		if (!MainController.Instance.Ready) return;
		if (!data.FollowedTeam) data.FollowedTeam = Player.Instance;

		UpdateTopPanel();

		if (data.CurrentObj != null)
		{
			var obj = data.CurrentObj;

			if (obj is Unit)
			{ UpdateUnitPanel(obj as Unit); }
			else if (obj is MainBuilding)
			{ UpdateMainBuildPanel(obj as MainBuilding); }
			else if (obj is Warehouse)
			{ UpdatePanel(((Warehouse)obj).GetPanelName(), ((Warehouse)obj).GetInventory().ToString()); }
			else
			{ UpdatePanel(obj); }
		}

	}

	public void SetCurrentObj(IHavePanel obj) { data.CurrentObj = obj; }

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
		AddPanel(PubNames.WarehousePanelName, "Containment:\n");
		AddPanel(PubNames.SpawnerPanelName, "");
		AddPanel(PubNames.OptionsPanelName, "");
		AddPanel(PubNames.BuildingPanelName, "");
		AddPanel(PubNames.DefaultPanelName, "");

		foreach (var p in data.AllPanels) { p.SetActive(false); }
		data.AlwaysActivePanels = new List<PanelNames>()
		{PanelNames.MoneyP, PanelNames.WoodP, PanelNames.OptionsP, PanelNames.WarningP};

		TurnOnInitPanels();
		SetupButtons();
	}

	void SetupButtons()
	{
		PubNames.TakeChildrenFromFolder(data.Buttons, PubNames.BuildButtsF);
		AssignBuildingButtons(data.Buttons);
		List<GameObject> optButts = new List<GameObject>();
		PubNames.TakeChildrenFromFolder(optButts, PubNames.OptionsPanelName);
		data.Buttons.AddRange(optButts);
		AssignOptionButtons(optButts);
	}

	private void AssignOptionButtons(List<GameObject> optButts)
	{
		optButts[0].GetComponent<Button>().onClick.AddListener(() => EnableDisablePanel(PanelNames.BuildingP));
		optButts[1].GetComponent<Button>().onClick.AddListener(() => EnableDisablePanel(PanelNames.UnitP));
		UpdatePanel(PanelNames.WarningP, "Have added functions to option buttons");
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

	public void HidePanels()
	{
		for (int i = 0; i < data.AllPanels.Count; i++)
		{
			if (!data.AlwaysActivePanels.Contains((PanelNames)i))
			{ data.AllPanels[i].SetActive(false); /*Debug.Log($"Hide {(PanelNames)i} panel");*/ }
		}
	}

	public void UpdatePanel(PanelNames name, string newText)
	{
		if (!data.AlwaysActivePanels.Contains(name))
		{ HidePanels(); }

		var panel = GetPanel(name);
		if (panel == null)
		{ Debug.Log($"{name} panel is null"); return; }
		panel.SetActive(true);
		var textComp = panel.GetComponent<Text>();

		if (textComp == null) textComp = panel.GetComponentInChildren<Text>();
		textComp.text = data.Prefixes[(int)name] + newText;
	}

	public void UpdatePanel(IHavePanel obj)
	{
		UpdatePanel(obj.GetPanelName(), $"Name: {obj.GetName()}\nTeam: {obj.GetTeamName()}");
	}

	// This pannel has unique formatting
	public void UpdateUnitPanel(Unit unit)
	{
		UpdatePanel(PanelNames.UnitP, $"Name: {unit.GetName()}\n" +
			$"Team: {unit.GetTeamName()}\nBag: {unit.GetInventory().ToString()}");
	}

	public void UpdateMainBuildPanel(MainBuilding build)
	{
		string text = "";
		text = $"{build.GetName()}\nTeam: {build.GetTeamName()}\nHealth: NO\nBag: {build.GetInventory()}";
		UpdatePanel(build.GetPanelName(), text);
	}

	public Team GetFollowedTeam() { return data.FollowedTeam; }

	public void EnableDisablePanel(PanelNames panelName)
	{
		var panel = GetPanel(panelName);
		HidePanels();

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

	public void MouseHitMapAction(int button)
	{
		if (0 == button)
		{
			var obj = MouseController.Instance.GetObjOnMousePos();
			if (obj != null)
			{ data.CurrentObj = obj.GetComponent<IHavePanel>(); }
			else 
			{ data.CurrentObj = null; }
		}
	}

	// ________________________________________________
}
