using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, IAlive
{
	float IAlive.Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

	public void Damage(float damage)
	{
		throw new System.NotImplementedException();
	}

	public void Destoy()
	{
		throw new System.NotImplementedException();
	}
}
