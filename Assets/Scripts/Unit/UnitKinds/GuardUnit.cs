using UnityEngine;
using System;


public class GuardUnit : UnitSelf<GuardUnit>
{
	private new void Awake()
	{
		data.Name = "GuardUnit";
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
		Debug.Log("Interact not ready yet. GuardUNIT");
	}

	public override void UpdatePanelInfo()
	{
		Debug.Log("UpdatePanelInfo not ready yet. GuardUNIT");

	}
}