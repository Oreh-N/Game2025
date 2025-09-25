using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHavePanel
{
	public GameObject Panel { get; set; }
	public void UpdatePanelInfo();
	public void ShowPanel();
}
