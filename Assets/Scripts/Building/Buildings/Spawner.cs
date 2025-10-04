using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Building
{
	public override string Name => "Spawner0";


	private new void Start()
	{ 
		base.Start();
        _panel = UIManager.Instance.GetPanelWithTag(PubNames.SpawnerPanelTag);
	}
	private new void Update()
	{ }


	// Actions________________________________________________
	public void Spawn(GameObject unit)
    {
        var spawn_pos = new Vector3(transform.localPosition.x, transform.position.y,
									transform.localPosition.z - 4);
        var unit_obj = Instantiate(unit, spawn_pos, Quaternion.identity);
		((ITeamMember)unit_obj.GetComponent<Unit>()).SetTeam(Team_);
    }

	public override void Interact()
	{ }

	public override void UpdatePanelInfo()
	{ }
	// _______________________________________________________
}
