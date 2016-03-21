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
        var b = factory.SetPosition(VectorUtils.ToV2(unit.transform.position + new Vector3(unit.direction * 0.5f, 0, 0)))
            .SetDmg(unit.damage)
            .SetDir(new Vector2(unit.direction, 0))
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