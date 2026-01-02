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
public abstract class Building : MonoBehaviour, IInteractable, IConstructable, IHavePanel, ITeamMember, IPlaceableOnMap
{
	protected BuildingData Data = new BuildingData();
	public HealthSystem HealthSys { get; protected set; } = new HealthSystem();



	public void Awake()
	{
		BoxCollider box = GetComponent<BoxCollider>();
		box.enabled = false;
		Data.Size = new Vector2Int(Mathf.CeilToInt(box.size.x * transform.localScale.x + 1),
							  Mathf.CeilToInt(box.size.y * transform.localScale.y)+1);
		
	}

	public void Start()
	{
		BuildingManager.AddBuilding(this, Data.TeamID);
	}

	public void Update()
	{
		if (HealthSys.GetHealth() <= 0)
		{ Destroy(gameObject); }
		//UpdatePanelInfo();
		//if (!BuildingManager.TeamIsInteracting(Data.TeamID))
		//{ Data.NowInteracting = false; }
	}

	public virtual void Construct()
	{
		gameObject.GetComponent<BoxCollider>().enabled = true;
		Destroy(gameObject.GetComponent<Movable>());
		ColorBuilding();
		Data.IsPlaced = true;
	}

	public virtual void ColorBuilding()
	{
		Color teamColor = BuildingManager.GetTeam(Data.TeamID).GetColor();
		Renderer[] renderers = GetComponentsInChildren<Renderer>();
		foreach (Renderer rend in renderers)
		{
			foreach (Material mat in rend.materials)
			{
				if (mat.HasProperty("_Color"))
				{ mat.color = teamColor; }
			}
		}
	}

	private void OnDestroy()
	{ 
		Debug.Log("Building destroing is not implemented yet");
		//BuildingManager.RemoveBuilding(this, Data.TeamID);
	}
	// _______________________________________________________________


	// Building actions_______________________________________________
	public void OnMouseDown()
	{
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (Data.IsPlaced)
		{
			BuildingManager.SetInteractableObj(this, Data.TeamID);
			BuildingManager.ShowPanel(Data.PanelID);
		}
	}

	public virtual void Interact()
	{ BuildingManager.ShowMessage("Building class Interact shouldn't be called"); }

	public virtual void UpdatePanelInfo()
	{ BuildingManager.ShowMessage("Building class UpdatePanelInfo shouldn't be called"); }

	// DATA_TRANSFERRING_______________________________________________________________

	public bool IsPlaced() { return Data.IsPlaced; }

	public int GetTeamID() { return Data.TeamID; }

	public string GetName() { return Data.Name; }

	public Vector2 GetSize() { return Data.Size; }

	public void SetTeam(int teamID) { Data.TeamID = teamID; }

	public Vector2Int GetTakeAreaSize() { return Data.Size; }

	public Vector3 GetPos() { return transform.position; }
	// _______________________________________________________________
}
