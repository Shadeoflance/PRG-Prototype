using UnityEngine;
using System;

public class DashState : PlayerState
{
    float speed, distance;
    Vector3 startPoint;
    public DashState(Unit unit, float speed, float distance) : base(unit) 
    {
        this.speed = speed;
        this.distance = distance;
        startPoint = unit.transform.position;
        foreach (var a in player.GetComponents<Collider2D>())
            a.enabled = false;
        unit.rb.gravityScale = 0;
        unit.rb.velocity = Vector2.zero;
    }

    public override void Attack() { }
    public override void TakeDamage(float amount, GameObject source) { }
    public override void Dash() { }
    public override void Jump() { }
    public override void Move(Vector2 dir) { }
    public override bool Transit(UnitState state) 
    {
        return false;
    }
    public override void Slam() { }
    public override void FixedUpdate()
    {
        Vector3 posDelta = new Vector3(unit.direction * speed * Time.fixedDeltaTime, 0, 0);
        RaycastHit2D[] hits = Physics2D.BoxCastAll(player.transform.position, player.main.size / 2, 0,
            new Vector2(player.direction, 0), Math.Abs(posDelta.x), LayerMask.GetMask("Enemy", "Level"));
        foreach (var a in hits)
        {
            if (a.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                ActionParams ap = new ActionParams();
                ap["enemy"] = a.collider.gameObject.GetComponent<Enemy>();
                player.eventManager.InvokeHandlers("dashPenetrateEnemy", ap);
            }
            else
            {
                player.transform.position = player.transform.position + new Vector3(a.distance * player.direction, 0, 0);
                Finish();
                return;
            }
        }
        //player.main.size = new Vector2(initialSize.x + Math.Abs(posDelta.x), initialSize.y);
        //player.main.offset = new Vector2(initialOffset.x + Math.Abs(posDelta.x) / 2, initialOffset.y);
        //if (!skippedFirstUpdate)
        //{
        //    skippedFirstUpdate = true;
        //    return;
        //}
        //if (player.main.IsTouchingLayers(1 << LayerMask.NameToLayer("Level")))
        //{
        //    Finish();
        //    return;
        //}
        unit.transform.position = unit.transform.position + posDelta;
        if (Vector3.Distance(startPoint, unit.transform.position) > distance)
            Finish();
    }
    public void Finish()
    {
        player.rb.velocity = Vector3.zero;
        player.currentState = player.walking;
        foreach (var a in player.GetComponents<Collider2D>())
            a.enabled = true;
        player.rb.gravityScale = player.gravityScale;
        player.eventManager.InvokeHandlers("dashFinish", null);
    }
}