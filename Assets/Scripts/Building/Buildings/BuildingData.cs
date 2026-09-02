using UnityEngine;
using MapSpace;


public class BuildingData
{
	public Renderer[] RendererChildren;
	public Map.CellType CellType = Map.CellType.Building;
	public UIManager.PanelNames PanelName = UIManager.PanelNames.DefaultP;
	public Vector2Int Size;
	public string Name;
	public int TeamID;

	public bool NowInteracting;
	public bool IsPlaced;
}
