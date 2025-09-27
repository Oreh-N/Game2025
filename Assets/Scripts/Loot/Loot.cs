using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LootType { Wood, Gold }
public class Loot
{											// The last loot item in enum 
	public static string[] LootNames = new string[((int)LootType.Gold) + 1] { "Wood", "Gold" };
	public LootType Type { get; protected set; } = LootType.Wood;


	public Loot(LootType type)
	{ Type = type; }
}
