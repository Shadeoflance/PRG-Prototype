using UnityEngine;

public class Tile : MonoBehaviour
{
    public int x, y;
    [System.NonSerialized]
    public SpritePainter painter;
    public Tiles tiles;
    [System.NonSerialized]
    public GameObject colObj = null;

    void Awake()
    {
        painter = GetComponent<SpritePainter>();
    }

    public virtual bool Collidable
    { get { return true; } }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            painter.Paint(new Color(0.6f, 1f, 0.6f), 0.5f, true);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.layer == LayerMask.NameToLayer("Bullet"))
        {
            Bullet b = col.GetComponent<Bullet>();
            if (b.unit.tag == "Player")
            {
                BulletHit(b);
            }
        }
    }

    string prefabPath = "Level/Tiles/";
    public virtual Tile Reinstantiate()
    {
        Tile t = Instantiate<GameObject>(Resources.Load<GameObject>(prefabPath + name)).GetComponent<Tile>();
        t.transform.parent = transform.parent;
        t.transform.localPosition = transform.localPosition;
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