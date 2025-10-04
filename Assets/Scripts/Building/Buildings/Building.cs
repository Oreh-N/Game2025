using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;



[RequireComponent(typeof(BoxCollider))]
public abstract class Building : MonoBehaviour, IInteractable, IConstructable, IDestructible, IHavePanel, ITeamMember
{
	GameObject IHavePanel.Panel { get => _panel; set => _panel = value; }
	float IDestructible.Health { get => _health; set => _health = value; }
	// Building_info________
	public Vector2Int Size { get; private set; }
	public bool Placed { get; protected set; }
	public abstract string Name { get; }
	protected float _health;
	// _____________________
	public bool NowInteracting { get; set; }
	public Team Team_ { get; set; }

	protected GameObject _panel;


	private void Awake()
	{
		BoxCollider box = GetComponent<BoxCollider>();
		box.enabled = false;
		Size = new Vector2Int(Mathf.CeilToInt(box.size.x * transform.localScale.x),
							  Mathf.CeilToInt(box.size.z * transform.localScale.z));
		Player.Instance.RegisterBuilding(this);
	}

	public void Update()
	{
		UpdatePanelInfo();
		if (!_panel.activeSelf)
		{ NowInteracting = false; }
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

	public virtual void TakeDamage(float damage)
	{ }

	private void OnDestroy()
	{ 
		Player.Instance.RemoveBuilding(this);
	}

	private void OnDrawGizmosSelected()
	{
		Size = new Vector2Int(20, 20);
		for (int x = 0; x < Size.x; x++)
		{
			for (int y = 0; y < Size.y; y++)
			{
				Gizmos.color = new Color(1f, 0.9f, 0.01f, 0.7f);
				Vector3 center_ = transform.position;
				center_ = new Vector3(center_.x - (Size.x / 2), center_.y, center_.z - (Size.y / 2));
				center_ += new Vector3(x, y: 0, z: y);
				Gizmos.DrawCube(center: center_, size: new Vector3(x: 1, y: 1f, z: 1));
			}
		}
	}

	// _______________________________________________________________


	// Building actions_______________________________________________
	public void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (Placed)
		{ 
			Player.Instance.ChangeInteractableObject(this);
			((IHavePanel)this).ShowPanel();
		}
	}

	public virtual void Interact()
	{ UIManager.Instance.UpdateWarningPanel("Building class Interact shouldn't be called"); }

	public virtual void UpdatePanelInfo()
	{ UIManager.Instance.UpdateWarningPanel("Building class UpdatePanelInfo shouldn't be called"); }
	// _______________________________________________________________
}
