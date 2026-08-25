using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildManagerData
{
	public List<GameObject> Buttons = new List<GameObject>();
	public Renderer[] Childrens_rends;
	public LayerMask Obstacles;
	public bool Ready = false;
}

