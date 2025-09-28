using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHavePanel
{
	public GameObject Panel { get; set; }
	public void UpdatePanelInfo()
	{
		if (Panel == null)
		{ UIManager.Instance.UpdateWarningPanel("Panel here is null"); }
	}

	public void ShowPanel()
	{
		if (Panel == null)
		{ UIManager.Instance.UpdateWarningPanel("Panel here is null"); }
		UIManager.Instance.EnableDisablePanel(Panel);
		UpdatePanelInfo();
	}
}
