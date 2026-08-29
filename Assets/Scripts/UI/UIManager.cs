using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Use UI to display information to the player
/// </summary>
public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	UIManagerData data = new UIManagerData();
	public bool Ready { get; private set; } = false;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

		FindAllPanels();
		data.DefaultCursor = Prefabs.DefaultCursor;
		data.DeclineCursor = Prefabs.DeclineCursor;
	}

	void FindAllPanels()
	{
		data.AllPanels.Add(GameObject.FindGameObjectWithTag(PubNames.MoneyPanelTag));
		data.MoneyPanel = data.AllPanels[0];
		data.AllPanels.Add(GameObject.FindGameObjectWithTag(PubNames.WarningPanelTag));
		data.WarningPanel = data.AllPanels[1];
		data.AllPanels.Add(GameObject.FindGameObjectWithTag(PubNames.WoodPanelTag));
		data.WoodPanel = data.AllPanels[2];
		data.AllPanels.Add(GameObject.FindGameObjectWithTag(PubNames.UnitPanelTag));
		data.AllPanels.Add(GameObject.FindGameObjectWithTag(PubNames.MainBuildingPanelTag));
		data.AllPanels.Add(GameObject.FindGameObjectWithTag(PubNames.WarehousePanelTag));
	}

	private void Start()
	{
		if (data.WarningPanel)
		{ data.WarningPanel.SetActive(false); }
		else
		{ Debug.Log("Warning panel not found!"); }
		Ready = true;
	}

	private void Update()
	{
		if (!MainController.Instance.Ready) return;
		if (!data.FollowedTeam) data.FollowedTeam = Player.Instance;
		


	}


	// Actions_________________________________________
	public void SetPanelText(string text, int panelID)
	{
		var t = GetPanel(panelID).GetComponentInChildren<Text>();
		t.text = text;
	}

	public void HideAllPanels()
	{
		foreach (GameObject panel in data.AllPanels)
		{ panel.SetActive(false); }
	}

	public void UpdatePanel(int panelID, string newText)
	{
		var panel = GetPanel(panelID);
		if (panel == null)
		{ Debug.Log("Warning panel is null"); return; }
		panel.SetActive(true);
		panel.GetComponent<Text>().text = newText;
	}

	public void UpdateWarningPanel(string warning)
	{
		if (data.WarningPanel == null)
		{ Debug.Log("Warning panel is null"); return; }
		data.WarningPanel.SetActive(true);
		data.WarningPanel.GetComponent<Text>().text = warning;
	}

	public void UpdateMoneyPanel(int new_num)
	{
		if (data.MoneyPanel == null)
		{ Debug.Log("Money panel is null"); return; }
		var text = data.MoneyPanel.GetComponent<Text>();

		text.text = $"Gold: {new_num.ToString()}";
	}

	public void UpdateWoodPanel(int wood_num)
	{
		if (data.MoneyPanel == null)
		{ Debug.Log("Wood panel is null"); return; }
		var text = data.WoodPanel.GetComponent<Text>();
		text.text = $"Wood: {wood_num.ToString()}";
	}

	public void EnableDisablePanel(GameObject panel)
	{
		if (panel == null)
		{ Debug.Log($"{panel.tag} panel is null!"); return; }

		foreach (var p in data.AllPanels)
		{ p.SetActive(false); }

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

	public GameObject GetPanel(int panelID)
	{
		if (data.AllPanels.Count == 0) return null;
		return data.AllPanels[panelID];
	}


	// ________________________________________________
}
