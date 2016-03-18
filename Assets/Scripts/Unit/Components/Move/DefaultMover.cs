using UnityEngine;

class DefaultMover : Mover
{
    float speed;
    public DefaultMover(Unit unit, float speed)
        : base(unit)
    {
        this.speed = speed;
    }

    public override void Move(Vector2 dir)
    {
        unit.rb.velocity = VectorUtils.TrimX(unit.rb.velocity);
        unit.rb.velocity += VectorUtils.TrimY(dir) * speed;
        if (dir.magnitude > 0)
        {
            unit.direction = dir.x > 0 ? 1 : -1;
        }
    }
}