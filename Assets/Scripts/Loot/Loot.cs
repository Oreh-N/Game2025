using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LootType { Tree, Money }
public class Loot 
{
	public LootType Type { get; protected set; } = LootType.Tree;

	public Loot(LootType type)
	{
		Type = type;
	}

}
