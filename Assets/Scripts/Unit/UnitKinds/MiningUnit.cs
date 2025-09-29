using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MiningUnit : Unit
{
	public override string UnitName => "Miner";
	string[] _abilities = new string[2] ;
	Chunk _chunk = new Chunk();


	private new void Awake()
	{
		base.Awake();
		_holder_capacity = 100;
		_abilities[0] = "Mine wood";
		_abilities[1] = "Collect gold";
	}

	private new void Start()
	{
		base.Start();
		_chunk = ForestManager.Instance.GetChunkOnPosition(transform.position);
		ForestManager.Instance.InitializeUnitInChunk(gameObject);
	}

	private new void Update()
	{
		base.Update();
		UpdateChunk();
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
	public override void UpdatePanelInfo()
	{
		Text[] panels = Panel.GetComponentsInChildren<Text>(true);
		panels[0].text = $"Unit name: {UnitName}\nTeam: {Team_.TeamName}\nHealth: {_health}";
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
	{ 
		((IHavePanel)this).ShowPanel();
	}
	// _______________________________________________________
}
