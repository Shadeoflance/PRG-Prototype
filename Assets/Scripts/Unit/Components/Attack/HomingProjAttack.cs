using UnityEngine;

class ProjAttack : Attack
{
    BulletFactory factory;
    public float life = 3, speed = 4, cd = 2;
    public ProjAttack(Unit u, bool homing = false) : base(u)
    {
        Bullet b = ((GameObject)Resources.Load("Bullet")).GetComponent<Bullet>();
        factory = new BulletFactory(unit)
            .SetBullet(b)
            .SetLife(life)
            .SetSpeed(speed)
            .SetDmgMask(LayerMask.GetMask("Player"))
            .SetColor(Color.red);
        if (homing)
            factory.AddModifier(new HomingModif()).SetColor(Color.magenta);
    }

    public override void DoAttack()
    {
        factory.SetPosition(unit.transform.position).SetDir((Player.instance.transform.position - unit.transform.position).normalized).Build();
        curCd = cd;
    }

    float curCd;
    public override void Update()
    {
        curCd -= Time.deltaTime;
        if (unit.controller.NeedAttack() && curCd <= 0)
        {
            unit.currentState.Attack();
        }
    }

    class HomingModif : BulletProcessingModifier
    {
        public void Modify(Bullet bullet)
        {
            bullet.needVel = (Player.instance.transform.position - bullet.transform.position).normalized;
        }

        public BulletProcessingModifier Instantiate()
        {
            return new HomingModif();
        }
    }


}