using UnityEngine;

class PlayerAirborneState : PlayerState
{
    public PlayerAirborneState(Unit unit) : base(unit) 
    {
    }

    public override void Update()
    {
        unit.rb.velocity = VectorUtils.TrimX(unit.rb.velocity);
        Vector2 needVel = unit.controller.NeedVel();
        unit.rb.velocity += VectorUtils.TrimY(needVel) * unit.speed;
    }

    public override void Transit(UnitState state)
    {
        if (state is PlayerAirborneState)
            return;
        base.Transit(state);
    }
}