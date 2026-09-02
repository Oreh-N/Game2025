using MapSpace;
using UnityEngine;
using System.Collections.Generic;

public class MouseController : MonoBehaviour
{
	public static MouseController Instance { get; private set; }
	List<IMouseListener> _listeners = new List<IMouseListener>();


	private void Awake()
	{
		if (Instance != null && Instance != this)
		{ Destroy(gameObject); }
		else
		{ Instance = this; }
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			LeftClick();

		}
		else if (Input.GetMouseButtonDown(1))
		{ RightClick(); }
	}

	public void AddListener(IMouseListener listener) { _listeners.Add(listener); }

	public void RemoveListener(IMouseListener listener) { _listeners.Remove(listener); }

	public void InformListeners(int mouseButt)
	{
		foreach (var l in _listeners)
		{ l.MouseHitMapAction(mouseButt); }
	}

	void RightClick() { InformListeners(1); }

	void LeftClick()
	{
		InformListeners(0);

		GameObject obj = GetObjOnMousePos();

		if (obj)
		{
			var interactObj = obj.GetComponent<IInteractable>();

			if (interactObj != null)
			{ interactObj.MouseDownAct(); }
		}
	}

	/// <summary>
	/// Get position of the mouse cursor on the world landscape
	/// </summary>
	/// <returns>Returns zero vector if didn't hit the ground</returns>
	public static Vector3 GetMouseWorldPos()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (MainController.groundPlane.Raycast(ray, out float distance))
		{ return ray.GetPoint(distance); }
		return Vector3.zero;
	}

	public GameObject GetObjOnMousePos()
	{
		Vector3 pos = GetMouseWorldPos();
		if (Vector3.zero == pos || Map.IsOutOfMap(pos)) { return null; }
		var mapPos = Map.WorldToMap(pos);

		return Map.GetGameObjectOnMap(mapPos);
	}
}

