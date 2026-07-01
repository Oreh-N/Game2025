using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class TreeCreator
{

	static GameObject _treeFolder = GameObject.Find("TreeFolder");
	static GameObject _treePrefab = Prefabs.Tree1;


	public static GameObject CreateTree(Vector3 pos)
	{
		return GameObject.Instantiate(_treePrefab, pos, 
			Quaternion.identity, _treeFolder.transform);
	}
}

