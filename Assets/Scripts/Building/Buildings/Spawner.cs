using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Building
{
	private new void Awake()
	{
		base.Awake();
	}

	private new void Start()
	{ 
		base.Start();
        // data.Panel = UIManager.Instance.GetPanelWithTag(PubNames.SpawnerPanelTag);
		Data.Name = "Spawner0";
	}
	private new void Update()
	{ }


	// Actions________________________________________________
	public void Spawn(GameObject unit)
    {
        var spawn_pos = new Vector3(transform.localPosition.x, transform.position.y,
									transform.localPosition.z - 4);
        var unit_obj = Instantiate(unit, spawn_pos, Quaternion.identity);
		((ITeamMember)unit_obj.GetComponent<Unit>()).SetTeam(Data.TeamID);
    }

	public override void Interact()
	{ }

	public override void UpdatePanelInfo()
	{ }
	// _______________________________________________________
}
