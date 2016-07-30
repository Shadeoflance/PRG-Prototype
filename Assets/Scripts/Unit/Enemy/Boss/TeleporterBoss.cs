using UnityEngine;
using System.Collections;

class TeleporterBoss : TeleporterEnemy
{
    protected override void Start()
    {
        base.Start();
        ProjAttack a = (ProjAttack)attack;
        a.life = 8;
        a.cd = 3;
        (controller as TeleporterController).distance = 20;
        StartCoroutine(Burst());
    }
    protected override void SetModifier()
    {
        EliteModifiers.SetRandomModifier(this);
    }
    IEnumerator Burst()
    {
        yield return new WaitForSeconds(6);
        for(int i = 0; i < 5; i++)
        {
            attack.DoAttack();
            (jumper as TeleportJumper).JumpNoCD();
            yield return new WaitForSeconds(0.1f);
        }
        StartCoroutine(Burst());
    }
}