using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class MeleeUnit : UnitSelf<MeleeUnit>
{
	float _attackCooldown = 1f;
	float _cooldown = 0;
	bool _allowAttack = true;
	int _damage = 20;

	private new void Awake()
	{
		data.Name = "MeleeUnit";
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
		if (other != null && other.tag == PubNames.UnitTag &&
			_allowAttack && UnitManager.GetTeamName(other.GetComponent<Unit>()) != UnitManager.GetTeamName(data.TeamID))
		{
			UnitManager.HitUnit(other.GetComponent<Unit>(), _damage);
			_cooldown = 0;
			_allowAttack = false;
		}
	}

	public override void Interact()
	{ 
		((IHavePanel)this).ShowPanel(data.Panel);
	}

	public override void UpdatePanelInfo()
	{
		Text[] panels = data.Panel.GetComponentsInChildren<Text>(true);
		panels[0].text = $"Unit name: {data.Name}\nTeam: {UnitManager.GetTeamName(data.TeamID)}\nHealth: {data.Health}";
	}
}
