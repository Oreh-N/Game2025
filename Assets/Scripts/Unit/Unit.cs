using UnityEngine;



public abstract class Unit : MonoBehaviour, ILootContainer, IHavePanel, ITeamMember
{
	protected UnitData data = new UnitData();


	public void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer(PubNames.UnitsLayer);
		gameObject.tag = PubNames.UnitTag;
	}

	public void Start()
	{ 
		data.Health = 100;
		//GetComponent<Renderer>().material.color = Team_.TeamColor;
		UnitSelectionManager.Instance.AllUnits.Add(gameObject);
	}

	public void Update()
	{
		if (!MainController.Instance.Ready) return;

		if (IsOutOfMap(transform.position) || data.Health <= 0)
		{
			Destroy(GetComponent<Unit>());
			Destroy(gameObject);
		}
	}

	public virtual void Setup(int teamId) 
	{
		SetTeam(teamId); 
		UnitManager.RegisterUnit(this);
	}


	public virtual void MouseDownAct()
	{

	}

	private bool IsOutOfMap(Vector3 pos)
	{
		if (pos.y < -5)
		{
			UIManager.Instance.UpdatePanel(UIManager.PanelNames.WarningP, $"The {data.Name} fell off a map");
			return true;
		}
		return false;
	}

	public MapSpace.Map.CellType GetUnitCellID()
	{ return (MapSpace.Map.CellType)(data.TeamID * 100 + (int)data.CellType); }

	public abstract void Interact();

	// Fight____________________________________________________________
	private void OnDestroy()
	{
		UnitSelectionManager.Instance.AllUnits.Remove(gameObject);
		UnitSelectionManager.Instance.UnitsSelected.Remove(gameObject);
	}

	public virtual void TakeDamage(float damage)
	{ data.Health -= damage; }

	// _________________________________________________________________
	public int GetTeamID()
	{ return data.TeamID; }

	public MapSpace.Map.CellType GetCellType()
	{ return data.CellType; }

	public Inventory GetInventory()
	{ return data.LootCounter; }

	public void SetTeam(int teamID)
	{ data.TeamID = teamID; }

	public string GetTeamName()
	{ return UnitManager.GetTeamName(data.TeamID); }

	public UIManager.PanelNames GetPanelName()
	{ return data.PanelName; }

	public string GetName()
	{ return data.Name; }
}

public abstract class UnitSelf<TSelf> : Unit where TSelf : Unit { }