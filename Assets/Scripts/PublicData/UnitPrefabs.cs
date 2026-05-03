using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class UnitPrefabs
{
	public static GameObject WorkerPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Worker");
	public static GameObject MainerPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Mainer");
	public static GameObject MeleePref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Melee");
	public static GameObject MagePref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Mage");
	public static GameObject SpyPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Spy");
	public static GameObject HealerPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Healer");
	public static GameObject GuardPref { get; private set; } = Resources.Load<GameObject>($"Prefabs/Units/Guard");

}
