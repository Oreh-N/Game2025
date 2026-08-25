using UnityEngine;
using MapSpace;


public class BuildingData
{
	public Renderer[] RendererChildren;
	public Map.CellType CellType = Map.CellType.Building;
	public Vector2Int Size;
	public string Name;
	public int PanelID;
	public int TeamID;

	public bool NowInteracting;
	public bool IsPlaced;
}
