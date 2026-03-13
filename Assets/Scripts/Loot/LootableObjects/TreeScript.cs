using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class TreeScript : LootHolder, IMyPlaceableOnMap
{
	public Vector3 GetPos()
	{
		return transform.position;
	}

	public Vector2Int GetTakeAreaSize()
	{
		return new Vector2Int(1, 1);
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
