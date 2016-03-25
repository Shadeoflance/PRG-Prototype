public class StunnedState : UnitState
{
    public StunnedState(Unit unit) : base(unit) { }
    public override void Jump() { }
    public override void Move(UnityEngine.Vector2 dir) { }
    public override void Attack() { }
    public void End()
    {
        unit.currentState = unit.walking;
    }
    public override bool Transit(UnitState state)
    {
        return false;
    }
}
public class PlayerStunnedState : PlayerState
{
    public PlayerStunnedState(Unit unit) : base(unit) { }
    public override void Jump() { }
    public override void Move(UnityEngine.Vector2 dir) { }
    public override void Attack() { }
    public void End()
    {
        unit.currentState = unit.walking;
    }
    public override bool Transit(UnitState state)
    {
        return false;
    }
}