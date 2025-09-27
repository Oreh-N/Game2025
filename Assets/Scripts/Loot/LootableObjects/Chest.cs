using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : LootHolder
{
	void Start()
    {
        FillHolderFully(LootType.Gold);
    }

    private new void Update()
    {
        base.Update();
    }
}
