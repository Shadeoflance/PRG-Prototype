using UnityEngine;

class ExplosibleTile : Tile
{
    void Start()
    {
        GetComponent<SpritePainter>().Paint(new PulsingChanger(new Color(0.4f, 0.4f, 0.4f), new Color(0.3f, 0.3f, 0.3f), null, 1.3f));
    }
    public override void ExplosionHit()
    {
        Destroy();
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}