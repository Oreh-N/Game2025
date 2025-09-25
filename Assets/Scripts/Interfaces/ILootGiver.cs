using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILootGiver
{
	/// <summary>
	/// Rewrites loot to the new list and clear given loot holder (allLoot)
	/// </summary>
	/// <param name="allLoot">Loot holder (can contain loot of any type)</param>
	/// <returns>Returns a new loot list</returns>
	public List<Loot> GiveAllLoot(List<Loot> allLoot)
	{
		List<Loot> retLoot = new List<Loot>();

		foreach (var loot in allLoot)
		{ retLoot.Add(new Loot(loot.Type)); }

		allLoot.Clear();
		return retLoot;
	}
}
