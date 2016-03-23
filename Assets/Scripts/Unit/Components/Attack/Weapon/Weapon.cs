using UnityEngine;

public class Weapon : Attack, IUpdatable
{
    public BulletFactory factory;
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


    public Weapon(Unit unit, float damage = 1, float speed = 10, float life = 1, float bps = 4f)
        : base(unit, damage)
    {
        this.bps = bps;
        this._speed = speed;
        this._life = life;
        player = (Player)unit;
        factory = new BulletFactory(unit)
            .SetBullet(((GameObject)Resources.Load("bullet")).GetComponent<Bullet>())
            .SetLife(life)
            .SetSpeed(speed);
    }

    public override void DoAttack()
    {
        Vector2 dir, vel = player.controller.NeedVel();
        if (vel.y > 0)
            dir = Vector2.up;
        else if (player.currentState == player.airborne && vel.y < 0)
            dir = Vector2.down;
        else dir = new Vector2(player.direction, 0);
        var b = factory.SetPosition(unit.transform.position.ToV2() + dir * 0.5f)
            .SetDmg(unit.damage)
            .SetDir(dir)
            .Build();
        ActionParams ap = new ActionParams();
        ap["bullet"] = b;
        unit.eventManager.InvokeInterceptors("shoot", ap);
        if (ap.forbid)
            GameObject.Destroy(b.gameObject);
        unit.eventManager.InvokeHandlers("shoot", ap);
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