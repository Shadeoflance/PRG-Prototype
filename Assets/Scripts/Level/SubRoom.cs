using UnityEngine;

class SubRoom : MonoBehaviour
{
    public Door right, left, top, bot;

    public void DoorTouch(Door d)
    {
        if (d == right)
            Level.instance.ChangeRoom(new Vector2(1, 0));
        if(d == left)
            Level.instance.ChangeRoom(new Vector2(-1, 0));
        if (d == top)
            Level.instance.ChangeRoom(new Vector2(0, 1));
        if (d == bot)
            Level.instance.ChangeRoom(new Vector2(0, -1));
    }

    public void Disable()
    {
        right.Close();
        left.Close();
        top.Close();
        bot.Close();
        gameObject.SetActive(false);
    }

    public void Enable()
    {
        gameObject.SetActive(true);
    }
}