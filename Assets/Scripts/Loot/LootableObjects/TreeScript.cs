using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour, ILootGiver
{
	public List<Loot> Wood { get; private set; } = new List<Loot>();
	public bool IsEmpty { get; private set; } = false;
	int _maxLootCount = 20;


	private void Awake()
	{
		for (int i = 0; i < _maxLootCount; i++)
		{ Wood.Add(new Loot(LootType.Wood)); }
	}

	private void Update()
	{
		if (IsEmpty) Destroy(this);
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
			((ILootTaker)unit).TakeLoot(Wood);

			IsEmpty = true;
		}
	}
}
