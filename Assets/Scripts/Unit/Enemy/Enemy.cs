using UnityEngine;

public class Enemy : Unit
{
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<Player>().currentState.DealDamage(damage, gameObject);
        }
    }
}