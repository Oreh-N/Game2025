using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Progress;

public interface ILootTaker : ILootContainer
{
	public void TakeAllLoot(Inventory loot)
	{
		foreach (var item in loot)
		{ AddLootTo(LootCounter, item.Key, item.Value); }
		loot.Clear();
	}

	/// <summary>
	/// Takes part of the first going loot kinds and leave the rest. 
	/// May take less loot if there is little loot.
	/// </summary>
	/// <param name="loot">The loot you are taking from</param>
	/// <param name="part">How many entities will be taken</param>
	public void TakeLootPart(Inventory loot, int part)
	{
		MoveLootPart(loot, LootCounter, part);
	}
	/// <summary>
	/// Take only required loot
	/// </summary>
	/// <param name="loot">Loot will be taken from this object</param>
	/// <param name="lootForTaking"></param>
	public void TakeSpecificLoot(Inventory loot, List<LootType> lootForTaking)
	{	// We can't iterate through loot itself if we want to modify it in the same time (not allowed)
		foreach (var key in loot.Keys.ToList())
		{
			if (lootForTaking.Contains(key))
			{
				AddLootTo(LootCounter, key, loot[key]); 
				loot[key] = 0;
			}
		}
	}
}
