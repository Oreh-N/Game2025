using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class UIManagerData
{
	public List<GameObject> _allPanels = new List<GameObject>();
	public GameObject MoneyPanel;
	public GameObject WarningPanel;
	public GameObject WoodPanel;

	public bool Is_default_cursor = true;
	public Texture2D DeclineCursor;
	public Texture2D DefaultCursor;
}
