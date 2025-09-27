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
		_teams = new Team[2] { Player.Instance, CreateEnemy() };
	}

	void Start()
    {
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }


	}

    void Update()
    {
        
    }

	private Team CreateEnemy()
	{
		GameObject enemyObj = Instantiate(EmptyEnemyObj);
		enemyObj.AddComponent<EnemyController>();
		return enemyObj.GetComponent<Team>();
	}
}
