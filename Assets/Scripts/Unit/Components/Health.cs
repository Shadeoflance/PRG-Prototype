using UnityEngine;

public class Health
{
    Unit unit;
    int currentHealth, maxHealth;
    public Health(Unit unit, int maxHealth)
    {
        this.unit = unit;
        this.maxHealth = maxHealth;
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        ActionParams ap = new ActionParams();
        ap.intParam = amount;
        ap.unit = unit;
        unit.eventManager.InvokeInterceptors("takeDmg", ap);
        if (ap.forbid)
            return;
        currentHealth -= ap.intParam;
        unit.eventManager.InvokeHandlers("takeDmg");
        if (currentHealth <= 0)
            Die();
    }

    public void Die()
    {
        ActionParams ap = new ActionParams();
        ap.unit = unit;
        unit.eventManager.InvokeInterceptors("die", ap);
        if (ap.forbid)
            return;
        unit.eventManager.InvokeHandlers("die");
        MonoBehaviour.Destroy(unit.gameObject);
    }
}