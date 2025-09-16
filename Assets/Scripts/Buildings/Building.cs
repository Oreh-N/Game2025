using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[RequireComponent(typeof(BoxCollider))]
public abstract class Building : MonoBehaviour, IBuilding
{
	Vector3 IBuilding.Position { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	float IDestructible.Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	public bool Placed { get; private set; }
	public Vector3Int Size { get; private set; }
	private Vector3[] _vertices;


	private void Start()
	{
		GetColliderVertexPosition();
		CalculateSizeInCells();
	}

	private void GetColliderVertexPosition()
	{
		BoxCollider box = GetComponent<BoxCollider>();
		_vertices = new Vector3[4];
		_vertices[0] = box.center + new Vector3(-box.size.x, -box.size.y, -box.size.z) * 0.5f;
		_vertices[1] = box.center + new Vector3(box.size.x, -box.size.y, -box.size.z) * 0.5f;
		_vertices[2] = box.center + new Vector3(box.size.x, -box.size.y, box.size.z) * 0.5f;
		_vertices[3] = box.center + new Vector3(-box.size.x, -box.size.y, box.size.z) * 0.5f;
	}

	private void CalculateSizeInCells()
	{
		var vertices = new Vector3Int[_vertices.Length];

		for (int i = 0; i < vertices.Length; i++)
		{
			Vector3 worldPos = transform.TransformPoint(_vertices[i]);
			vertices[i] = BuildingManager.Instance.Grid_.WorldToCell(worldPos);
		}
		Size = new Vector3Int(x:Math.Abs((vertices[0] - vertices[1]).x), 
							  y:Math.Abs((vertices[0] - vertices[3]).y), 
							  z:1);
	}

	public Vector3 GetStartPosition()
	{
		return transform.TransformPoint(_vertices[0]);
	}

	public virtual void Construct()
	{
		var movable = gameObject.GetComponent<Movable>();
		Destroy(movable);
		Placed = true;
	}

	public virtual void Damage(float damage)
	{ }

	public virtual void Destroy()
	{ }
}
