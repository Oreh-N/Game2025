using System;
using UnityEngine;


public class BuildingPrefabs
{
	public static GameObject DefBuildPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Buildings/DefBuild1");
	public static GameObject MainBuildPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Buildings/MainBuild");
	public static GameObject SpawnPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Buildings/Spawner1");
	public static GameObject WareHousePref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Buildings/Warehouse0");
}

