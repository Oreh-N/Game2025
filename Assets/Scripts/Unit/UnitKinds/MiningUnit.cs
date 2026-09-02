using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MiningUnit : UnitSelf<MiningUnit>
{
	string[] _abilities = new string[2] ;


	private new void Awake()
	{
		base.Awake();
		data.Name = "Miner";
		data.HolderCapacity = 100;
		_abilities[0] = "Mine wood";
		_abilities[1] = "Collect gold";
		data.CellType = MapSpace.Map.CellType.MiningUnit;
	}

	private new void Start()
	{
		base.Start();
	}

	private new void Update()
	{
		base.Update();
	}


	public override void Interact()
	{ 
	}
	// _______________________________________________________
}
