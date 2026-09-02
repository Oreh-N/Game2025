using System.Collections.Generic;
using UnityEngine;
using MapSpace;
using PN = UIManager.PanelNames;


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
		data.Ready = true;
	}

	public bool Ready() { return data.Ready; }


	#region Interaction with player
	public static void ShowMessage(string m)
	{
		if (m == null) return;
		UIManager.Instance.UpdatePanel(PN.WarningP, m);
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
