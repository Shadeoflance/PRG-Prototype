using UnityEngine;

public class TestEnemy : Enemy
{
    void Start()
    {
        controller = new TestEnemyController(this);
        mover = new DefaultMover(this, speed);
        health = new Health(this, 2);
        walking = new WalkingState(this);
        airborne = new AirborneState(this);
    }
}

class TestEnemyController : IController
{
    float timer = 1;
    Unit unit;

    public TestEnemyController(Unit unit)
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
        return new Vector2(unit.direction, 0);
    }

    public void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            unit.direction *= -1;
            timer = 1;
        }
    }
}