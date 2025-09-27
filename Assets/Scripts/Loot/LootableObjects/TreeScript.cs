using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : LootHolder
{
	private void Awake()
	{
		FillHolderFully(LootType.Wood);
	}

	private new void Update()
	{
		base.Update();
	}

}
