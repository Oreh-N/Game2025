using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public interface ILootGiver : ILootContainer
{
	/// <summary>
	/// Rewrites loot to the new dictionary and clear given loot holder (allLoot)
	/// </summary>
	/// <param name="allLoot">Loot holder (can contain loot of any type)</param>
	/// <returns>Returns a new loot dictionary</returns>
	public Inventory GiveAllLoot()
	{
		var retLoot = new Inventory();

		foreach (var loot in LootCounter)
		{ retLoot.Add(loot.Key, loot.Value); }

		LootCounter.Clear();
		return retLoot;
	}

	public Inventory GiveLootPart(int part)
	{
		var returnLoot = new Inventory();
		MoveLootPart(LootCounter, returnLoot, part);
		return returnLoot;
	}
}
