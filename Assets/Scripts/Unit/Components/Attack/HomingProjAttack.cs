using UnityEngine;

class HomingProjAttack : Attack
{
    BulletFactory factory;
    float life = 3, speed = 4;
    public HomingProjAttack(Unit u) : base(u)
    {
        Bullet b = ((GameObject)Resources.Load("HomingProj")).GetComponent<Bullet>();
        //b.transform.FindChild("Sprite").GetComponent<SpriteRenderer>().sprite = sprite;
        //b.modifiers.Add(new HomingModif());
        factory = new BulletFactory(unit)
            .SetBullet(b)
            .SetLife(life)
            .SetSpeed(speed)
            .SetDmgMask(LayerMask.GetMask("Player"))
            .AddModifier(new HomingModif());
    }

    public override void DoAttack()
    {
        factory.SetPosition(unit.transform.position).Build();
        cd = 2;
    }

    float cd;
    public override void Update()
    {
        cd -= Time.deltaTime;
        if (unit.controller.NeedAttack() && cd <= 0)
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