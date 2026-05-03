using UnityEngine.EventSystems;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public abstract class Building : MonoBehaviour, IInteractable, IConstructable, IHavePanel, ITeamMember, IMyPlaceableOnMap
{
	protected BuildingData Data = new BuildingData();
	public HealthSystem HealthSys { get; protected set; } = new HealthSystem();



	public void Awake()
	{
		BoxCollider box = GetComponent<BoxCollider>();
		Data.Size = new Vector2Int(Mathf.CeilToInt(box.size.x * transform.localScale.x + 1),
								   Mathf.CeilToInt(box.size.y * transform.localScale.y + 1));
		box.enabled = false;
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
		//UpdatePanelInfo();
		//if (!BuildingManager.TeamIsInteracting(Data.TeamID))
		//{ Data.NowInteracting = false; }
	}

	public virtual void Construct()
	{
		gameObject.GetComponent<BoxCollider>().enabled = true;
		Destroy(gameObject.GetComponent<Movable>());
		ColorBuilding();
		Data.IsPlaced = true;
		BuildingManager.AddBuilding(this, Data.TeamID);

	}

	public virtual void ColorBuilding()
	{
		Color teamColor = BuildingManager.GetTeam(Data.TeamID).GetColor();
		foreach (Renderer rend in Data.RendererChildren)
		{
			foreach (Material mat in rend.materials)
			{
				if (mat.HasProperty("_Color"))
				{ mat.color = teamColor; }
			}
		}
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

	public bool IsPlaced() { return Data.IsPlaced; }

	public int GetTeamID() { return Data.TeamID; }

	public string GetName() { return Data.Name; }

	public Vector2 GetSize() { return Data.Size; }

	public void SetTeam(int teamID) { Data.TeamID = teamID; }

	public Vector2Int GetTakeAreaSize() { return Data.Size; }

	public Vector3 GetPos() { return transform.position; }

	public Renderer[] GetRendererChildren() { return Data.RendererChildren; }

	#endregion 
}
