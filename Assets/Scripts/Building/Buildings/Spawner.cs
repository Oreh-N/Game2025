using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Building
{
	public override string Name => "Spawner0";


	private void Start()
	{
        _panel = UIManager.Instance.GetPanelWithTag(PubNames.SpawnerPanelTag);
	}


	// Actions________________________________________________
	public override void Spawn(GameObject unit)
    {
        var spawn_pos = new Vector3(transform.localPosition.x, transform.position.y,
									transform.localPosition.z - 4);
        var unit_obj = Instantiate(unit, spawn_pos, Quaternion.identity);
		unit_obj.GetComponent<Unit>().SetTeam(TeamColor, TeamName);
    }

	public override void Interact()
	{ }
	// _______________________________________________________
}
