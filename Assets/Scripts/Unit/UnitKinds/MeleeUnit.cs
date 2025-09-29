using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MeleeUnit : Unit
{
	public override string UnitName => "MeleeUnit";
	float _attackCooldown = 1f;
	float _cooldown = 0;
	bool _allowAttack = true;
	int _damage = 20;

	private new void Awake()
	{
		base.Awake();
	}

	new void Start()
    {
        base.Start();
    }

    new void Update()
    {
		base.Update();
		_cooldown += Time.deltaTime;
		if (!_allowAttack && _cooldown >= _attackCooldown)
		{ _allowAttack = true; }
    }

	private void OnTriggerStay(Collider other)
	{
		if (other != null && other.tag == PubNames.UnitTag && _allowAttack)
		{
			other.GetComponent<Unit>().TakeDamage(_damage);
			_cooldown = 0;
			_allowAttack = false;
		}
	}

	public override void Interact()
	{ 
		((IHavePanel)this).ShowPanel();
	}

	public override void UpdatePanelInfo()
	{
		Text[] panels = Panel.GetComponentsInChildren<Text>(true);
		panels[0].text = $"Unit name: {UnitName}\nTeam: {Team_.TeamName}\nHealth: {_health}";
	}
}
