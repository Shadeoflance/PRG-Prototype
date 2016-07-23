using UnityEngine;

class Tile : MonoBehaviour
{
    public int x, y;
    SpritePainter painter;
    [System.NonSerialized]
    public Tiles tiles;

    void Awake()
    {
        painter = GetComponent<SpritePainter>();
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            painter.Paint(new Color(0.6f, 1f, 0.6f), 0.5f, true);
        }
    }

    string prefabPath = "Level/Tiles/";
    public Tile Reinstantiate()
    {
        Tile t = Instantiate<GameObject>(Resources.Load<GameObject>(prefabPath + name)).GetComponent<Tile>();
        t.transform.position = transform.position;
        t.transform.localPosition = transform.localPosition;
        t.transform.parent = transform.parent;
        Destroy(gameObject);
        return t;
    }

    public virtual void BulletHit(Bullet bullet) { }
    public virtual void ExplosionHit() { }
    protected virtual void Destroy()
    {
        tiles.DestroyTile(this);
    }
}