using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Team : MonoBehaviour, ITeamMember
{
    public List<Building> Buildings { get; protected set; } = new List<Building>();
    public List<IAlive> Members { get; protected set; } = new List<IAlive>();
	Color ITeamMember.TeamColor { get => TeamColor; set => TeamColor = value; }
	string ITeamMember.TeamName { get => TeamName; set => TeamName = value; }
	public MainBuilding MainBuilding { get; protected set; }
	public string TeamName { get; protected set; }
    public Color TeamColor { get; protected set; }


	public void SetTeam(Color teamColor, string teamName)
	{
		TeamColor = teamColor;
		TeamName = teamName;
	}
}
