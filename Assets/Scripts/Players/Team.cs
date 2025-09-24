using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Team : MonoBehaviour
{
    public List<Building> Buildings { get; protected set; } = new List<Building>();
    public List<IAlive> Members { get; protected set; } = new List<IAlive>();
    public string Name { get; protected set; }
    public Color Color { get; protected set; }

    
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
}
