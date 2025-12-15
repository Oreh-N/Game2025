using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class TreeScript : LootHolder, IPlaceableOnMap
{
	public Vector3 GetPos()
	{
		return transform.position;
	}

	public Vector2Int GetTakeAreaSize()
	{
		var box = transform.GetComponent<BoxCollider>();
		Vector2Int takeAreaSize = new Vector2Int((int)box.size.x+1, (int)box.size.z + 1);
		return takeAreaSize;
	}

	private void Awake()
	{
		FillHolderFully(LootType.Wood);
	}

	private new void Update()
	{
		base.Update();
	}

}
