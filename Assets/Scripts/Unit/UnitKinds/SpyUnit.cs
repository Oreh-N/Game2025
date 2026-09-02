using UnityEngine;
using System;


public class SpyUnit : UnitSelf<SpyUnit>
{
	private new void Awake()
	{
		base.Awake();
		data.Name = "SpyUnit";
		data.CellType = MapSpace.Map.CellType.SpyUnit;
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

}