using UnityEngine;

class Flyer : Mover
{
    float speed;
    public Flyer(Unit unit, float speed)
        : base(unit)
    {
        this.speed = speed;
    }

    public override void Move(Vector2 dir)
    {
        unit.rb.velocity = dir * speed;
        if (dir.magnitude > 0)
        {
            unit.direction = dir.x > 0 ? 1 : -1;
        }
    }
}