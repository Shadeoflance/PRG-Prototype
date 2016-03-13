using UnityEngine;

public class Weapon : Attack
{
    BulletFactory factory;
    float _speed, _life;

    public float speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
            factory.SetSpeed(_speed);
        }
    }
    public float life
    {
        get
        {
            return _life;
        }
        set
        {
            _life = value;
            factory.SetLife(_life);
        }
    }


    public Weapon(Unit unit, float speed = 10, float life = 1)
        : base(unit)
    {
        this._speed = speed;
        this._life = life;
        factory = new BulletFactory(unit)
            .SetBullet((Bullet)GameObject.Instantiate((GameObject)Resources.Load("bullet")).GetComponent<Bullet>())
            .SetLife(life)
            .SetSpeed(speed);
    }

    public override void DoAttack()
    {
        factory.SetPosition(VectorUtils.V3ToV2(unit.transform.position + new Vector3(unit.direction * 1, 0, 0)))
            .SetDmg(unit.damage)
            .SetDir(new Vector2(unit.direction, 0))
            .Build();
    }
}