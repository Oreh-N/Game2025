using System.Collections.Generic;
using UnityEngine;

public class UIManagerData
{
	public Team FollowedTeam;
	public List<GameObject> AllPanels = new List<GameObject>();
	public GameObject MoneyPanel;
	public GameObject WarningPanel;
	public GameObject WoodPanel;

	public bool Is_default_cursor = true;
	public Texture2D DeclineCursor;
	public Texture2D DefaultCursor;
}
