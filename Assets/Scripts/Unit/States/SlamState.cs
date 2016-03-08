using UnityEngine;
using System;
using System.Collections.Generic;

public class SlamState : PlayerState
{
    float speed, range;
    public SlamState(Unit unit, float speed, float range)
        : base(unit)
    {
        this.speed = speed;
        this.range = range;
        foreach (var a in player.GetComponents<Collider2D>())
            a.enabled = false;
        unit.rb.gravityScale = 0;
        unit.rb.velocity = Vector2.zero;
    }

    public override void Attack() { }
    public override void Damage(int amount) { }
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
        float delta = speed * Time.fixedDeltaTime;
        RaycastHit2D[] hit = Physics2D.BoxCastAll(player.transform.position, player.main.size / 2, 0,
            new Vector2(0, -1), delta, LayerMask.GetMask("Level"));

        if (hit.Length > 0)
        {
            player.transform.position = player.transform.position + new Vector3(0, -hit[0].distance, 0);
            Finish();
            return;
        }
        unit.transform.position = unit.transform.position + new Vector3(0, -delta, 0);
    }
    public void Finish()
    {
        player.rb.velocity = Vector3.zero;
        player.currentState = player.walking;
        foreach (var a in player.GetComponents<Collider2D>())
            a.enabled = true;
        player.rb.gravityScale = player.gravityScale;
        Vector2 pointA = VectorUtils.V3ToV2(player.transform.position) - player.main.size / 2 - new Vector2(range, 0),
            pointB = VectorUtils.V3ToV2(player.transform.position) + player.main.size / 2 + new Vector2(range, 0);
        Debug.DrawLine(pointA, pointB, Color.red, 2);
        Collider2D[] hits = Physics2D.OverlapAreaAll(pointA, pointB, LayerMask.GetMask("Enemy"));
        List<Enemy> enemies = new List<Enemy>();
        foreach (var a in hits)
            enemies.Add(a.GetComponent<Enemy>());
        ActionParams ap = new ActionParams();
        ap["enemies"] = enemies;
        player.eventManager.InvokeHandlers("slamFinish", ap);
    }
}