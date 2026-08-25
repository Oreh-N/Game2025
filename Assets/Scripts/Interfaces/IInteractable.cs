using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
	public void Interact();

	public virtual void MouseDownAct() { Debug.Log("Shouldn't call MouseDownAct in interface"); }
}
