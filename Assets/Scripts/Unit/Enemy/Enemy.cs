using UnityEngine;

public class Enemy : Unit
{
    protected void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            collision.gameObject.GetComponent<Player>().currentState.DealDamage(damage, gameObject);
        }
    }
}