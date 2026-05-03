using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Team
{
	private new void Start()
	{
		base.Start();
	}

	private new void Update()
	{
		base.Update();
	}


	/// <summary>
	/// Setup enemy base. If had troubles (out of map, etc.) will return false, otherwise true.
	/// </summary>
	/// <param name="position"></param>
	/// <param name="color"></param>
	/// <param name="name"></param>
	/// <returns></returns>
	public bool Setup(Vector2Int position, Color color, string name, int id)
	{
		if (Map.Instance.IsOutOfMap(position))
		{
			Destroy(this);
			Debug.Log("The base is out of map. Setup will be ignored.");
			return false;
		}
		data.BaseCenter = new Vector3(position.x, 0, position.y);
		SetTeam(color, name, id);
		return true;
	}
}
