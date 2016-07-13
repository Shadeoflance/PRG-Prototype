using UnityEngine;
using System.Collections;

class JumperEnemy : Enemy
{
    public SpriteRenderer triangle;
    protected override void Start()
    {
        controller = new JumperController(this);
        jumper = new EnemyJumper(this);
        health = new Health(this, hp);
        walking = new WalkingState(this);
        airborne = new AirborneState(this);
        mover = new DefaultMover(this, speed);
        attack = new Attack(this);
        base.Start();
    }
}

class EnemyJumper : Jumper
{
    float cd = 3;
    bool canJump = true;
    private Material triangle;

    public EnemyJumper(Unit unit) : base(unit)
    {
        SpriteRenderer sr = ((JumperEnemy)unit).triangle;
        triangle = GameObject.Instantiate<Material>(sr.material);
        sr.material = triangle;
    }

    public override void Jump()
    {
        if (!CanJump())
            return;
        canJump = false;
        unit.StartCoroutine(ResetCD());
        if(unit.currentState.Transit(new JumpingState(unit)))
        {
            unit.rb.AddForce(new Vector2(14f * unit.direction, 8f), ForceMode2D.Impulse);
            triangle.SetFloat("_Percentage", 0f);
            unit.StartCoroutine(ResetState());
            unit.StartCoroutine(RenderTriangle());
        }
    }

    IEnumerator ResetState()
    {
        yield return new WaitForSeconds(1f);
        unit.currentState = unit.walking;
    }

    IEnumerator RenderTriangle()
    {
        yield return new WaitForSeconds(cd / 3 * 2);
        float resetCD = 0;
        while(resetCD < cd / 3)
        {
            resetCD += Time.deltaTime;
            triangle.SetFloat("_Percentage", resetCD / cd * 3);
            yield return null;
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

class JumpingState : UnitState
{
    public JumpingState(Unit unit) : base(unit) { }

    public override void Move(Vector2 dir) { }

    public override void Jump() { }

    public override bool Transit(UnitState state)
    {
        if (state is WalkingState)
            return base.Transit(state);
        else return false;
    }
}

class JumperController : IController
{
    JumperEnemy unit;
    float timer = 1;
    Vector2 needPoint;

    public JumperController(JumperEnemy unit)
    {
        this.unit = unit;
    }

    public bool NeedAttack()
    {
        return false;
    }

    public bool NeedJump()
    {
        return Player.Distance(unit.transform.position) < 5;
    }


    public Vector2 NeedVel()
    {
        Vector2 dir = Utils.TrimY(needPoint - Utils.ToV2(unit.transform.position));
        if (dir.magnitude < 0.05)
            return Vector2.zero;
        return dir.normalized;
    }

    public void Update()
    {
        float dist = Player.Distance(unit.transform.position);
        if (dist < 5)
        {
            needPoint = Player.instance.transform.position;
            return;
        }
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            timer = 5;
            needPoint = Utils.ToV2(unit.transform.position) + new Vector2((Random.Range(0, 3) == 1 ? -1 : 1) * 3, 0);
        }
    }
}
