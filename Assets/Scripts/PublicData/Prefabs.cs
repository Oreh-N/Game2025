using UnityEngine;
using System;


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

}

