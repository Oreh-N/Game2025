using UnityEngine;


public class ScaleIndependent : MonoBehaviour
{
	private static readonly Vector3 DesiredWorldScale = Vector3.one;

	// AI
	private void LateUpdate()
	{
		Vector3 parentScale = transform.parent.lossyScale;
		transform.localScale = new Vector3(
			DesiredWorldScale.x / parentScale.x,
			DesiredWorldScale.y / parentScale.y,
			DesiredWorldScale.z / parentScale.z
		);
	}
}

