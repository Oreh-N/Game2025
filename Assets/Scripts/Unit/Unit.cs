using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public abstract class Unit : MonoBehaviour, IAlive, IInteractable, ILootGiver, ILootTaker, IHavePanel, ITeamMember
{
	float IDestructible.Health { get => _health; set => _health = value; }
	string IAlive.Name { get => _unit_name; set => _unit_name = value; }
	GameObject IHavePanel.Panel { get => Panel; set => Panel = value; }

	public Inventory LootCounter { get; set; } = new Inventory();
	public GameObject Panel { get; protected set; }
	public Team Team_ { get; set; }

	// Unit_info___________________
	protected string _unit_name = "Default";
	protected int _holder_capacity = 2;
	protected float _health = 100;
	// ____________________________


	public void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer(PubNames.UnitsLayer);
		gameObject.tag = PubNames.UnitTag;
	}

	public void Start()
	{ 
		GetComponent<Renderer>().material.color = Team_.TeamColor;
		UnitSelectionManager.Instance.AllUnits.Add(gameObject);
	}

	public void Update()
	{
		if (IsOutOfMap(transform.position) || _health <= 0)
		{ Destroy(this); }
	}
	public void OnMouseDown()
	{
		Team_.ChangeInteractableObject(this);
		((IHavePanel)this).ShowPanel();
	}

	private bool IsOutOfMap(Vector3 pos)
	{
		if (pos.y < -5)
		{
			UIManager.Instance.UpdateWarningPanel($"The {_unit_name} fell off a map");
			return true;
		}
		return false;
	}

	public abstract void Interact();


	// Fight____________________________________________________________
	private void OnDestroy()
	{ UnitSelectionManager.Instance.AllUnits.Remove(gameObject); }

	public void TakeDamage(float damage)
	{
		throw new System.NotImplementedException();
	}
	// _________________________________________________________________
}
