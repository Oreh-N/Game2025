using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour, IAlive, IInteractable
{
	float IDestructible.Health { get => _health; set => _health = value; }
	string IAlive.Name { get => _unit_name; set => _unit_name = value; }

	List<Loot> _lootBag = new List<Loot>();
	string _unit_name = "Default";	//unit name
	float _health;


	private void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer(PubNames.UnitsLayer);
		gameObject.tag = PubNames.UnitTag;
		_lootBag.Add(new Loot(LootType.Tree));
	}
	private void Start()
	{
		UnitSelectionManager.Instance.AddUnit(gameObject.GetComponent<Unit>());
	}

	private void Update()
	{
		if (IsOutOfMap(transform.position))
		{ Destroy(this); }
	}

	private bool IsOutOfMap(Vector3 pos)
	{
		if (pos.y < 0) return true;
		return false;
	}



	// Fight____________________________________________________________
	private void OnDestroy()
	{ UnitSelectionManager.Instance.RemoveUnit(gameObject.GetComponent<Unit>()); }

	public void Damage(float damage)
	{
		throw new System.NotImplementedException();
	}

	public void Destroy()
	{
		Destroy(this);
	}
	// _________________________________________________________________


	// Actions__________________________________________________________
	public List<Loot> GiveAllLoot()
	{
		List<Loot> loot = new List<Loot>();

		for (int i = _lootBag.Count -1; i >= 0; i--)
		{
			loot.Add(new Loot(_lootBag[i].Type));
			_lootBag.RemoveAt(i);
		}

		return loot;
	}

	public void Interact()
	{
		throw new System.NotImplementedException();
	}

	public void Spawn(GameObject obj)
	{
		throw new NotImplementedException();
	}
	// _________________________________________________________________
}
