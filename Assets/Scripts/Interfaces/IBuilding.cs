using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilding : IConstructable, IDestructible
{
    public Vector3 Position { get; protected set; }
}
