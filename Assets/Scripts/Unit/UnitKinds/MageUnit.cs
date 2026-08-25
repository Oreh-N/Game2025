using UnityEngine;
using System;


public class MageUnit : UnitSelf<MageUnit>
{
	private new void Awake()
	{
		base.Awake();
		data.Name = "MageUnit";
		data.CellType = MapSpace.Map.CellType.MageUnit;
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
		Debug.Log("Interact not ready yet. MageUNIT");
	}

	public override void UpdatePanelInfo()
	{
		Debug.Log("UpdatePanelInfo not ready yet. MageUNIT");

	}
}