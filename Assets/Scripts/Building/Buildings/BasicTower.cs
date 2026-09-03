using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTower : Building
{
	//float _attackRadius = 10f;
	//Unit _currTarget;
	//float _attackCooldown = 2f;
	//float _cooldown;
	//bool _allowAttack;
	//Projectile _currProjectile;
	//int _damage;



	private new void Awake()
	{
		base.Awake();
		data.Name = "Tower0";
		data.CellType = MapSpace.Map.CellType.BasicTower;
		data.Size = new Vector2Int(5, 5);
		//_damage = 30;

	}

	new void Start()
    {
        base.Start();
    }

	// Fill panel part
	private new void Update()
	{
		base.Update();
		//_cooldown += Time.deltaTime;
		//if (!_allowAttack && _cooldown >= _attackCooldown)
		//{ _allowAttack = true; }

		//if (_currProjectile._startPos != _currProjectile._endPos)
		//{ _currProjectile.Move(); }
		//else
		//{
		//	_currTarget.TakeDamage(_damage);
		//	Destroy(_currProjectile._projectile.gameObject);
		//}
	}

}
