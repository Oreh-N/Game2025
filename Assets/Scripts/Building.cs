using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Building : IBuilding
{
	Vector3 IBuilding.Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	public void Construct()
	{
		throw new System.NotImplementedException();
	}

	public void Damage(float damage)
	{
		throw new System.NotImplementedException();
	}

	public void Destoy()
	{
		throw new System.NotImplementedException();
	}
}
