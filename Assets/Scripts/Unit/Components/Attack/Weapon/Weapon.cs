using UnityEngine;

public class Weapon : Attack, IUpdatable
{
    BulletFactory factory;
    float _speed, _life;
    public float bps;
    float cd = 0;
    Player player;

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


    public Weapon(Unit unit, float speed = 10, float life = 1, float bps = 2f)
        : base(unit)
    {
        this.bps = bps;
        this._speed = speed;
        this._life = life;
        player = (Player)unit;
        factory = new BulletFactory(unit)
            .SetBullet((Bullet)GameObject.Instantiate((GameObject)Resources.Load("bullet")).GetComponent<Bullet>())
            .SetLife(life)
            .SetSpeed(speed);
    }

    public override void DoAttack()
    {
        factory.SetPosition(VectorUtils.V3ToV2(unit.transform.position + new Vector3(unit.direction * 0.5f, 0, 0)))
            .SetDmg(unit.damage)
            .SetDir(new Vector2(unit.direction, 0))
            .Build();
        cd = 1f / bps;
    }

    public void Update()
    {
        cd -= Time.deltaTime;
        if (player.controller.NeedAttack() && cd <= 0)
        {
            player.currentState.Attack();
        }
    }
}