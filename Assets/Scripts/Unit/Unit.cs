using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Unit : MonoBehaviour, IAlive, IInteractable, ILootGiver, ILootTaker, IHavePanel
{
	float IDestructible.Health { get => _health; set => _health = value; }
	string IAlive.Name { get => _unit_name; set => _unit_name = value; }
	GameObject IHavePanel.Panel { get => Panel; set => Panel = value; }

	public List<Loot> LootBag { get; private set; } = new List<Loot>();
	public GameObject Panel { get; protected set; }

	protected string _unit_name = "Default";
	protected int _bag_capacity = 2;
	protected float _health;
	protected int _money;


	public void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer(PubNames.UnitsLayer);
		gameObject.tag = PubNames.UnitTag;
		LootBag.Add(new Loot(LootType.Wood));
	}

	public void Start()
	{ UnitSelectionManager.Instance.AddUnit(gameObject); }

	public void Update()
	{
		if (IsOutOfMap(transform.position))
		{ Destroy(this); }
	}

	private bool IsOutOfMap(Vector3 pos)
	{
		if (pos.y < -5) return true;
		return false;
	}

	public void OnMouseDown()
	{
		Player.Instance.ChangeInteractableObject(this);
		ShowPanel();
	}


	// Visual___________________________________________________________
	public virtual void ShowPanel()
	{
		if (Panel == null)
		{ UIManager.Instance.UpdateWarningPanel("Uses Unit panel (not initialising here)"); }
		UIManager.Instance.EnableDisablePanel(Panel);
	}

	public virtual void UpdatePanelInfo()
	{
		if (Panel == null)
		{ UIManager.Instance.UpdateWarningPanel("Uses Unit panel (not initialising here)"); }
	}
	// _________________________________________________________________


	// Fight____________________________________________________________
	private void OnDestroy()
	{ UnitSelectionManager.Instance.RemoveUnit(gameObject); }

	public void TakeDamage(float damage)
	{
		throw new System.NotImplementedException();
	}
	// _________________________________________________________________


	// Actions__________________________________________________________
	public void Interact()
	{
		throw new System.NotImplementedException();
	}

	public void Spawn(GameObject obj)
	{
		throw new NotImplementedException();
	}

	public void TakeLoot(List<Loot> loot)
	{
		for (int i = loot.Count - 1; i >= 0; i--)
		{
			if (LootBag.Count >= _bag_capacity) break;
			LootBag.Add(new Loot(loot[i].Type));
			loot.RemoveAt(i);
		}
	}
	// _________________________________________________________________
}
