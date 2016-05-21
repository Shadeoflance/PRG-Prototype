using UnityEngine;
using System.Collections.Generic;

class OrbBomb : MonoBehaviour
{
    public float fuseTime = 2;
    public float range = 2;
    public float dmgMult = 1;
    public float baseDmg = 3;
    public float effectiveDmg { get { return baseDmg * dmgMult; } }

    void Update()
    {
        fuseTime -= Time.deltaTime;
        if (fuseTime < 0)
            Explode();
    }

    void Explode()
    {
        OrbExplosion.Create(transform.position);
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, range, LayerMask.GetMask("Player", "Enemy", "Level"));
        HashSet<Enemy> enemies = new HashSet<Enemy>();
        bool didPlayer = false;
        foreach(var a in hits)
        {
            Enemy e = a.GetComponent<Enemy>();
            if(e != null)
            {
                enemies.Add(e);
                continue;
            }
            if(!didPlayer && a.GetComponent<Player>() != null)
            {
                didPlayer = true;
                Player.instance.health.TakeDamage(effectiveDmg, gameObject);
                continue;
            }
            Tile t = a.GetComponent<Tile>();
            if(t != null)
            {
                t.ExplosionHit();
            }
        }
        foreach(var e in enemies)
        {
            e.health.TakeDamage(effectiveDmg, gameObject);
        }
        Destroy(gameObject);
    }

    static Prefab prefab = new Prefab("Pickups/OrbBomb");

    public static void Drop(Vector2 pos)
    {
        GameObject instance = prefab.Instantiate();
        instance.transform.position = pos;
    }
}