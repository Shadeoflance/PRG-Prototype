class PlayerState : UnitState
{
    public PlayerState(Unit unit) : base(unit) { }
    public virtual void Dash() { }
    public virtual void Slam() { }
    public virtual void UseItem() { }
}