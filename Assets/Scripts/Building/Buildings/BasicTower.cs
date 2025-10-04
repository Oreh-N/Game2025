using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Building
{
	public override string Name => "Tower0";
    float _attackRadius = 10f;
	Unit _currTarget;
	float _attackCooldown = 2f;
	float _cooldown;
	bool _allowAttack;
	Projectile _currProjectile;
	int _damage;



	private new void Awake()
	{
		base.Awake();
		GetComponent<SphereCollider>().radius = _attackRadius;
		_damage = 30;
	}

	new void Start()
    {
        base.Start();
    }

	// Fill panel part
	private new void Update()
	{
		_cooldown += Time.deltaTime;
		if (!_allowAttack && _cooldown >= _attackCooldown)
		{ _allowAttack = true; }

		if (_currProjectile._startPos != _currProjectile._endPos)
		{ _currProjectile.Move(); }
		else
		{
			_currTarget.TakeDamage(_damage);
			Destroy(_currProjectile._projectile.gameObject);
		}
	}


	private void OnTriggerStay(Collider other)
	{
		if (other.tag == PubNames.UnitTag && _allowAttack)
		{
			_currTarget = other.gameObject.GetComponent<Unit>();
			Vector3 startPos = transform.position;
			startPos.y = 7;
			Vector3 endPos = other.transform.position;
			_currProjectile = new Projectile(startPos, endPos, _damage);
			_allowAttack = false;
		}
	}
}
