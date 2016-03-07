using UnityEngine;

public class PlayerWalkingState : PlayerState
{
    public PlayerWalkingState(Unit unit) : base(unit) { }
    public override void Dash()
    {
        player.dasher.Dash();
    }
}

public class WalkingState : UnitState
{
    public WalkingState(Unit unit) : base(unit) { }
}