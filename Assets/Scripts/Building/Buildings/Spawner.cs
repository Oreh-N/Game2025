using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MapSpace;

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
		Data.Size = new Vector2Int(7, 7);
		Data.CellType = MapSpace.Map.CellType.Spawner;

	}
	private new void Update()
	{ }


	// Actions________________________________________________
	public void Spawn(GameObject unit)
    {
        var spawn_pos = new Vector3(transform.localPosition.x, transform.position.y,
									transform.localPosition.z - 4);
        var unit_obj = Creator.CreateUnit(unit, spawn_pos);
		((ITeamMember)unit_obj.GetComponent<Unit>()).SetTeam(Data.TeamID);
    }

	public override void Interact()
	{ }

	public override void UpdatePanelInfo()
	{ }
	// _______________________________________________________
}
