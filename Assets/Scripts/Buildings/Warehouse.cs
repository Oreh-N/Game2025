using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warehouse : Building
{
    public List<Loot> Loot { get; protected set; } = new List<Loot>();


	private void OnTriggerEnter(Collider other)
	{
		Interact();
	}
}
