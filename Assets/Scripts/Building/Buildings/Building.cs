using MapSpace;
using MapSpace.MapLayers;
using System;
using UnityEngine;
using UnityEngine.EventSystems;



public abstract class Building : MonoBehaviour, IInteractable, IConstructable, IHavePanel, ITeamMember, IMyPlaceableOnMap
{
	protected BuildingData Data = new BuildingData();
	public HealthSystem HealthSys { get; protected set; } = new HealthSystem();



	public void Awake()
	{
		//BoxCollider box = GetComponent<BoxCollider>();
		//Data.Size = new Vector2Int(Mathf.CeilToInt(box.size.x * transform.localScale.x + 1),
		//						   Mathf.CeilToInt(box.size.y * transform.localScale.y + 1));
		//box.enabled = false;
		HealthSys.SetHealth(100);
		Data.RendererChildren = GetComponentsInChildren<Renderer>();
		if (Data.RendererChildren == null) Debug.Log("No renderers in this building");
	}

	public void Start()
	{
	}

	public void Update()
	{
		if (HealthSys.GetHealth() <= 0)
		{ Destroy(gameObject); }
		//if (IsOverBuilding()) { }
		//UpdatePanelInfo();
		//if (!BuildingManager.TeamIsInteracting(Data.TeamID))
		//{ Data.NowInteracting = false; }
	}

	private bool IsOverBuilding()
	{
		var mapPos = Map.WorldToMap(MapController.GetMouseWorldPos());
		return true;
	}


	public MapSpace.Map.CellType GetCellID()	// Maybe its better to make it interface method (for placeable on map objects like units and buildings)???
	{ return (MapSpace.Map.CellType)(Data.TeamID * 100 + (int)Data.CellType); }

	public virtual void Construct()
	{
		Team t = MainController.Instance.GetTeam(Data.TeamID);
		if (!t) { Debug.Log($"Team {Data.TeamID} doesn't exist!"); return; }
		Destroy(gameObject.GetComponent<Movable>());
		ColorBuilding(t.GetColor());

		Map.FillMapAreaSquare(Map.WorldToMap(GetPos()),
			GetSize(), Maps.CombineTeamCell(GetCellID(), GetTeamID()) , Maps.MapNames.EnvMap);
		Data.IsPlaced = true;

		BuildingManager.AddBuilding(this, Data.TeamID);
	}


	public void ColorBuilding(Color color)
	{
		if (Data.RendererChildren == null) return;
		foreach (Renderer rend in Data.RendererChildren)
		{ rend.material.color = color; }
	}

	private void OnDestroy()
	{
		BuildingManager.RemoveBuilding(this, Data.TeamID);
	}


	public void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (Data.IsPlaced)
		{
			BuildingManager.SetInteractableObj(this, Data.TeamID);
			BuildingManager.ShowPanel(Data.PanelID);
		}
	}

	public virtual void Interact()
	{ BuildingManager.ShowMessage("Building class Interact shouldn't be called"); }

	public virtual void UpdatePanelInfo()
	{ BuildingManager.ShowMessage("Building class UpdatePanelInfo shouldn't be called"); }

	#region Data transfering

	public Map.CellType GetCellType() {  return Data.CellType; }
	public bool IsPlaced() { return Data.IsPlaced; }

	public int GetTeamID() { return Data.TeamID; }

	public string GetName() { return Data.Name; }

	public Vector2Int GetSize() { return Data.Size; }

	public void SetTeam(int teamID) { Data.TeamID = teamID; }

	public Vector3 GetPos() { return transform.position; }

	public Renderer[] GetRendererChildren() { return Data.RendererChildren; }

	#endregion 
}
