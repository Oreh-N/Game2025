using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHavePanel : IInteractable
{
	public UIManager.PanelNames GetPanelName();
	public string GetName();
	public string GetTeamName();
}
