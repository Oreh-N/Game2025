using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlive : IDestructible
{
	public Vector3 Position { get; protected set; }
	public string Name { get; protected set; }
}
