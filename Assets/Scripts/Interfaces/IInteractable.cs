using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
	public bool NowInteracting { get; set; } 
	public void Interact();
}
