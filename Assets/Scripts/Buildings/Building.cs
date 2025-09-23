using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;



[RequireComponent(typeof(BoxCollider))]
public abstract class Building : MonoBehaviour, IBuilding
{
	Vector3 IBuilding.Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	float IDestructible.Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	public bool Placed { get; private set; }
	public Vector3Int Size { get; private set; }


	private void Start()
	{
		BoxCollider box = GetComponent<BoxCollider>();
		box.enabled = false;
		Size = new Vector3Int(Mathf.CeilToInt(box.size.x),
							  Mathf.CeilToInt(box.size.y),
							  Mathf.CeilToInt(box.size.z));
		Debug.Log(box.size);
		Debug.Log("Box");
		Debug.Log(Size);
	}

	public virtual void Construct()
	{
		gameObject.AddComponent<NavMeshObstacle>();
		var meshObstacle = gameObject.GetComponent<NavMeshObstacle>();
		meshObstacle.carveOnlyStationary = false;
		meshObstacle.carving = true;
		gameObject.GetComponent<BoxCollider>().enabled = true;
		var movable = gameObject.GetComponent<Movable>();
		Destroy(movable);
		Placed = true;
	}

	public virtual void Damage(float damage)
	{ }

	public virtual void Destroy()
	{ }
}
