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
        unit.rb.velocity = Vector2.zero;
        unit.rb.velocity += dir * speed;
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