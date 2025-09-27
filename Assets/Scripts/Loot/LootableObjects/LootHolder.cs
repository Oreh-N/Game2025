using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootHolder : MonoBehaviour, ILootGiver
{
	public List<Loot> Loot_ { get; protected set; } = new List<Loot>();
	public bool IsEmpty { get; protected set; } = false;
	protected int _maxLootCount = 20;


	public void Update()
	{
		if (IsEmpty) Destroy(this);
	}

	public void FillHolderFully(LootType lootType)
	{
		for (int i = 0; i < _maxLootCount; i++)
		{ Loot_.Add(new Loot(lootType)); }
	}

	public List<Loot> GiveAllLoot(List<Loot> allLoot)
	{
		IsEmpty = true;
		return ((ILootGiver)this).GiveAllLoot(allLoot);
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (IsEmpty) return;

		if (collision.collider.tag == PubNames.UnitTag)
		{
			var unit = collision.gameObject.GetComponent<Unit>();
			((ILootTaker)unit).TakeLoot(Loot_);

			IsEmpty = true;
		}
	}
}
