using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlive : IDestructible
{
	public string Name { get; protected set; }
}
