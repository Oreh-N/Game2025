using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MiningUnit : Unit
{
	string[] _abilities = new string[2] ;
	//Chunk _chunk = new Chunk();


	private new void Awake()
	{
		base.Awake();
		data.Name = "Miner";
		data.HolderCapacity = 100;
		_abilities[0] = "Mine wood";
		_abilities[1] = "Collect gold";
	}

	private new void Start()
	{
		base.Start();
		//_chunk = EnvManager.Instance.GetChunkOnPosition(transform.position);
		//EnvManager.Instance.InitializeUnitInChunk(gameObject);
	}

	private new void Update()
	{
		base.Update();
		//UpdateChunk();
	}

	//private void UpdateChunk()
	//{
	//	Chunk currChunk = EnvManager.Instance.GetChunkOnPosition(transform.position);

	//	if (_chunk != currChunk)
	//	{
	//		EnvManager.Instance.UpdateUnitCountInChunks(gameObject, _chunk);
	//		_chunk = currChunk;
	//	}
	//}


	// Visual_________________________________________________
	public override void UpdatePanelInfo()
	{
		Text[] panels = data.Panel.GetComponentsInChildren<Text>(true);
		panels[0].text = $"Unit name: {data.Name}\nTeam: {BuildingManager.GetTeamName(data.TeamID)}\nHealth: {data.Health}";
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
		foreach (var loot in data.LootCounter)
		{  text += $"{Loot.LootNames[(int)loot.Key]}: {loot.Value}\n"; }

		return text;
	}

	public override void Interact()
	{ 
		((IHavePanel)this).ShowPanel(data.Panel);
	}
	// _______________________________________________________
}
