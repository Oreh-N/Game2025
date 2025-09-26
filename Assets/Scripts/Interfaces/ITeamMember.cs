using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITeamMember
{
	public Color TeamColor { get; protected set; }
	public string TeamName { get; protected set; }

	public void SetTeam(Color teamColor, string teamName);
}
