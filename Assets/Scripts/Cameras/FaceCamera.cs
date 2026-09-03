using UnityEngine;

public class FaceCamera : MonoBehaviour
{
	static readonly Quaternion CanvasFacing = Quaternion.Euler(45f, 45f, 0f);

	private void LateUpdate()
	{ transform.rotation = CanvasFacing; }
}

