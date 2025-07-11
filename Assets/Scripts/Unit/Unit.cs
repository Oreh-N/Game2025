using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class Unit : MonoBehaviour, IAlive, IInteractable
{
	enum Task {Rest, MineWood, Build, Destroy, Attack } 

	float health;
	Vector3 pos;
	string u_name = "Default";  //unit name
	Task task;

	float IDestructible.Health { get => health; set => health = value; }
	Vector3 IAlive.Position { get => pos; set => pos = value; }
	string IAlive.Name { get => u_name; set => u_name = value; }



	private void Start()
	{
		UnitSelectionManager.Instance.allUnits.Add(gameObject);
	}

	private void Update()
	{
		if (IsOutOfMap(pos)) Destroy();
		
	}

	private bool IsOutOfMap(Vector3 pos)
	{
		if (pos.y < 0) return true;
		return false;
	}

	private void OnDestroy()
	{
		UnitSelectionManager.Instance.allUnits.Remove(gameObject);
	}

	public void Damage(float damage)
	{
		throw new System.NotImplementedException();
	}

	public void Destroy()
	{
		UnityEngine.Object.Destroy(this);
	}

	public void Interact()
	{
		throw new System.NotImplementedException();
	}

	public void Select()
	{
		throw new System.NotImplementedException();
	}
}
