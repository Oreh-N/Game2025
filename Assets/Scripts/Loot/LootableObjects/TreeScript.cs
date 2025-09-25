using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeScript : MonoBehaviour, ILootGiver
{
	public List<Loot> Wood { get; private set; } = new List<Loot>();
	int _maxLootCount = 20;
	bool _is_empty = false;


	private void Awake()
	{
		for (int i = 0; i < _maxLootCount; i++)
		{ Wood.Add(new Loot(LootType.Wood)); }
	}

	private void Update()
	{
		if (_is_empty) Destroy(this);
	}

	public List<Loot> GiveAllLoot(List<Loot> allLoot)
	{
		_is_empty = true;
		return ((ILootGiver)this).GiveAllLoot(allLoot);
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == PubNames.UnitTag)
		{
			var unit = other.gameObject.GetComponent<Unit>();
			((ILootTaker)unit).TakeLoot(Wood);

			_is_empty = true;
		}
	}
}
