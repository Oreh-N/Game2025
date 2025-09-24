using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;



[RequireComponent(typeof(BoxCollider))]
public abstract class Building : MonoBehaviour, IInteractable, IConstructable, IDestructible
{
	public GameObject Panel { get; protected set; }
	float IDestructible.Health { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
	public bool Placed { get; private set; }
	public Vector3Int Size { get; private set; }

	private void Awake()
	{
		BoxCollider box = GetComponent<BoxCollider>();
		box.enabled = false;
		Size = new Vector3Int(Mathf.CeilToInt(box.size.x * transform.localScale.x),
							  Mathf.CeilToInt(box.size.y * transform.localScale.y),
							  Mathf.CeilToInt(box.size.z * transform.localScale.z));

		Player.Instance.Buildings.Add(this);
		Debug.Log($"Add building. Count ({this}): {Player.Instance.Buildings.Count}");
	}


	public virtual void Construct()
	{
		gameObject.AddComponent<NavMeshObstacle>();
		var meshObstacle = gameObject.GetComponent<NavMeshObstacle>();
		meshObstacle.center = gameObject.GetComponent<BoxCollider>().center;
		meshObstacle.size = gameObject.GetComponent<BoxCollider>().size;
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
	public void OnMouseDown()
	{
		if (Placed)
		{ 
			Player.Instance.ChangeInteractableObject(this);
			ShowPanel();
		}
	}

	public virtual void ShowPanel()
	{
		UIManager.Instance.EnableDisablePanel(Panel);
	}

	public virtual void Interact()
	{ Debug.Log("Not implemented"); }

	public virtual void Spawn(GameObject obj)
	{ Debug.Log("Not implemented"); }
}
