using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class UnitManager
{
	public static string GetTeamName(int teamID)
	{
		Team t = GetTeam(teamID);
		if (t) return t.GetName();
		return "";
	}
	public static Team GetTeam(int teamID)
	{
		Team t = MainController.Instance.GetTeam(teamID);
		if (t) return t;
		return null;
	}

	public static string GetTeamName(Unit unit)
	{
		return unit.GetTeamName();
	}

	public static string GetPlayerTeamName()
	{
		return Player.Instance.GetTeamName();
	}

	public static void HitUnit(Unit injuredUnit, int damage)
	{
		injuredUnit.TakeDamage(damage);
	}
}
