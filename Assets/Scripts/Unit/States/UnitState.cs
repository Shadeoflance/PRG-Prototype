using UnityEngine;

public class UnitState
{
    protected Unit unit;
    public UnitState(Unit unit)
    {
        this.unit = unit;
    }

    public virtual void Jump()
    {
        unit.jumper.Jump();
    }

    public virtual void Attack()
    {
    }

    public virtual void Move(Vector2 dir)
    {
        unit.mover.Move(dir);
    }

    /// <summary>
    /// Do initialization and set unit to this state.
    /// </summary>
    /// <returns>this instance of UnitState</returns>
    public virtual UnitState Enter()
    {
        return this;
    }

    public virtual void Update() { }

    /// <summary>
    /// Change state to a new one.
    /// </summary>
    /// <param name="state">New state.</param>
    public virtual void Transit(UnitState state)
    {
        unit.state = state.Enter();
    }
}