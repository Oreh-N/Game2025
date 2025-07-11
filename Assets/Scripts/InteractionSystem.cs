using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[RequireComponent(typeof(Canvas))]
public class InteractionSystem : MonoBehaviour
{
    public LayerMask npcLayer;
    Camera mainCam;
    public Canvas UICanvas;

    void Start()
    {
        npcLayer = LayerMask.NameToLayer("NPC");
        mainCam = Camera.main;
        UICanvas = this.GetComponent<Canvas>();
    }

    
    void Update()
	{
    }
}
