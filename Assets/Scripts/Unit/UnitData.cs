using UnityEngine;


public class UnitData
{
	public MapSpace.Map.CellType CellType = MapSpace.Map.CellType.Unit;
	public Inventory LootCounter = new Inventory();
	public GameObject Panel;
	public int TeamID;
	
	public bool NowInteracting;
	public string Name;

	public int HolderCapacity = 2;
	public float Health;

}

