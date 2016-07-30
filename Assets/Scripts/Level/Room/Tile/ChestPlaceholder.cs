using UnityEngine;

class ChestPlaceholder : Tile
{
    static Prefab chest = new Prefab("Pickups/Chest");
    void Start()
    {
        if(Random.Range(0f, 1f) > 0.8f)
            SpawnChest();
    }
    public void SpawnChest()
    {
        GameObject c = chest.Instantiate();
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
}