using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile
{
    const float edgeSize = 0.1f;
    public GameObject _projectile { get; protected set; }
	public Vector3 _startPos { get; protected set; }
    public Vector3 _endPos { get; protected set; }
	public float _speed { get; protected set; } = 50f;
    public int _damage { get; protected set; }


	public Projectile(Vector3 startPos, Vector3 endPos, int damage)
    {
        _projectile = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var col = _projectile.GetComponent<BoxCollider>();
        col.transform.localScale = new Vector3(edgeSize, edgeSize, edgeSize);
        col.isTrigger = true;
        _startPos = startPos;
        _endPos = endPos;
    }

	public void Move()
    {
		_projectile.transform.LookAt(_endPos);
		_projectile.transform.position = Vector3.MoveTowards(_projectile.transform.position, _endPos, _speed * Time.deltaTime);
		Debug.DrawLine(_projectile.transform.position, _endPos, Color.red);
	}
}
