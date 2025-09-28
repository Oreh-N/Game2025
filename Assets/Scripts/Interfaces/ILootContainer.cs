using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILootContainer
{
	public Inventory LootCounter { get; set; }

	/// <summary>
	/// Add loot to a loot holder
	/// </summary>
	/// <param name="lootHolder">Where you want to put loot</param>
	/// <param name="lootType"></param>
	/// <param name="lootCount"></param>
	public void AddLootTo(Inventory lootHolder, LootType lootType, int lootCount)
	{
		if (lootHolder.ContainsKey(lootType))
		{ lootHolder[lootType] += lootCount; }
		else
		{ lootHolder.Add(lootType, lootCount); }
	}

	/// <summary>
	/// Move part of loot to another inventory
	/// </summary>
	/// <param name="from">Where you take</param>
	/// <param name="to">Where you put</param>
	/// <param name="part">How much loot you move</param>
	public void MoveLootPart(Inventory from, Inventory to, int part)
	{
		foreach (var item in from)
		{
			if (item.Value < part)
			{
				part -= item.Value;
				AddLootTo(to, item.Key, item.Value);
				from[item.Key] = 0;
			}
			else
			{
				AddLootTo(to, item.Key, item.Value);
				from[item.Key] -= part;
				break;
			}
		}
	}
}
