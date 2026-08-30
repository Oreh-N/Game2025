using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : Team
{
	public static Player Instance;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	private new void Start()
	{
		base.Start();
		
		data.Ready = true;
	}

	new void Update()
	{
		base.Update();
	}

	public string GetTeamName()
	{
		return data.TeamName;
	}
}
