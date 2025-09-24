using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Building
{
	private void Start()
	{
        Panel = UIManager.Instance.GetPanelWithTag(PubNames.SpawnerPanelTag);
	}


	// Actions________________________________________________
	public override void Spawn(GameObject unit)
    {
        var spawn_pos = new Vector3(transform.localPosition.x, transform.position.y,
									transform.localPosition.z - 4);
        Instantiate(unit, spawn_pos, Quaternion.identity);

    }

	public override void Interact()
	{ }
	// _______________________________________________________
}
