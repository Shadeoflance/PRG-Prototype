using UnityEngine;

class SubRoom : MonoBehaviour
{
    public Door right, left;

    public void DoorTouch(Door d)
    {
        if (d == right)
            Level.instance.ChangeRoom(new Vector2(1, 0));
        if(d == left)
            Level.instance.ChangeRoom(new Vector2(-1, 0));
    }
}