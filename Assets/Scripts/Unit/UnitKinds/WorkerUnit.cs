using UnityEngine;
using System;


public class WorkerUnit : UnitSelf<WorkerUnit>
{
	private new void Awake()
	{
		data.Name = "WorkerUnit";
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
		Debug.Log("Interact not ready yet. WorkerUNIT");
	}

	public override void UpdatePanelInfo()
	{
		Debug.Log("UpdatePanelInfo not ready yet. WorkerUNIT");

	}
}
