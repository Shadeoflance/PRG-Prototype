using UnityEngine;

public class Weapon : Attack
{
    BulletFactory factory;

    public Weapon(Unit unit, float speed = 10, float life = 1)
        : base(unit)
    {
        factory = new BulletFactory()
            .SetBullet((Bullet)GameObject.Instantiate((GameObject)Resources.Load("bullet")).GetComponent<Bullet>())
            .SetLife(life)
            .SetSpeed(speed);
    }

    public override void DoAttack()
    {
        factory.SetPosition(VectorUtils.V3ToV2(unit.transform.position + new Vector3(unit.direction * 1, 0, 0)))
            .SetDir(new Vector2(unit.direction, 0))
            .Build();
    }
}