using UnityEngine;
using System;

public class Item : MonoBehaviour
{
    Player player;
    Action<Player> getAction;

    void Awake()
    {
        getAction = (player) =>
        {
            if (player.jumper is MultipleJumper)
            {
                MultipleJumper jumper = (MultipleJumper)player.jumper;
                jumper.extraJumps++;
            }
            else if (player.jumper is DefaultJumper)
            {
                DefaultJumper jumper = (DefaultJumper)player.jumper;
                player.jumper = new MultipleJumper(player, jumper.force, jumper.maxHeight, 1);
            }
        };
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            getAction(collision.gameObject.GetComponent<Player>());
            Destroy(gameObject);
        }
    }
}