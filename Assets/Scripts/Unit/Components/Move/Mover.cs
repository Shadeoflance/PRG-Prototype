using UnityEngine;

public abstract class Mover
{
    protected Unit unit;
    public Mover(Unit unit)
    {
        this.unit = unit;
    }
    public abstract void Move(Vector2 dir);
}