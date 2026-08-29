using static UnityEditor.Timeline.TimelinePlaybackControls;
using static UnityEngine.UI.CanvasScaler;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.Tilemaps;
using System.Collections;
using UnityEngine;
using MapSpace;
using System;
using UnityEngine.UI;


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

	private void Start()
	{
		FindBuildingButtons(data.Buttons);
		AssignButtons();
		data.Ready = true;
	}

	public bool Ready() { return data.Ready; }

	void AssignButtons()
	{
		foreach (var button in data.Buttons)
		{
			var prefab = Prefabs.NameToPrefab(button.name);
			button.GetComponent<Button>().onClick.AddListener(() => MapController.Instance.SpawnMovableBuild	
			(prefab, Player.Instance.GetID()));
		}
	}

	void FindBuildingButtons(List<GameObject> buttons)
	{
		var buttsFolder = GameObject.Find("BuildingButts");
		var count = buttsFolder.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			var button = buttsFolder.transform.GetChild(i);
			data.Buttons.Add(button.gameObject);
		}
	}


	#region Interaction with player
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

	#endregion



	public GameObject SpawnObjOnPos(GameObject obj_, Team t, Vector3 pos)
	{
		var obj = Creator.CreateBuilding(obj_, pos);
		if (obj.GetComponent<ITeamMember>() != null)
		{ obj.GetComponent<ITeamMember>().SetTeam(t.GetID()); }
		return obj;
	}




	public static void ColorCurrBuilding(Building b, Color color)
	{
		if (!b) return;
		b.ColorBuilding(color);
	}



	#region Data transfering
	public static void AddBuilding(Building b, int teamID)
	{
		Team t = MainController.Instance.GetTeam(teamID);
		if (t) t.RegisterBuilding(b);
		else Debug.Log("Couldn't register the building");

	}

	public static void RemoveBuilding(Building b, int teamID)
	{
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
