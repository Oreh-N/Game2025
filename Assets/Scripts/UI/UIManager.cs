using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;


/// <summary>
/// Use UI to display information to the player
/// </summary>
public class UIManager : MonoBehaviour
{
	public static UIManager Instance;
	UIManagerData data = new UIManagerData();

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }


		data.MoneyPanel = GameObject.FindGameObjectWithTag(PubNames.MoneyPanelTag);
		data.WarningPanel = GameObject.FindGameObjectWithTag(PubNames.WarningPanelTag);
		data.WoodPanel = GameObject.FindGameObjectWithTag(PubNames.WoodPanelTag);
		data.DefaultCursor = Resources.Load<Texture2D>("My2DAssets/Cursors/DefaultCursor0.png");
		data.DeclineCursor = Resources.Load<Texture2D>("My2DAssets/Cursors/DeclineCursor.png");
	}

	private void Start()
	{
		if (data.WarningPanel)
		{ data.WarningPanel.SetActive(false); }
		else
		{ Debug.Log("Warning panel not found!"); }
	}


	// Actions_________________________________________
	public void SetPanelText(string text, int panelID)
	{
		var t = GetPanel(panelID).GetComponentInChildren<Text>();
		t.text = text;
	}

	public void HideAllPanels()
	{
		foreach (GameObject panel in data._allPanels)
		{ panel.SetActive(false); }
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

		foreach (var p in data._allPanels)
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
		foreach (var panel in data._allPanels)
		{
			if (panel.tag == tag)
			{ return panel; }
		}
		return null;
	}

	public GameObject GetPanel(int panelID)
	{
		return data._allPanels[panelID];
	}


	// ________________________________________________
}
