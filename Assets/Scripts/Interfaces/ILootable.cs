using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILootable : IInteractable
{
	public void PutLoot(List<Loot> loot);
	public void TakeLoot();
	public void SpawnLootNear();
}
