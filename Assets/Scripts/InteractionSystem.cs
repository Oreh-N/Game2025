using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InteractionSystem : MonoBehaviour
{
    public LayerMask npcLayer;
    Camera mainCam;
    Canvas UICanvas;

    void Start()
    {
        npcLayer = LayerMask.NameToLayer("NPC");
        mainCam = Camera.main;
        UICanvas = GetComponent<Canvas>();
    }

    
    void Update()
	{
        Vector3 mousePos = Input.mousePosition;
		RaycastHit hit;
		if (Input.GetMouseButtonDown(1))
        {
			Ray ray = mainCam.ScreenPointToRay(mousePos);

			// If we are hitting a clickble object
			if (Physics.Raycast(ray, out hit, Mathf.Infinity, npcLayer))
            {

            }

		}
    }
}
