using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : LootHolder
{
	private void Awake()
	{
        _maxLootCount = 200;
	}
	void Start()
    {
        FillHolderFully(LootType.Gold);
    }

    private new void Update()
    {
        base.Update();
    }
}
