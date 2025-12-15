using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface ILootContainer
{
	/// <summary>
	/// Add loot to a loot holder
	/// </summary>
	/// <param name="lootHolder">Where you want to put loot</param>
	/// <param name="lootType"></param>
	/// <param name="lootCount"></param>
	public static void AddLootTo(Inventory lootHolder, LootType lootType, int lootCount)
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
	public static void MoveLootPart(Inventory from, Inventory to, int part)
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

	public static void MoveSpecificLoot(Inventory from, Inventory to, List<LootType> requiredLoot)
	{
		// We can't iterate through loot itself if we want to modify it in the same time (not allowed)
		foreach (var key in from.Keys.ToList())
		{
			if (requiredLoot.Contains(key))
			{
				AddLootTo(to, key, from[key]);
				from[key] = 0;
			}
		}
	}
	/// <summary>
	/// Rewrites loot to the new dictionary and clear given loot holder (allLoot)
	/// </summary>
	/// <param name="allLoot">Loot holder (can contain loot of any type)</param>
	/// <returns>Returns a new loot dictionary</returns>
	public static Inventory GiveAllLoot(Inventory from)
	{
		var retLoot = new Inventory();

		foreach (var loot in from)
		{ retLoot.Add(loot.Key, loot.Value); }

		from.Clear();
		return retLoot;
	}

	public static Inventory GiveLootPart(int part, Inventory from)
	{
		var returnLoot = new Inventory();
		MoveLootPart(from, returnLoot, part);
		return returnLoot;
	}

	public static Inventory GiveSpecificLoot(List<LootType> lootForGiving, Inventory from)
	{
		var returnLoot = new Inventory();
		MoveSpecificLoot(from, returnLoot, lootForGiving);
		return returnLoot;
	}
	public static void TakeAllLoot(Inventory loot, Inventory lootHolder)
	{
		foreach (var item in loot)
		{ AddLootTo(lootHolder, item.Key, item.Value); }
		loot.Clear();
	}

	public Inventory GetInventory();
}
