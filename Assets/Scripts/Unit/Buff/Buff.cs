using UnityEngine;

public class Buff : IUpdatable
{
    Unit unit;
    float duration;

    public Buff(Unit unit, float duration)
    {
        this.unit = unit;
        this.duration = duration;
    }

    public virtual void Update()
    {
        duration -= Time.deltaTime;
        if (duration < 0)
            End();
    }

    public virtual void End()
    {
        unit.buffs.Remove(this);
    }
}