using UnityEngine;

public class Weapon : Attack, IUpdatable
{
    public BulletFactory factory;
    public float bps;
    float cd = 0;

    public Weapon(Unit unit, float damage = 1f, float speed = 10f, float life = 1f, float bps = 4f)
        : base(unit, damage)
    {
        this.bps = bps;
        factory = new BulletFactory(unit)
            .SetBullet(((GameObject)Resources.Load("Bullet")).GetComponent<Bullet>())
            .SetLife(life)
            .SetSpeed(speed)
            .SetDmgMask(LayerMask.GetMask("Enemy"));
    }

    public override void DoAttack()
    {
        Vector2 dir, vel = unit.controller.NeedVel();
        if (vel.y > 0)
            dir = Vector2.up;
        else if (unit.currentState == unit.airborne && vel.y < 0)
            dir = Vector2.down;
        else dir = new Vector2(unit.direction, 0);
        var b = factory.SetPosition(unit.transform.position.ToV2() + dir * 0.5f)
            .SetDir(dir)
            .Build();
        ActionParams ap = new ActionParams();
        ap["bullet"] = b;
        unit.eventManager.InvokeInterceptors("shoot", ap);
        if (ap.forbid)
        {
            GameObject.Destroy(b.gameObject);
            return;
        }
        unit.eventManager.InvokeHandlers("shoot", ap);
        cd = 1f / bps;
    }

    public override void Update()
    {
        cd -= Time.deltaTime;
        if (unit.controller.NeedAttack() && cd <= 0)
        {
            unit.currentState.Attack();
        }
    }
}