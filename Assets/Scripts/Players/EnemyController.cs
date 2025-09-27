using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Team
{
	private void Awake()
	{
		BaseLocation = new Vector3(360, 0, 400);
		SetTeam(new Color(0.7f, 0.1f, 0.2f), "Velvet");

	}

	private new void Start()
	{
		base.Start();
	}

	private new void Update()
    {
        base.Update();
    }
}
