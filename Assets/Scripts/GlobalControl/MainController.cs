using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
	public static MainController Instance;

	[SerializeField] public GameObject MainBuildingPrefab;
	[SerializeField] public GameObject EmptyEnemyObj;
	Team[] _teams;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }

	}

	private void Start()
	{
		_teams = new Team[3] { 
			Player.Instance,
			CreateEnemy(new Vector2Int(700,800), Color.orchid, ":3"),
			CreateEnemy(new Vector2Int(100, 300), Color.aliceBlue, "Alice")
		};
	}

	public Team[] GetAllTeams() { return _teams; }

	/// <summary>
	/// Returns team by its ID
	/// </summary>
	/// <param name="teamID"></param>
	/// <returns>Returns team if exists, else returns null</returns>
	public Team GetTeam(uint teamID)
	{
		if (teamID < _teams.Length)
			return _teams[teamID];
		Debug.LogWarning("Team with ID " + teamID + " does not exist.");
		return null;
	}

	private Team CreateEnemy(Vector2Int pos, Color c, string name)
	{
		GameObject enemyObj = Instantiate(EmptyEnemyObj);
		var enemy = enemyObj.AddComponent<EnemyController>();
		enemy.Setup(pos, c, name);
		
		return enemyObj.GetComponent<Team>();
	}
}

// coroutine (nebo vedlejsi vlakno)
// nemenit verze unity
