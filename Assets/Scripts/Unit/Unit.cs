using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour, IAlive, IInteractable
{
	List<Loot> LootBag = new List<Loot>();
	string _unit_name = "Default";	//unit name
	float _health;
	Vector3 _pos;


	float IDestructible.Health { get => _health; set => _health = value; }
	string IAlive.Name { get => _unit_name; set => _unit_name = value; }

	private void Awake()
	{
		gameObject.layer = LayerMask.NameToLayer(PubNames.UnitsLayer);
		gameObject.tag = PubNames.UnitTag;
	}
	private void Start()
	{
		UnitSelectionManager.Instance.AddUnit(gameObject.GetComponent<Unit>());
		
	}

	private void Update()
	{
		if (IsOutOfMap(_pos)) Destroy();
		
	}

	private bool IsOutOfMap(Vector3 pos)
	{
		if (pos.y < 0) return true;
		return false;
	}

	public List<Loot> GiveAllLoot()
	{
		List<Loot> loot = new List<Loot>();
		for (int i = LootBag.Count -1; i >= 0; i--)
		{
			loot.Add(new Loot(LootBag[i].Type));
			LootBag.RemoveAt(i);
		}

		return loot;
	}

	private void OnDestroy()
	{
		UnitSelectionManager.Instance.RemoveUnit(gameObject.GetComponent<Unit>());
	}

	public void Damage(float damage)
	{
		throw new System.NotImplementedException();
	}

	public void Destroy()
	{
		Destroy(this);
	}

	public void Interact()
	{
		throw new System.NotImplementedException();
	}

	public void Select()
	{
		throw new System.NotImplementedException();
	}

	public void Spawn(GameObject obj)
	{
		throw new NotImplementedException();
	}
}
