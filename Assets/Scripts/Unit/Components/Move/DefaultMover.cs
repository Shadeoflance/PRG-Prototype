using UnityEngine;

class DefaultMover : Mover
{
    public DefaultMover(Unit unit)
        : base(unit)
    {
    }

    public override void Move(Vector2 dir)
    {
        unit.rb.velocity = Utils.TrimX(unit.rb.velocity);
        unit.rb.velocity += Utils.TrimY(dir) * unit.stats.speed;
        if (dir.magnitude > 0)
        {
            unit.direction = dir.x > 0 ? 1 : -1;
        }
    }
}