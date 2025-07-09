using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : IBuilding
{
	Vector3 IBuilding.Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	float IDestructible.Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	public void Construct()
	{
		throw new System.NotImplementedException();
	}

	public void Damage(float damage)
	{
		throw new System.NotImplementedException();
	}

	public void Destroy()
	{
		throw new System.NotImplementedException();
	}
}
