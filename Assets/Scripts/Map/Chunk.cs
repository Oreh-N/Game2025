using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	public static readonly Vector2Int _size = new Vector2Int(32, 32);
	public List<GameObject> UnitsOnChunk { get; private set; } = new List<GameObject>();
	public List<GameObject> TreePrefabs { get; private set; } = new List<GameObject>();
	public List<int> TreeIndices = new List<int>();
	public bool IsActive = false;


	/// <summary>
	/// Erase only trees that was cut off by a unit
	/// </summary>
	public void EraseCutOffTrees()
	{
		if (UnitsOnChunk.Count == 0) return;

		for (int i = 0; i < TreePrefabs.Count; i++)
		{
			if (TreePrefabs[i].GetComponent<TreeScript>().IsEmpty)
			{
				TreePrefabs.RemoveAt(i);
				TreeIndices.RemoveAt(i);	// removes index and list becomes shorter, maybe an issue
				//ForestManager.Instance.ChangeTreeColor(i);

				//Debug.Log(TreePrefabs.Count);
				//Debug.Log(TreeIndices.Count);
			}
		}
	}

	/// <summary>
	/// Spawn trees instances if there is a unit on the chunk territory, 
	/// if no units are on the chunk trees will be earased from it
	/// </summary>
	public void ActivateIfHaveUnits()
	{
		if (UnitsOnChunk.Count == 0)
		{ ErasePrefabs(); }
		else
		{ SpawnTreesInstances(); }
	}

	private void ErasePrefabs()
	{
		foreach (var prefab in TreePrefabs)
		{ ForestManager.Instance.DestroyPrefab(prefab); }

		TreePrefabs.Clear();
		IsActive = false;
	}

	private void SpawnTreesInstances()
	{
		if (IsActive) return;

		foreach (var treeIndex in TreeIndices)
		{ ForestManager.Instance.SpawnTreePrefabInChunk(this, treeIndex); }

		IsActive = true;
	}
}
