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
        if (needVel.x > 0 && unit.transform.localScale.x < 0)
        {
            unit.transform.localScale = Vector3.Scale(unit.transform.localScale, new Vector3(-1, 1, 1));
        }
        else if (needVel.x < 0 && unit.transform.localScale.x > 0)
        {
            unit.transform.localScale = Vector3.Scale(unit.transform.localScale, new Vector3(-1, 1, 1));
        }
    }

    public override void Transit(UnitState state)
    {
        if (state is PlayerAirborneState)
            return;
        base.Transit(state);
    }
}