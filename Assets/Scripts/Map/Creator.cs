using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace MapSpace
{
	public static class Creator // Initialize trees in specific folder
	{

		static GameObject _treeFolder = GameObject.Find("TreeFolder");
		static GameObject _buildingFolder = GameObject.Find("BuildingFolder");
		static GameObject _unitFolder = GameObject.Find("UnitFolder");



		public static GameObject CreateTree(Vector3 pos)
		{
			return GameObject.Instantiate(Prefabs.Tree1, pos,
				Quaternion.identity, _treeFolder.transform);
		}


		public static GameObject CreateUnit(GameObject unitPrefab, Vector3 pos)
		{
			return GameObject.Instantiate(unitPrefab, pos,
			Quaternion.identity, _unitFolder.transform);
		}


		public static GameObject CreateBuilding(GameObject buildingPrefab, Vector3 pos)
		{
			return GameObject.Instantiate(buildingPrefab, pos,
			Quaternion.identity, _buildingFolder.transform);
		}
	}
}

