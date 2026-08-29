using System.Collections.Generic;
using UnityEngine;


public class Prefabs
{
	#region Building prefabs
	public static GameObject DefBuildPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Buildings/DefBuild1");
	public static GameObject MainBuildPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Buildings/MainBuild");
	public static GameObject SpawnPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Buildings/Spawner1");
	public static GameObject WareHousePref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Buildings/Warehouse0");
	#endregion


	#region Unit prefabs
	public static GameObject WorkerPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Worker");
	public static GameObject MainerPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Mainer");
	public static GameObject MeleePref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Melee");
	public static GameObject MagePref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Mage");
	public static GameObject SpyPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Spy");
	public static GameObject HealerPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Healer");
	public static GameObject GuardPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Guard");
	#endregion

	public static GameObject Tree1 { get; private set; } = Resources.Load<GameObject>($"Prefabs/Environment/Tree1");

	public static Texture2D DefaultCursor = Resources.Load<Texture2D>("My2DAssets/Cursors/DefaultCursor0.png");
	public static Texture2D InteractCursor = Resources.Load<Texture2D>("My2DAssets/Cursors/InteractCursor0.png");
	public static Texture2D DeclineCursor = Resources.Load<Texture2D>("My2DAssets/Cursors/DeclineCursor.png");

	public static GameObject NameToPrefab(string name)
	{
		if ("DefBuild1" == name) return DefBuildPref;
		if ("MainBuild" == name) return MainBuildPref;
		if ("Spawner1" == name) return SpawnPref;
		if ("Warehouse0" == name) return WareHousePref;

		return null;
	}


	public static void AddChildrenFromFolder(List<GameObject> addToList, string folder)
	{
		var buttsFolder = GameObject.Find(folder);
		var count = buttsFolder.transform.childCount;
		for (int i = 0; i < count; i++)
		{
			var button = buttsFolder.transform.GetChild(i);
			addToList.Add(button.gameObject);
		}
	}
}

