using MapSpace;
using UnityEngine;
using UnityEngine.EventSystems;



public abstract class Building : MonoBehaviour, IInteractable, IConstructable, IHavePanel, ITeamMember, IMyPlaceableOnMap
{
	protected BuildingData data = new BuildingData();
	public HealthSystem HealthSys { get; protected set; } = new HealthSystem();



	public void Awake()
	{
		HealthSys.SetHealth(100);
		data.RendererChildren = GetComponentsInChildren<Renderer>();
		if (data.RendererChildren == null) Debug.Log("No renderers in this building");
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



	public Map.CellType GetCellID()	// Maybe its better to make it interface method (for placeable on map objects like units and buildings)???
	{ return (Map.CellType)(data.TeamID * 100 + (int)data.CellType); }

	public virtual void Construct()
	{
		Team t = MainController.Instance.GetTeam(data.TeamID);
		if (!t) { Debug.Log($"Team {data.TeamID} doesn't exist!"); return; }
		Destroy(gameObject.GetComponent<Movable>());
		ColorBuilding(t.GetColor());

		Map.FillMapAreaSquare(Map.WorldToMap(GetPos()),
			GetSize(), Map.CombineTeamCell(GetCellID(), GetTeamID()) , Map.MapNames.EnvMap);
		data.IsPlaced = true;

		BuildingManager.AddBuilding(this, data.TeamID);
	}


	public void ColorBuilding(Color color)
	{
		if (data.RendererChildren == null) return;
		foreach (Renderer rend in data.RendererChildren)
		{ rend.material.color = color; }
	}

	private void OnDestroy()
	{
		BuildingManager.RemoveBuilding(this, data.TeamID);
	}


	public void MouseDownAct()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (data.IsPlaced)
		{
			BuildingManager.SetInteractableObj(this, data.TeamID);
			BuildingManager.ShowPanel(data.PanelName);
		}
	}

	public virtual void Interact()
	{ BuildingManager.ShowMessage("Building class Interact shouldn't be called"); }

	public virtual void UpdatePanelInfo()
	{ BuildingManager.ShowMessage("Building class UpdatePanelInfo shouldn't be called"); }

	#region Data transfering

	public Map.CellType GetCellType() {  return data.CellType; }
	public bool IsPlaced() { return data.IsPlaced; }

	public int GetTeamID() { return data.TeamID; }

	public string GetName() { return data.Name; }

	public Vector2Int GetSize() { return data.Size; }

	public void SetTeam(int teamID) { data.TeamID = teamID; }

	public Vector3 GetPos() { return transform.position; }

	public Renderer[] GetRendererChildren() { return data.RendererChildren; }

	#endregion 
}
