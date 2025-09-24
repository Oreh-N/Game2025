using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	[SerializeField] List<GameObject> _collapsiblePanels;
	GameObject _moneyPanel;
	GameObject _warningBar;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

		_moneyPanel = GameObject.FindWithTag(PubNames.MoneyPanelTag);
		_warningBar = GameObject.FindWithTag(PubNames.WarningPanelTag);
		_warningBar.SetActive(false);
	}

	// Actions_________________________________________
	public void UpdateWarningPanel(string warning)
	{
		if (_warningBar == null) Debug.Log("Warning bar is null");
		_warningBar.SetActive(true);
		_warningBar.GetComponent<Text>().text = warning;
	} 

	public void UpdateMoneyPanel(int new_num)
	{
		if (_moneyPanel == null) Debug.Log("Money bar is null");
		var text = _moneyPanel.GetComponent<Text>();
		text.text = new_num.ToString();
	}

	public void EnableDisablePanel(GameObject panel)
	{
		foreach (var p in _collapsiblePanels)
		{ p.SetActive(false); }

		if (panel.activeSelf)
			panel.SetActive(false);
		else panel.SetActive(true);
	}
	// ________________________________________________


	// Database________________________________________
	public GameObject GetPanelWithTag(string tag)
	{
		foreach (var panel in _collapsiblePanels)
		{
			if (panel.tag == tag)
			{ return panel; }
		}
		return null;
	}
	// ________________________________________________
}
