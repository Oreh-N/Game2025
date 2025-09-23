using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
	public static UIManager Instance;


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	public static void EnableDisablePanel(GameObject panel)
	{
		if (panel.activeSelf)
			panel.SetActive(false);
		else panel.SetActive(true);
	}
}
