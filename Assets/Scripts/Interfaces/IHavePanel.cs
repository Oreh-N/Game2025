using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHavePanel
{
	public virtual void UpdatePanelInfo(GameObject panel)
	{
		if (panel == null)
		{ UIManager.Instance.UpdateWarningPanel("Panel here is null"); }
	}

	public void ShowPanel(GameObject panel)
	{
		if (panel == null)
		{ UIManager.Instance.UpdateWarningPanel("Panel here is null"); }
		UIManager.Instance.EnableDisablePanel(panel);
		UpdatePanelInfo(panel);
	}
}
