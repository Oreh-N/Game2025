using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IAlive
{
	float IDestructible.Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	Vector3 IAlive.Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	string IAlive.Name { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	public void Damage(float damage)
	{
		throw new System.NotImplementedException();
	}

	public void Destoy()
	{
		throw new System.NotImplementedException();
	}
}
