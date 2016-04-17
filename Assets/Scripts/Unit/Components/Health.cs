using UnityEngine;

public class Health
{
    Unit unit;
    public float currentHealth, maxHealth;
    public Health(Unit unit, float maxHealth)
    {
        this.unit = unit;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount, GameObject source)
    {
        ActionParams ap = new ActionParams();
        ap["amount"] = amount;
        unit.eventManager.InvokeInterceptors("takeDmg", ap);
        if (ap.forbid)
            return; 
        currentHealth -= (float)ap["amount"];
        unit.eventManager.InvokeHandlers("takeDmg", ap);
        if (currentHealth <= 0)
        {
            Die();
            return;
        }
        int direction = source.transform.position.x < unit.transform.position.x ? 1 : -1;
        if (unit is Player)
        {
            unit.currentState.Transit(new PlayerDamageTakenState(unit, direction));
        }
        else
        {
            unit.currentState.Transit(new DamageTakenState(unit, direction));
        }
    }

    public void Die()
    {
        ActionParams ap = new ActionParams();
        ap.unit = unit;
        unit.eventManager.InvokeInterceptors("die", ap);
        if (ap.forbid)
            return;
        unit.eventManager.InvokeHandlers("die", null);
        //GameObject.Destroy(unit.gameObject);
        unit.gameObject.SetActive(false);
    }
}