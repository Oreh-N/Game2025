using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectionBox : MonoBehaviour
{
	Camera myCam;

	[SerializeField]
	RectTransform boxVisual;

	Rect selectionBox;

	Vector3 startPosition;
	Vector3 endPosition;

	private void Start()
	{
		myCam = Camera.main;
		startPosition = Vector3.zero;
		endPosition = Vector3.zero;
		DrawVisual();
	}

	private void Update()
	{
		// When Clicked
		if (Input.GetMouseButtonDown(0))
		{
			startPosition = Input.mousePosition;

			// For selection the Units
			selectionBox = new Rect();
		}

		// When Dragging
		if (Input.GetMouseButton(0))
		{
			if (boxVisual.rect.width > 0 || boxVisual.rect.height > 0)
			{
				UnitSelectionManager.Instance.DeselectAll();
				SelectUnits();
			}

			endPosition = Input.mousePosition;
			DrawVisual();
			DrawSelection();
		}

		// When Releasing
		if (Input.GetMouseButtonUp(0))
		{
			SelectUnits();

			startPosition = Vector3.zero;
			endPosition = Vector3.zero;
			DrawVisual();
		}
	}

	void DrawVisual()
	{
		// Fixed on X axis
		// Calculate the starting and ending positions of the selection box.
		Vector3 boxStart = startPosition;
		Vector3 boxEnd = endPosition;

		// Calculate the center of the selection box.
		Vector3 boxCenter = (boxStart + boxEnd) / 2;

		// Set the position of the visual selection box based on its center.
		boxVisual.position = boxCenter;

		// Calculate the size of the selection box in both width and height.
		Vector3 boxSize = new Vector3(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y), Mathf.Abs(boxStart.z - boxEnd.z));

		// Set the size of the visual selection box based on its calculated size.
		boxVisual.sizeDelta = boxSize;
	}

	void DrawSelection()
	{
		if (Input.mousePosition.x < startPosition.x)
		{
			selectionBox.xMin = Input.mousePosition.x;
			selectionBox.xMax = startPosition.x;
		}
		else
		{
			selectionBox.xMin = startPosition.x;
			selectionBox.xMax = Input.mousePosition.x;
		}


		if (Input.mousePosition.y < startPosition.y)
		{
			selectionBox.yMin = Input.mousePosition.y;
			selectionBox.yMax = startPosition.y;
		}
		else
		{
			selectionBox.yMin = startPosition.y;
			selectionBox.yMax = Input.mousePosition.y;
		}
	}

	void SelectUnits()
	{
		foreach (var unit in UnitSelectionManager.Instance.allUnits)
		{
			if (selectionBox.Contains(myCam.WorldToScreenPoint(unit.transform.position)))
			{
				UnitSelectionManager.Instance.DragSelect(unit);
			}
		}
	}
}