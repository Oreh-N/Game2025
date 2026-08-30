using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Dictionary<LootType, int>
{ 
	public void AddAllLootTypes()
	{
		Add(LootType.Gold, 0);
		Add(LootType.Wood, 0);
	}

	public override string ToString() 
	{
		string strInv = "";
		foreach (var loot in this)
		{ strInv += $"{loot.Key}: {loot.Value}\n"; }
		return strInv;
	}
}
