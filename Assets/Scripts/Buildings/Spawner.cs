using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : Building
{

	private void Awake()
	{
        Panel = UIManager.Instance.GetPanelWithTag(PubNames.SpawnerPanelTag);
	}

	// Start is called before the first frame update
	void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

	}

	public override void Spawn(GameObject unit)
    {
        var spawn_pos = new Vector3(transform.localPosition.x, transform.position.y,
									transform.localPosition.z - 4);
        Instantiate(unit, spawn_pos, Quaternion.identity);

    }

	public override void Interact()
	{
		
	}
}
