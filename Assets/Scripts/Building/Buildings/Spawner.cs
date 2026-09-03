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
		data.Name = "Spawner0";
		data.Size = new Vector2Int(7, 7);
		data.CellType = MapSpace.Map.CellType.Spawner;

	}
	private new void Update()
	{ base.Update(); }


	// Actions________________________________________________
	public void Spawn(GameObject unitPrefab)
    {
        var spawn_pos = new Vector3(transform.localPosition.x, transform.position.y,
									transform.localPosition.z - 4);
        var unit_obj = Creator.CreateUnit(unitPrefab, spawn_pos);
		var unit = unit_obj.GetComponent<Unit>();
		unit.Setup(data.TeamID);
    }

	public override void Interact()
	{ }

	// _______________________________________________________
}
