using UnityEngine;

class Placeholder : Tile
{
    public override bool Collidable
    {
        get
        {
            return false;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }
}