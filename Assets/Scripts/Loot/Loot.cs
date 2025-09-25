using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LootType { Wood, Money }
public class Loot
{											// The last loot item in enum 
	public static string[] LootNames = new string[((int)LootType.Money) + 1] { "Wood", "Money" };
	public LootType Type { get; protected set; } = LootType.Wood;


	public Loot(LootType type)
	{ Type = type; }
}
