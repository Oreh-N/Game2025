using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


public class BuildingData
{
	public Renderer[] RendererChildren;
	public Vector2Int Size;
	public string Name;
	public int PanelID;
	public int TeamID;

	public bool NowInteracting;
	public bool IsPlaced;
}
