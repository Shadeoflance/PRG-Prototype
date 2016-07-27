using UnityEngine;

class JumperBoss : JumperEnemy
{
    protected override void Start()
    {
        base.Start();
        ((EnemyJumper)jumper).force = new Vector2(20, 100);
        attack = new ProjAttack(this, true);
        ProjAttack a = (ProjAttack)attack;
        a.life = 8;
        a.speed = 2;
        a.cd = 3;
        controller = new JumperController(this, 20);
    }
    protected override void SetModifier()
    {
        EliteModifiers.SetRandomModifier(this);
    }
}