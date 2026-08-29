using System.Collections.Generic;
using UnityEngine;

public class UIManagerData
{

	public List<GameObject> Buttons = new List<GameObject>();
	public List<GameObject> AllPanels = new List<GameObject>();
	public List<UIManager.PanelNames> AlwaysActivePanels;
	public List<string> Prefixes = new List<string>();

	public bool Is_default_cursor = true;
	public Texture2D DeclineCursor;
	public Texture2D DefaultCursor;

	public bool Ready = false;
	public Team FollowedTeam;
}
