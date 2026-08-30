using System.Collections;
using UnityEngine;

public interface IListener
{
	public void MouseHitMapAction(int button);

	public IEnumerator StartListening()
	{
		yield return new WaitUntil(() => MainController.Instance.Ready);
		MouseController.Instance.AddListener(this);
	}
}

