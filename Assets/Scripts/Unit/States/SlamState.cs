using UnityEngine;
using System;
using System.Collections.Generic;

public class SlamState : PlayerState
{
    float speed, range;
    static TrailRenderer trail;
    public SlamState(Unit unit, float speed, float range)
        : base(unit)
    {
        this.speed = speed;
        this.range = range;
        foreach (var a in player.GetComponents<Collider2D>())
            a.enabled = false;
        unit.rb.gravityScale = 0;
        unit.rb.velocity = Vector2.zero;

        if (trail == null)
            trail = player.transform.FindChild("SlamTrail").GetComponent<TrailRenderer>();
        trail.Clear();
        trail.enabled = true;
    }

    public override void Attack() { }
    public override void TakeDamage(float amount, GameObject source, bool invertBump) { }
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
            Finish(hit[0].collider.gameObject);
            return;
        }
        unit.transform.position = unit.transform.position + new Vector3(0, -delta, 0);
    }
    public void Finish(GameObject o)
    {
        Tile t = o.GetComponent<Tile>();
        LightupTiles(t);

        trail.enabled = false;
        player.rb.velocity = Vector3.zero;
        player.currentState = player.walking;
        foreach (var a in player.GetComponents<Collider2D>())
            a.enabled = true;
        player.rb.gravityScale = player.gravityScale;
        Vector2 pointA = Utils.ToV2(player.transform.position) - player.main.size / 2 - new Vector2(range, 0),
            pointB = Utils.ToV2(player.transform.position) + player.main.size / 2 + new Vector2(range, 0);
        //Debug.DrawLine(pointA, pointB, Color.red, 2);
        Collider2D[] hits = Physics2D.OverlapAreaAll(pointA, pointB, LayerMask.GetMask("Enemy"));
        HashSet<Enemy> enemies = new HashSet<Enemy>();
        foreach (var a in hits)
            enemies.Add(a.GetComponent<Enemy>());
        ActionParams ap = new ActionParams();
        ap["enemies"] = enemies;
        player.eventManager.InvokeHandlers("slamFinish", ap);
        player.eventManager.InvokeHandlers("land", null);
    }

    void LightupTiles(Tile t)
    {
        if (t == null)
            return;
        for(int i = t.x; i < t.tiles.map.GetLength(0); i++)
        {
            Tile k = t.tiles.map[i, t.y];
            if (k == null || (k.transform.position - t.transform.position).magnitude > range)
            {
                break;
            }
            k.painter.Paint(new Color(1f, 0.9f, 0f), 1f, true);
        }
        for (int i = t.x - 1; i > 0; i--)
        {
            Tile k = t.tiles.map[i, t.y];
            if (k == null || (k.transform.position - t.transform.position).magnitude > range)
                break;
            k.painter.Paint(new Color(1f, 0.9f, 0f), 1f, true);
        }
    }
}