using UnityEngine;

class ChestPlaceholder : Tile
{
    static Prefab chest = new Prefab("Pickups/Chest");
    public float spawnChance = 0.8f;
    public int id;
    void Start()
    {
        if(Random.Range(0f, 1f) < spawnChance)
            SpawnChest();
    }
    public void SpawnChest()
    {
        GameObject c = chest.Instantiate();
        c.GetComponent<Chest>().id = id;
        c.transform.parent = transform.parent.parent;
        c.transform.localPosition = transform.localPosition;
    }
    public override bool Collidable
    {
        get
        {
            return false;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }


    public override Tile Reinstantiate()
    {
        Tile t = base.Reinstantiate();
        (t as ChestPlaceholder).spawnChance = spawnChance;
        (t as ChestPlaceholder).id = id;
        return t;
    }
}