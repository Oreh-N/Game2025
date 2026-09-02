using UnityEngine;
using System;


public class GuardUnit : UnitSelf<GuardUnit>
{
	private new void Awake()
	{
		base.Awake();
		data.Name = "GuardUnit";
		data.CellType = MapSpace.Map.CellType.GuardUnit;
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

}