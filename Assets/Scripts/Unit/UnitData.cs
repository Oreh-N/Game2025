using UnityEngine;


public class UnitData
{
	public MapSpace.Map.CellType CellType = MapSpace.Map.CellType.Unit;
	public UIManager.PanelNames PanelName = UIManager.PanelNames.UnitP;
	public Inventory LootCounter = new Inventory();
	public int TeamID;
	
	public bool NowInteracting;
	public string Name;

	public int HolderCapacity = 2;
	public float Health;

}

