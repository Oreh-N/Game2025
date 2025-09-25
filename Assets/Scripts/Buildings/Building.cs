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
	float IDestructible.Health { get => _health; set => _health = value; }
	public GameObject Panel { get; protected set; }
	public Vector3Int Size { get; private set; }
	public bool Placed { get; private set; }
	public abstract string Name { get; }
	float _health;


	private void Awake()
	{
		BoxCollider box = GetComponent<BoxCollider>();
		box.enabled = false;
		Size = new Vector3Int(Mathf.CeilToInt(box.size.x * transform.localScale.x),
							  Mathf.CeilToInt(box.size.y * transform.localScale.y),
							  Mathf.CeilToInt(box.size.z * transform.localScale.z));
		Player.Instance.RegisterBuilding(this);
	}


	// Build/Destruct_________________________________________________
	public virtual void Construct()
	{
		gameObject.AddComponent<NavMeshObstacle>();
		var mesh = gameObject.GetComponent<NavMeshObstacle>();
		mesh.center = gameObject.GetComponent<BoxCollider>().center;
		mesh.size = gameObject.GetComponent<BoxCollider>().size;
		mesh.carveOnlyStationary = false;
		mesh.carving = true;

		gameObject.GetComponent<BoxCollider>().enabled = true;
		Destroy(gameObject.GetComponent<Movable>());
		Placed = true;
	}

	public virtual void Damage(float damage)
	{ }

	private void OnDestroy()
	{ 
		Player.Instance.RemoveBuilding(this);
	}
	// _______________________________________________________________


	// Building actions_______________________________________________
	public void OnMouseDown()
	{
		if (Placed)
		{ 
			Player.Instance.ChangeInteractableObject(this);
			ShowPanel();
		}
	}

	public virtual void ShowPanel()
	{ UIManager.Instance.EnableDisablePanel(Panel); }

	public virtual void Interact()
	{ UIManager.Instance.UpdateWarningPanel("Building class Interact shouldn't be called"); }

	public virtual void Spawn(GameObject obj)
	{ UIManager.Instance.UpdateWarningPanel("Building class Spawn shouldn't be called"); }
	// _______________________________________________________________
}
