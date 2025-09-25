using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UnitSelectionBox : MonoBehaviour
{
	[SerializeField] RectTransform _boxVisual;

	Vector2 _startPosition;
	Vector2 _endPosition;

	Rect _selectionBox;


	private void Start()
	{
		_startPosition = Vector2.zero;
		_endPosition = Vector2.zero;
	}

	private void Update()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

		if (Input.GetMouseButtonDown(0))
		{
			_startPosition = Input.mousePosition;
			_selectionBox = new Rect();
		}

		// When Dragging
		if (Input.GetMouseButton(0))
		{ BoxSelection(); }

		if (Input.GetMouseButtonUp(0))
		{ EndSelecting(); }
	}

	// Selection_________________________________________________
	void SelectUnits()
	{
		foreach (var unit in UnitSelectionManager.Instance.AllUnits)
		{
			if (_selectionBox.Contains(Camera.main.WorldToScreenPoint(unit.transform.position)))
			{
				UnitSelectionManager.Instance.DragSelect(unit);
			}
		}
	}

	private void EndSelecting()
	{
		SelectUnits();

		_startPosition = Vector2.zero;
		_endPosition = Vector2.zero;
		DrawVisual();
	}

	private void BoxSelection()
	{
		if (_boxVisual.rect.width > 0 || _boxVisual.rect.height > 0)
		{
			UnitSelectionManager.Instance.DeselectAll();
			SelectUnits();
		}

		_endPosition = Input.mousePosition;
		DrawVisual();
		DrawSelection();
	}
	// __________________________________________________________


	// Visual____________________________________________________
	void DrawVisual()
	{
		Vector2 boxStart = _startPosition;
		Vector2 boxEnd = _endPosition;

		// Calculate the center of the selection box.
		Vector2 boxCenter = (boxStart + boxEnd) / 2;

		// Set the position of the visual selection box based on its center.
		_boxVisual.position = boxCenter;
		Vector2 boxSize = new Vector2(Mathf.Abs(boxStart.x - boxEnd.x), Mathf.Abs(boxStart.y - boxEnd.y));
		_boxVisual.sizeDelta = boxSize;
	}

	void DrawSelection()
	{
		if (Input.mousePosition.x < _startPosition.x)
		{
			_selectionBox.xMin = Input.mousePosition.x;
			_selectionBox.xMax = _startPosition.x;
		}
		else
		{
			_selectionBox.xMin = _startPosition.x;
			_selectionBox.xMax = Input.mousePosition.x;
		}


		if (Input.mousePosition.y < _startPosition.y)
		{
			_selectionBox.yMin = Input.mousePosition.y;
			_selectionBox.yMax = _startPosition.y;
		}
		else
		{
			_selectionBox.yMin = _startPosition.y;
			_selectionBox.yMax = Input.mousePosition.y;
		}
	}
	// __________________________________________________________
}