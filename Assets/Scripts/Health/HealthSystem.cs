using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class HealthSystem : IDestructible
{
	HealthSysData healthSysData = new HealthSysData();


	public void TakeDamage(float damage)
	{
		throw new NotImplementedException();
	}

	public void Heal(float healing)
	{ throw new NotImplementedException(); }

	public float GetHealth()
	{
		return healthSysData.Health;
	}
	public void SetHealth(float health)
	{
		healthSysData.Health = health;
	}

}

