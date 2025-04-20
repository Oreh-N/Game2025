using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LootType { Tree, Money }

public class Loot : MonoBehaviour
{
   public LootType Type { get; protected set; }
}
