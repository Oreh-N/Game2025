using System.Collections;
using UnityEngine;

public interface IMouseListener
{
	public void MouseHitMapAction(int button);

	/// <summary>
	/// Start this coroutine to recieve notifications from mouse
	/// </summary>
	/// <returns></returns>
	public IEnumerator StartListening()
	{
		yield return new WaitUntil(() => MainController.Instance.Ready);
		MouseController.Instance.AddListener(this);
	}
}

