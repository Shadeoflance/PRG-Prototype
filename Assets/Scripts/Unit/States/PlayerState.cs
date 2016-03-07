public class PlayerState : UnitState
{
    protected Player player;
    public PlayerState(Unit unit) : base(unit) 
    {
        player = (Player)unit;
    }
    public virtual void Dash() { }
    public virtual void Slam() { }
    public virtual void UseItem() { }
}