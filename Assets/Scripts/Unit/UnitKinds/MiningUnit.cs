using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MiningUnit : Unit
{
	string[] _abilities = new string[1] ;
	Chunk _chunk = new Chunk();

	private new void Awake()
	{
		base.Awake();
		_holder_capacity = 100;
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
	public void UpdatePanelInfo()
	{
		((IHavePanel)this).UpdatePanelInfo();
		Text[] panels = Panel.GetComponentsInChildren<Text>(true);
		panels[0].text = $"Unit name: {_unit_name}\nTeam: {Team_.TeamName}";
		panels[1].text = InventoryContentToStr();
		Button[] buttons = panels[2].gameObject.GetComponentsInChildren<Button>();
		for (int i = 0; i < buttons.Length; i++)
		{
			if (i < _abilities.Count())
			{ buttons[i].GetComponentInChildren<Text>().text = _abilities[i]; }
			else
			{ buttons[i].gameObject.SetActive(false); }
		}
	}

	private string InventoryContentToStr()
	{
		string text = "Bag:\n";
		foreach (var loot in LootCounter)
		{  text += $"{Loot.LootNames[(int)loot.Key]}: {loot.Value}\n"; }

		return text;
	}

	public override void Interact()
	{ ((IHavePanel)this).ShowPanel(); }
	// _______________________________________________________
}
