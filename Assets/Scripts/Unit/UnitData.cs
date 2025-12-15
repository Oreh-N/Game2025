using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class UnitData
{
	public Inventory LootCounter = new Inventory();
	public GameObject Panel;
	public int TeamID;
	
	public bool NowInteracting;
	public string Name;

	public int HolderCapacity = 2;
	public float Health;

}

