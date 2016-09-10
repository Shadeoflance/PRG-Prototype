using System;
using System.Collections;
using UnityEngine;

class TeleporterEnemy : Enemy
{
    public Transform circle;
    protected override void Start()
    {
        controller = new TeleporterController(this);
        jumper = new TeleportJumper(this, circle);
        airborne = new AirborneState(this);
        walking = airborne;
        currentState = airborne;
        health = new Health(this, hp);
        attack = new ProjAttack(this, true);
        base.Start();
    }
}

class TeleportJumper : Jumper
{
    float cd = 3;
    bool canJump = true;
    Transform circle;

    public TeleportJumper(Unit unit, Transform circle) : base(unit)
    {
        this.circle = circle;
    }

    public override void Jump()
    {
        if (!CanJump())
            return;
        unit.StartCoroutine(ResetCD());
        canJump = false;
        unit.transform.position = Level.instance.current.GetAirClearTilePos();
        circle.localScale = new Vector2(0.3f, 0.3f);
    }
    
    public void JumpNoCD()
    {
        unit.transform.position = Level.instance.current.GetAirClearTilePos();
    }

    IEnumerator ResetCD()
    {
        yield return new WaitForSeconds(cd / 3 * 2);
        unit.StartCoroutine(CircleGrow());
        yield return new WaitForSeconds(cd / 3);
        canJump = true;
    }

    IEnumerator CircleGrow()
    {
        float restCd = cd / 3;
        while(restCd > 0)
        {
            restCd -= Time.deltaTime;
            float scale = (1 - restCd / cd * 3) * 0.7f + 0.3f;
            circle.localScale = new Vector2(scale, scale);
            yield return null;
        }
    }
    protected override bool CanJump()
    {
        return canJump;
    }
}

class TeleporterController : IController
{
    TeleporterEnemy unit;
    public float distance = 5;

    public TeleporterController(TeleporterEnemy unit)
    {
        this.unit = unit;
    }

    public bool NeedAttack()
    {
        return Player.Distance(unit.transform.position) < distance;
    }

    public bool NeedJump()
    {
        return true;
    }

    public Vector2 NeedVel()
    {
        return Vector2.zero;
    }

    public void Update()
    {
    }
}
