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
            if (dir.x > 0 && unit.transform.localScale.x < 0)
            {
                unit.transform.localScale = Vector3.Scale(unit.transform.localScale, new Vector3(-1, 1, 1));
                unit.direction = 1;
            }
            else if (dir.x < 0 && unit.transform.localScale.x > 0)
            {
                unit.transform.localScale = Vector3.Scale(unit.transform.localScale, new Vector3(-1, 1, 1));
                unit.direction = -1;
            }
        }
    }
}