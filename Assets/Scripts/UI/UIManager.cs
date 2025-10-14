using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;

	[SerializeField] List<GameObject> _collapsiblePanels;
	[SerializeField] GameObject _moneyPanel;
	[SerializeField] GameObject _warningPanel;
	[SerializeField] GameObject _woodPanel;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	private void Start()
	{
		_warningPanel.SetActive(false);
	}


	// Actions_________________________________________
	public void HideAllPanels()
	{	
		foreach (GameObject panel in _collapsiblePanels)
		{ panel.SetActive(false); }
	}

	public void UpdateWarningPanel(string warning)
	{
		if (_warningPanel == null) 
		{ Debug.Log("Warning panel is null"); return; }
		_warningPanel.SetActive(true);
		_warningPanel.GetComponent<Text>().text = warning;
	} 

	public void UpdateMoneyPanel(int new_num)
	{
		if (_moneyPanel == null) 
		{ Debug.Log("Money panel is null"); return; }
		var text = _moneyPanel.GetComponent<Text>();

		text.text = $"Gold: {new_num.ToString()}";
	}

	public void UpdateWoodPanel(int wood_num)
	{
		if (_moneyPanel == null)
		{ Debug.Log("Wood panel is null"); return; }
		var text = _woodPanel.GetComponent<Text>();
		text.text = $"Wood: {wood_num.ToString()}";
	}

	public void EnableDisablePanel(GameObject panel)
	{
		if (panel == null) 
		{ Debug.Log($"{panel.tag} panel is null!"); return; }

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
