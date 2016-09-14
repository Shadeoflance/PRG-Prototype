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
        CreateExplosion(LayerMask.GetMask("Player", "Enemy", "Level"), transform.position, effectiveDmg);
        Destroy(gameObject);
    }

    public static void CreateExplosion(int dmgMask, Vector2 position, float dmg = 3, float range = 2)
    {
        OrbExplosion.Create(position);
        Collider2D[] hits = Physics2D.OverlapCircleAll(position, range, dmgMask);
        HashSet<Unit> units = new HashSet<Unit>();
        foreach (var a in hits)
        {
            Unit u = a.GetComponent<Unit>();
            if (u != null)
            {
                units.Add(u);
                continue;
            }
            Tile t = a.GetComponent<Tile>();
            if (t != null)
            {
                t.ExplosionHit();
            }
        }

        GameObject source = new GameObject();
        source.transform.position = position;
        foreach (var u in units)
        {
            u.health.TakeDamage(dmg, source);
        }
        Destroy(source);
    }

    static Prefab prefab = new Prefab("Pickups/OrbBomb");

    public static void Drop(Vector2 pos)
    {
        GameObject instance = prefab.Instantiate();
        instance.transform.position = pos;
    }
}