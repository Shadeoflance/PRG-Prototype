using UnityEngine;

class TestFlyingEnemy : Enemy
{
    void Start()
    {
        controller = new TFEController(this);
        mover = new Flyer(this, speed);
        health = new Health(this, 2);
        walking = new WalkingState(this);
        airborne = new AirborneState(this);
    }
}

class TFEController : IController
{
    float timer = 1;
    Unit unit;

    public TFEController(Unit unit)
    {
        this.unit = unit;
    }

    public bool NeedAttack()
    {
        return false;
    }

    public bool NeedJump()
    {
        return false;
    }

    public Vector2 NeedVel()
    {
        if (Player.instance == null)
            return Vector2.zero;
        return VectorUtils.V3ToV2(Player.instance.transform.position - unit.transform.position).normalized;
    }

    public void Update()
    {
    }
}