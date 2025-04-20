using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAlive : IDestroyable
{
	public float health { get; protected set; }
}
