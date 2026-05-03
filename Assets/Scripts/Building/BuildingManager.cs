using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.UI.CanvasScaler;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine;
using System;


public class BuildingManager : MonoBehaviour
{
	public static BuildingManager Instance;
	protected BuildManagerData data = new BuildManagerData();


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}




	# region Interaction with player
	public static void ShowMessage(string m)
	{
		if (m == null) return;
		UIManager.Instance.UpdateWarningPanel(m);
	}


	public static void SetInteractableObj(Building b, int teamID)
	{
		GetTeam(teamID).ChangeInteractableObject(b);
	}

	public void UpdatePanelText(string t, int panelID)
	{
		UIManager.Instance.SetPanelText(t, panelID);
	}


	public static void ShowPanel(int panelID)
	{
		var p = UIManager.Instance.GetPanel(panelID);
		if (p != null)
		{ UIManager.Instance.EnableDisablePanel(p); }
		else { UIManager.Instance.UpdateWarningPanel("Try to access a null panel"); }
	}

	# endregion 



	public GameObject SpawnObjOnPos(GameObject obj_, Team t, Vector3 pos)
	{
		var obj = Instantiate(obj_, pos, Quaternion.identity);
		if (obj.GetComponent<ITeamMember>() != null)
		{ obj.GetComponent<ITeamMember>().SetTeam(t.GetID()); }
		return obj;
	}

	


	public static void ColorCurrBuilding(Building b, Color color)
	{
		if (!b || b.GetRendererChildren() == null) return;
		foreach (Renderer rend in b.GetRendererChildren())
		{ rend.material.color = color; }
	}

	

	# region Data transfering
	public static void AddBuilding(Building b, int teamID)
	{
		if (!MainController.Instance.Ready)
			return;

		Team t = MainController.Instance.GetTeam(teamID);
		if (t) t.RegisterBuilding(b);
		else Debug.Log("Couldn't register the building");
			
	}

	public static void RemoveBuilding(Building b, int teamID)
	{
		Debug.Log("The building has been removed from team list");
		GetTeam(teamID).RemoveBuilding(b);
	}

	public void MoveLoot(ILootContainer from, ILootContainer to, List<LootType> content)
	{
		ILootContainer.MoveSpecificLoot(from.GetInventory(), to.GetInventory(), content);
	}
	public static bool TeamIsInteracting(int teamID)
	{
		return GetTeam(teamID).Interacting();
	}

	public static string GetTeamName(int teamID)
	{
		return GetTeam(teamID).GetName();
	}

	public static Team GetTeam(int teamID)
	{
		if (MainController.Instance.Ready)
			return MainController.Instance.GetTeam(teamID);
		return null;
	}
	#endregion
}
