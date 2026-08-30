using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHavePanel
{
	public virtual void UpdatePanelInfo(UIManager.PanelNames panelName)
	{
		UIManager.Instance.UpdatePanel(UIManager.PanelNames.WarningP, "Calling from interface");
	}

	public void ShowPanel(UIManager.PanelNames panelName)
	{
		UIManager.Instance.EnableDisablePanel(panelName);
		UpdatePanelInfo(panelName);
	}
}
