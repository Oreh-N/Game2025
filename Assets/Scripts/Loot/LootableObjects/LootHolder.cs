using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LootHolder : MonoBehaviour, ILootGiver
{
	public Inventory LootCounter { get; set; } = new Inventory();
	public bool IsEmpty { get; protected set; } = false;
	protected int _maxLootCount = 20;


	public void Update()
	{
		if (IsEmpty) Destroy(this);
	}

	public void FillHolderFully(LootType lootType)
	{
		LootCounter.Add(lootType,_maxLootCount); 
	}

	public Inventory GiveAllLoot()
	{
		IsEmpty = true;

		return ((ILootGiver)this).GiveAllLoot();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (IsEmpty) return;

		if (collision.collider.tag == PubNames.UnitTag)
		{
			var unit = collision.gameObject.GetComponent<Unit>();
			((ILootTaker)unit).TakeAllLoot(LootCounter);

			IsEmpty = true;
		}
	}
}
