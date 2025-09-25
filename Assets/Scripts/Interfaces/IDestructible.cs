using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDestructible
{
	public float Health { get; protected set; }
	public void TakeDamage(float damage);
}
