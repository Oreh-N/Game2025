using UnityEngine;
using System;


public class HealerUnit : UnitSelf<HealerUnit>
{
	private new void Awake()
	{
		data.Name = "HealerUnit";
		base.Awake();
	}

	new void Start()
	{
		base.Start();
	}

	new void Update()
	{
		base.Update();
	}
	public override void Interact()
	{
		Debug.Log("Interact not ready yet. HealerUNIT");
	}

	public override void UpdatePanelInfo()
	{
		Debug.Log("UpdatePanelInfo not ready yet. HealerUNIT");

	}
}