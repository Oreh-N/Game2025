using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : LootHolder
{
	void Start()
    {
        FillHolderFully(LootType.Money);
    }

    private new void Update()
    {
        base.Update();
    }
}
