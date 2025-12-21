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

		_teams = new Team[2] { Player.Instance, CreateEnemy() };
	}


	public Team[] GetAllTeams() { return _teams; }

	/// <summary>
	/// Returns team by its ID
	/// </summary>
	/// <param name="teamID"></param>
	/// <returns>Returns team if exists, else returns null</returns>
	public Team GetTeam(int teamID)
	{
		if (teamID < _teams.Length)
			return _teams[teamID];
		Debug.LogWarning("Team with ID " + teamID + " does not exist.");
		return null;
	}

	private Team CreateEnemy()
	{
		GameObject enemyObj = Instantiate(EmptyEnemyObj);
		enemyObj.AddComponent<EnemyController>();
		return enemyObj.GetComponent<Team>();
	}
}
