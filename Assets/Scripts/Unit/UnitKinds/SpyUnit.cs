using UnityEngine;
using System;


public class SpyUnit : UnitSelf<SpyUnit>
{
	private new void Awake()
	{
		data.Name = "SpyUnit";
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
		Debug.Log("Interact not ready yet. SpyUNIT");
	}

	public override void UpdatePanelInfo()
	{
		Debug.Log("UpdatePanelInfo not ready yet. SpyUNIT");

	}
}