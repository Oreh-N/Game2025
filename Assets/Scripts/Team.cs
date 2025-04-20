using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Team : MonoBehaviour
{
    public List<Unit> units { get; protected set; } = new List<Unit>();
    public string Name { get; protected set; }
    public Color Color { get; protected set; }

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
