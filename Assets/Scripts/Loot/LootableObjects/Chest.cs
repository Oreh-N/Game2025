using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Chest : LootHolder, IPlaceableOnMap
{
	public Vector3 GetPos()
	{
		return transform.position;
	}

	public Vector2Int GetTakeAreaSize()
	{
		var box = transform.GetComponent<BoxCollider>();
		Vector2Int takeAreaSize = new Vector2Int((int)box.size.x, (int)box.size.z+1);
		return takeAreaSize;
	}

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
