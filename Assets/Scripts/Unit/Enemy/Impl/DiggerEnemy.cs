using System;
using System.Collections;
using UnityEngine;

class DiggerEnemy : Enemy
{
    protected override void Start()
    {
        controller = new DiggerController(this);
        health = new Health(this);
        walking = new WalkingState(this);
        airborne = walking;
        currentState = walking;
        jumper = new DigJumper(this);
        attack = new ProjAttack(this);

        base.Start();
    }
}
class DigJumper : Jumper
{
    float cd = 4;
    bool canJump = false;
    public DigJumper(Unit unit) : base(unit)
    {
        unit.StartCoroutine(ResetCD());
    }
    public override void Jump()
    {
        if (!CanJump())
            return;
        if(unit.currentState.Transit(new DiggingState(unit)))
        {
            canJump = false;
            unit.StartCoroutine(ResetCD());
        }
    }

    IEnumerator ResetCD()
    {
        yield return new WaitForSeconds(cd);
        canJump = true;
    }

    protected override bool CanJump()
    {
        return canJump;
    }
}
class DiggingState : UnitState
{
    float totalTime = 1, curTime = 0;
    bool started = false;

    public DiggingState(Unit unit) : base(unit)
    {
    }

    public override void Jump() { }
    public override void Attack() { }
    public override void Move(Vector2 dir) { }
    public override void TakeDamage(float amount, GameObject source, bool invertBump) { }
    public override bool Transit(UnitState state)
    {
        return false;
    }

    public override void Update()
    {
        base.Update();
        if(!started)
        {
            unit.StartCoroutine(StartDig());
            started = true;
        }
    }

    IEnumerator StartDig()
    {
        while(curTime < totalTime / 2)
        {
            unit.transform.position -= new Vector3(0, Time.deltaTime);
            curTime += Time.deltaTime;
            yield return null;
        }
        ChangeLocation();
        unit.StartCoroutine(EndDig());
    }
    IEnumerator EndDig()
    {
        while(curTime < totalTime)
        {
            unit.transform.position += new Vector3(0, Time.deltaTime);
            curTime += Time.deltaTime;
            yield return null;
        }
        unit.currentState = unit.walking;
    }
    void ChangeLocation()
    {
        Vector2 v;
        int k = 0;
        do
        {
            k++;
            v = Level.instance.current.GetTopClearTilePos();
        }
        while (Player.Distance(v) > 8 && k < 300);
        unit.transform.position = v;
    }
}
class DiggerController : IController
{
    DiggerEnemy unit;

    public DiggerController(DiggerEnemy unit)
    {
        this.unit = unit;
    }

    public bool NeedAttack()
    {
        return Player.Distance(unit.transform.position) < 5;
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