using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MiningUnit : Unit
{
	Dictionary<LootType, int> _bag_containment = new Dictionary<LootType, int>();
	string[] _abilities = new string[1] ;
	Chunk _chunk = new Chunk();

	private new void Awake()
	{
		base.Awake();
		_bag_capacity = 100;
		_unit_name = "Miner";
		_abilities[0] = "Mine wood";
	}

	private new void Start()
	{
		base.Start();
		_chunk = ForestManager.Instance.GetChunkOnPosition(transform.position);
		ForestManager.Instance.InitializeUnitInChunk(gameObject);
		Panel = UIManager.Instance.GetPanelWithTag(PubNames.UnitPanelTag);

	}

	private new void Update()
	{
		base.Update();
		UpdateChunk();
		UpdatePanelInfo();
	}

	private void UpdateChunk()
	{
		Chunk currChunk = ForestManager.Instance.GetChunkOnPosition(transform.position);

		if (_chunk != currChunk)
		{
			ForestManager.Instance.UpdateUnitCountInChunks(gameObject, _chunk);
			_chunk = currChunk;
		}
	}


	// Visual_________________________________________________
	public override void ShowPanel()
	{
		base.ShowPanel();
		UpdatePanelInfo();
	}

	private new void UpdatePanelInfo()
	{
		Text[] panels = Panel.GetComponentsInChildren<Text>(true);
		panels[0].text = $"Unit name: {_unit_name}\nTeam: {TeamName}";
		panels[1].text = BagContainmentToString();
		Button[] buttons = panels[2].gameObject.GetComponentsInChildren<Button>();
		for (int i = 0; i < buttons.Length; i++)
		{
			if (i < _abilities.Count())
			{ buttons[i].GetComponentInChildren<Text>().text = _abilities[i]; }
			else
			{ buttons[i].gameObject.SetActive(false); }
		}
	}

	private void RecalculateBagContainment()
	{
		_bag_containment.Clear();

		foreach (var loot in LootBag)
		{
			if (_bag_containment.ContainsKey(loot.Type))
			{ _bag_containment[loot.Type]++; }
			else
			{ _bag_containment.Add(loot.Type, 1); }
		}
	}

	private string BagContainmentToString()
	{
		RecalculateBagContainment();
		string text = "Bag:\n";
		foreach (var loot in _bag_containment)
		{  text += $"{Loot.LootNames[(int)loot.Key]}: {loot.Value}"; }

		return text;
	}
	// _______________________________________________________
}
