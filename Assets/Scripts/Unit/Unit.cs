using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public abstract class Unit : MonoBehaviour, IInteractable, ILootContainer, IHavePanel, ITeamMember
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
		data.Panel = UIManager.Instance.GetPanelWithTag(PubNames.UnitPanelTag);
	}

	public void Update()
	{
		if (IsOutOfMap(transform.position) || data.Health <= 0)
		{
			Destroy(GetComponent<UnitMovement>());
			Destroy(gameObject);
		}
		if (data.Panel != null && !data.Panel.activeSelf)
		{ data.NowInteracting = false; }
		if (data.NowInteracting && UnitManager.GetTeamName(data.TeamID) == UnitManager.GetPlayerTeamName())
		{ UpdatePanelInfo(); }
	}


	public void OnMouseDown()
	{
		UnitManager.GetTeam(data.TeamID).ChangeInteractableObject(this);
		((IHavePanel)this).ShowPanel(data.Panel);
	}

	private bool IsOutOfMap(Vector3 pos)
	{
		if (pos.y < -5)
		{
			UIManager.Instance.UpdateWarningPanel($"The {data.Name} fell off a map");
			return true;
		}
		return false;
	}

	public abstract void Interact();

	// Fight____________________________________________________________
	private void OnDestroy()
	{
		UnitSelectionManager.Instance.AllUnits.Remove(gameObject);
		UnitSelectionManager.Instance.UnitsSelected.Remove(gameObject);
	}

	public virtual void TakeDamage(float damage)
	{ data.Health -= damage; }

	public abstract void UpdatePanelInfo();

	public Inventory GetInventory()
	{
		return data.LootCounter;
	}

	public void SetTeam(int teamID)
	{
		data.TeamID = teamID;
	}

	public string GetTeamName()
	{
		return UnitManager.GetTeamName(data.TeamID);
	}

	// _________________________________________________________________
}

public abstract class UnitSelf<TSelf> : Unit where TSelf : Unit { }