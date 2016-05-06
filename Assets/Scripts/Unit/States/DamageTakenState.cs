using UnityEngine;

public class PlayerDamageTakenState : PlayerState
{
    private static float speed = 700, duration = 0.25f;
    private float currentDuration;
    public PlayerDamageTakenState(Unit unit, int direction)
        : base(unit)
    {
        currentDuration = duration;
        unit.rb.AddForce(Vector2.up.Rotate(-direction * Mathf.PI / 5) * speed);
        unit.transform.Find("Sprite").GetComponent<Animator>().SetTrigger("Dmg");
    }
    public override void FixedUpdate()
    {
        currentDuration -= Time.deltaTime;
        if (currentDuration < 0)
            unit.currentState = unit.walking;
    }
    public override void Jump() { }
    public override void Attack() { }
    public override void Move(Vector2 dir) { }

    public override bool Transit(UnitState state)
    {
        if (state is PlayerStunnedState)
            return base.Transit(state);
        return false;
    }
}
public class DamageTakenState : UnitState
{
    private float speed = 500, duration = 0.25f;
    private float currentDuration;
    public DamageTakenState(Unit unit, int direction)
        : base(unit)
    {
        currentDuration = duration;
        unit.rb.AddForce(Vector2.up.Rotate(-direction * Mathf.PI / 5) * speed);
    }
    public override void FixedUpdate()
    {
        currentDuration -= Time.deltaTime;
        if (currentDuration < 0)
            unit.currentState = unit.walking;
    }
    public override void Jump() { }
    public override void Attack() { }
    public override void Move(Vector2 dir) { }

    public override bool Transit(UnitState state)
    {
        if (state is StunnedState)
            return base.Transit(state);
        return false;
    }
}